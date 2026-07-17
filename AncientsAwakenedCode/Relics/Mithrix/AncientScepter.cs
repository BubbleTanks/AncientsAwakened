using AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Saves;
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
        var basicCount = player.Deck.Cards.Count(c => c.IsBasicStrikeOrDefend);
        if (basicCount >= 3)
        {
            var strikeCard = GetBasicStrikeCard(player);
            var defendCard = GetBasicDefendCard(player);
            if (strikeCard != null)
            {
                StrikeCard = PerfectedStrikeUpgrades.TryGetValue(strikeCard.Id, out var perfectedCard) ? 
                    ModelDb.GetById<CardModel>(perfectedCard).ToMutable().ToSerializable() : 
                    ModelDb.Card<PerfectStrike>().ToMutable().ToSerializable();
            }

            if (defendCard != null)
            {
                DefendCard = PerfectedDefendUpgrades.TryGetValue(defendCard.Id, out var perfectedCard) ? 
                    ModelDb.GetById<CardModel>(perfectedCard).ToMutable().ToSerializable() : 
                    ModelDb.Card<PerfectDefend>().ToMutable().ToSerializable();
            }
            UpdateHoverTips();
            return true;
        }
        return false;
    }

    public override async Task AfterObtained()
    {
        var transformations = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsBasicStrikeOrDefend && c.IsRemovable).ToList()
            .Select(c => new CardTransformation(c, GetPerfectedTransformedCard(c)));
        var list = (await CardCmd.Transform(transformations, null, CardPreviewStyle.None)).ToList();
        if (list.Count > 0 && LocalContext.IsMe(Owner))
        {
            NSimpleCardsViewScreen.ShowScreen(list, new LocString("relics", "ANCIENTSAWAKENED-ANCIENT_SCEPTER.infoText"));
        }
        if(!SaveManager.Instance.Progress.DiscoveredCards.Contains(ModelDb.Card<PerfectStrike>().Id)) SaveManager.Instance.MarkCardAsSeen(ModelDb.Card<PerfectStrike>());
        if(!SaveManager.Instance.Progress.DiscoveredCards.Contains(ModelDb.Card<PerfectDefend>().Id)) SaveManager.Instance.MarkCardAsSeen(ModelDb.Card<PerfectDefend>());
    }
    
    private static CardModel? GetBasicStrikeCard(Player player) => player.Deck.Cards.FirstOrDefault(c => c.Tags.Contains(CardTag.Strike) && c.IsBasicStrikeOrDefend);
    private static CardModel? GetBasicDefendCard(Player player) => player.Deck.Cards.FirstOrDefault(c => c.Tags.Contains(CardTag.Defend) && c.IsBasicStrikeOrDefend);

    private CardModel GetPerfectedTransformedCard(CardModel starterCard)
    {
        ModelId? replacement;
        if (starterCard.Tags.Contains(CardTag.Strike))
        {
            replacement = PerfectedStrikeUpgrades.TryGetValue(starterCard.Id, out var perfectedCard)
                ? perfectedCard
                : ModelDb.Card<PerfectStrike>().Id;
        } 
        else if (starterCard.Tags.Contains(CardTag.Defend))
        {
            replacement = PerfectedDefendUpgrades.TryGetValue(starterCard.Id, out var perfectedCard)
                ? perfectedCard
                : ModelDb.Card<PerfectDefend>().Id;
        }
        else
        {
            replacement = null;
            AncientsAwakenedMain.Logger.Warn($"Card that is not Strike or Defend {starterCard.Id} attempted to be transformed by Ancient Scepter.");
        }

        if (replacement == null) return Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
        
        var cardModel = starterCard.Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(replacement), starterCard.Owner);
        if (starterCard.IsUpgraded)
        {
            CardCmd.Upgrade(cardModel);
        }
        if (starterCard.Enchantment != null)
        {
            var enchantmentModel = (EnchantmentModel)starterCard.Enchantment.MutableClone();
            CardCmd.Enchant(enchantmentModel, cardModel, enchantmentModel.Amount);
        }
        return cardModel;
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
            var card = CardModel.FromSerializable(StrikeCard);
            _extraHoverTips.AddRange(card.HoverTips);
            _extraHoverTips.Add(HoverTipFactory.FromCard(card));
            ((StringVar)DynamicVars[_perfectedPrefixKey]).StringValue = GetPrefixString(card.Title);
        }
        if (DefendCard != null)
        {
            var card = CardModel.FromSerializable(DefendCard);
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
    private static string GetPrefixString(string title)
    {
        var strikeTitle = new LocString("cards", "STRIKE_IRONCLAD.title").GetFormattedText();
        var defendTitle = new LocString("cards", "DEFEND_IRONCLAD.title").GetFormattedText();
        return title.Replace(strikeTitle, "").Replace(defendTitle, "");
    }
}