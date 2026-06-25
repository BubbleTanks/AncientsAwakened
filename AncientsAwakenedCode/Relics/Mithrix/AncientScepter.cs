using AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public class AncientScepter : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private const string _perfectedPrefixKey = "PerfectedPrefixKey";

    private List<IHoverTip> _extraHoverTips = [];

    private SerializableCard? _serializableStrikeCard;
    private SerializableCard? _serializableDefendCard;

    private static Dictionary<ModelId, ModelId>? _perfectedStrikeUpgrades;
    private static Dictionary<ModelId, ModelId>? _perfectedDefendUpgrades;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => _extraHoverTips;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(_perfectedPrefixKey)];
    
    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return SetupForPlayer(owner);
    }

    private static Dictionary<ModelId, ModelId> PerfectedStrikeUpgrades
    {
        get
        {
            if (_perfectedStrikeUpgrades == null)
            {
                _perfectedStrikeUpgrades = new Dictionary<ModelId, ModelId>();
                foreach (var kv in VanillaPerfectedStrikeUpgrades)
                {
                    _perfectedStrikeUpgrades.Add(kv.Key, kv.Value);
                }
                foreach (var kv in CustomPerfectedCardExtension.CustomPerfectedStrikeCards)
                {
                    _perfectedStrikeUpgrades.Add(kv.Key, kv.Value);
                }
            }
            return _perfectedStrikeUpgrades;
        }
    }
    
    private static Dictionary<ModelId, ModelId> PerfectedDefendUpgrades
    {
        get
        {
            if (_perfectedDefendUpgrades == null)
            {
                _perfectedDefendUpgrades = new Dictionary<ModelId, ModelId>();
                foreach (var kv in VanillaPerfectedDefendUpgrades)
                {
                    _perfectedDefendUpgrades.Add(kv.Key, kv.Value);
                }
                foreach (var kv in CustomPerfectedCardExtension.CustomPerfectedDefendCards)
                {
                    _perfectedDefendUpgrades.Add(kv.Key, kv.Value);
                }
            }
            return _perfectedDefendUpgrades;
        }
    }

    public override bool HasUponPickupEffect => true;

    [SavedProperty]
    private SerializableCard? StrikeCard
    {
        get => _serializableStrikeCard;
        set
        {
            AssertMutable();
            _serializableStrikeCard = value;
            UpdateHoverTips();
        }
    }

    [SavedProperty]
    private SerializableCard? DefendCard
    {
        get => _serializableDefendCard;
        set
        {
            AssertMutable();
            _serializableDefendCard = value;
            UpdateHoverTips();
        }
    }

    public bool SetupForPlayer(Player player)
    {
        if (player == null)
        {
            return false;
        }
        
        AssertMutable(); 
        var (strikeCard, strikeCount) = GetPerfectedStrikeCard(player);
        var (defendCard, defendCount) = GetPerfectedDefendCard(player);
        if (strikeCount + defendCount >= 3)
        {
            if (strikeCard != null && PerfectedStrikeUpgrades.TryGetValue(strikeCard.Id, out var perfectedStrikeCard))
                StrikeCard = ModelDb.GetById<CardModel>(perfectedStrikeCard).ToMutable().ToSerializable();
            if (defendCard != null && PerfectedDefendUpgrades.TryGetValue(defendCard.Id, out var perfectedDefendCard))
                DefendCard =  ModelDb.GetById<CardModel>(perfectedDefendCard).ToMutable().ToSerializable();
            UpdateHoverTips();
            return true;
        }
        return false;
    }

    public override async Task AfterObtained()
    {
        IEnumerable<CardTransformation> transformations = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsBasicStrikeOrDefend && c.IsRemovable).ToList()
            .Select(c => new CardTransformation(c, GetPerfectedTransformedCard(c)));
        List<CardPileAddResult> list = (await CardCmd.Transform(transformations, null, CardPreviewStyle.None)).ToList();
        if (list.Count > 0 && LocalContext.IsMe(Owner))
        {
            NSimpleCardsViewScreen.ShowScreen(list, new LocString("relics", "ANCIENTSAWAKENED-ANCIENT_SCEPTER.infoText"));
        }
    }
    
    private static (CardModel?,int) GetPerfectedStrikeCard(Player player) => (player.Deck.Cards.FirstOrDefault(c => PerfectedStrikeUpgrades.ContainsKey(c.Id)), player.Deck.Cards.Count(c => PerfectedStrikeUpgrades.ContainsKey(c.Id)));
    private static (CardModel?,int) GetPerfectedDefendCard(Player player) => (player.Deck.Cards.FirstOrDefault(c => PerfectedDefendUpgrades.ContainsKey(c.Id)), player.Deck.Cards.Count(c => PerfectedDefendUpgrades.ContainsKey(c.Id)));

    private CardModel GetPerfectedTransformedCard(CardModel starterCard)
    {
        ModelId replacement = PerfectedStrikeUpgrades.TryGetValue(starterCard.Id, out replacement) ? replacement : PerfectedDefendUpgrades.TryGetValue(starterCard.Id, out replacement) ? replacement : null;
        if (replacement != null)
        {
            CardModel cardModel = starterCard.Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(replacement), starterCard.Owner);
            if (starterCard.IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }
            if (starterCard.Enchantment != null)
            {
                EnchantmentModel enchantmentModel = (EnchantmentModel)starterCard.Enchantment.MutableClone();
                CardCmd.Enchant(enchantmentModel, cardModel, enchantmentModel.Amount);
            }
            return cardModel;
        }
        return Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
    }
    
    private static Dictionary<ModelId, ModelId> VanillaPerfectedStrikeUpgrades => new()
    {
        {
            ModelDb.Card<StrikeIronclad>().Id,
            ModelDb.Card<DemonicStrike>().Id
        },
        {
            ModelDb.Card<StrikeSilent>().Id,
            ModelDb.Card<DeadlyStrike>().Id
        },
        {
            ModelDb.Card<StrikeRegent>().Id,
            ModelDb.Card<CosmicStrike>().Id
        },
        {
            ModelDb.Card<StrikeNecrobinder>().Id,
            ModelDb.Card<EternalStrike>().Id
        },
        {
            ModelDb.Card<StrikeDefect>().Id,
            ModelDb.Card<EmpoweredStrike>().Id
        }
    };
    
    private static Dictionary<ModelId, ModelId> VanillaPerfectedDefendUpgrades => new()
    {
        {
            ModelDb.Card<DefendIronclad>().Id,
            ModelDb.Card<DemonicDefend>().Id
        },
        {
            ModelDb.Card<DefendSilent>().Id,
            ModelDb.Card<DeadlyDefend>().Id
        },
        {
            ModelDb.Card<DefendRegent>().Id,
            ModelDb.Card<CosmicDefend>().Id
        },
        {
            ModelDb.Card<DefendNecrobinder>().Id,
            ModelDb.Card<EternalDefend>().Id
        },
        {
            ModelDb.Card<DefendDefect>().Id,
            ModelDb.Card<EmpoweredDefend>().Id
        }
    };

    protected override void AfterCloned()
    {
        base.AfterCloned();
        _extraHoverTips = [];
    }

    private void UpdateHoverTips()
    {
        _extraHoverTips.Clear();
        if (StrikeCard != null)
        {
            CardModel card = CardModel.FromSerializable(StrikeCard);
            _extraHoverTips.AddRange(card.HoverTips);
            _extraHoverTips.Add(HoverTipFactory.FromCard(card));
            ((StringVar)DynamicVars[_perfectedPrefixKey]).StringValue = GetPrefixString(card.Title);
        }
        if (DefendCard != null)
        {
            CardModel card = CardModel.FromSerializable(DefendCard);
            _extraHoverTips.AddRange(card.HoverTips);
            _extraHoverTips.Add(HoverTipFactory.FromCard(card));
            ((StringVar)DynamicVars[_perfectedPrefixKey]).StringValue = GetPrefixString(card.Title);
        }
    }

    /// <summary>
    /// Note for Localizers:
    /// Please keep in mind that the space will be included in the replacement card titles to avoid possible localization issues.
    /// Reflect this in the Ancient Scepter's description.
    /// </summary>
    private String GetPrefixString(String title)
    {
        var strikeTitle = new LocString("cards", "STRIKE_IRONCLAD.title").GetFormattedText();
        var defendTitle = new LocString("cards", "DEFEND_IRONCLAD.title").GetFormattedText();
        return title.Replace(strikeTitle, "").Replace(defendTitle, "");
    }
}