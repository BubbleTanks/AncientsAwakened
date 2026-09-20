using AncientsAwakened.AncientsAwakenedCode.Cards.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class ShadowCrystal : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private const string StarterCardKey = "StarterCard";
    private const string AncientCardKey = "AncientCard";

    private List<IHoverTip> _extraHoverTips = [];

    private SerializableCard? _serializableStarterCard;
    private SerializableCard? _serializableAncientCard;

    private static Dictionary<ModelId, ModelId>? _shadowCrystalCards;
    
    private static Dictionary<ModelId, ModelId> VanillaShadowCrystalCards => new()
    {
        {
            ModelDb.Card<Bash>().Id,
            ModelDb.Card<Pulverize>().Id
        },
        {
            ModelDb.Card<Survivor>().Id,
            ModelDb.Card<Vestige>().Id
        },
        {
            ModelDb.Card<Venerate>().Id,
            ModelDb.Card<Revere>().Id
        },
        {
            ModelDb.Card<Bodyguard>().Id,
            ModelDb.Card<Guardian>().Id
        },
        {
            ModelDb.Card<Zap>().Id,
            ModelDb.Card<Lucid>().Id
        }
    };
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => _extraHoverTips;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(StarterCardKey), new StringVar(AncientCardKey)];
    
    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return SetupForPlayer(owner);
    }

    private static Dictionary<ModelId, ModelId> ShadowCrystalCards
    {
        get
        {
            if (_shadowCrystalCards == null)
            {
                _shadowCrystalCards = new Dictionary<ModelId, ModelId>();
                foreach (var kv in VanillaShadowCrystalCards)
                {
                    _shadowCrystalCards.Add(kv.Key, kv.Value);
                }
                foreach (var kv in CustomShadowCrystalCardExtension.CustomShadowCrystalCards)
                {
                    _shadowCrystalCards.Add(kv.Key, kv.Value);
                }
            }
            return _shadowCrystalCards;
        }
    }

    public override bool HasUponPickupEffect => true;

    [SavedProperty]
    private SerializableCard? StarterCard
    {
        get => _serializableStarterCard;
        set
        {
            AssertMutable();
            _serializableStarterCard = value;
            UpdateHoverTips();
        }
    }

    [SavedProperty]
    private SerializableCard? AncientCard
    {
        get => _serializableAncientCard;
        set
        {
            AssertMutable();
            _serializableAncientCard = value;
            UpdateHoverTips();
        }
    }

    public bool SetupForPlayer(Player player)
    {
        if (player == null)
            return false;
        
        AssertMutable();
        
        var starter = GetShadowStarterCard(player);
        if (starter == null)
            return false;
            
        StarterCard = starter.ToSerializable();
        AncientCard = GetShadowCrystalCard(starter).ToSerializable();
        
        UpdateHoverTips();
        return true;
    }

    public override async Task AfterObtained()
    {
        var starterCard = GetShadowStarterCard(Owner);
        if(starterCard == null)
            return;
        await CardCmd.Transform(starterCard, GetShadowCrystalCard(starterCard));
    }

    private static CardModel? GetShadowStarterCard(Player player) => player.Deck.Cards.FirstOrDefault(c => ShadowCrystalCards.ContainsKey(c.Id));
    
    private CardModel GetShadowCrystalCard(CardModel starterCard)
    {
        ModelId? replacement = null;
        if (ShadowCrystalCards.TryGetValue(starterCard.Id, out var shadowCard))
        {
            replacement = shadowCard;
        } 
        
        if (replacement == null) 
            return Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
        
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
    
    protected override void AfterCloned()
    {
        base.AfterCloned();
        _extraHoverTips = [];
    }

    private void UpdateHoverTips()
    {
        _extraHoverTips.Clear();
        if (StarterCard != null)
        {
            var card = CardModel.FromSerializable(StarterCard);
            _extraHoverTips.AddRange(card.HoverTips);
            _extraHoverTips.Add(HoverTipFactory.FromCard(card));
            ((StringVar)DynamicVars[AncientCardKey]).StringValue = card.Title;
        }
        if (AncientCard != null)
        {
            var card = CardModel.FromSerializable(AncientCard);
            _extraHoverTips.AddRange(card.HoverTips);
            _extraHoverTips.Add(HoverTipFactory.FromCard(card));
            ((StringVar)DynamicVars[AncientCardKey]).StringValue = card.Title;
        }
    }
}