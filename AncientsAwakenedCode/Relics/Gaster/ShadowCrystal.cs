using AncientsAwakened.AncientsAwakenedCode.Cards.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class ShadowCrystal : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private const string AncientCardKey = "AncientCard";

    private IEnumerable<IHoverTip> _extraHoverTips = [];

    private ModelId? _ancientCard;

    private static Dictionary<ModelId, ModelId>? _shadowCrystalCards;
    
    private static Dictionary<ModelId, ModelId> VanillaShadowCrystalCards => new()
    {
        {
            ModelDb.Character<Ironclad>().Id,
            ModelDb.Card<Pulverize>().Id
        },
        {
            ModelDb.Character<Silent>().Id,
            ModelDb.Card<Vestige>().Id
        },
        {
            ModelDb.Character<Regent>().Id,
            ModelDb.Card<Revere>().Id
        },
        {
            ModelDb.Character<Necrobinder>().Id,
            ModelDb.Card<Guardian>().Id
        },
        {
            ModelDb.Character<Defect>().Id,
            ModelDb.Card<Lucid>().Id
        }
    };
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => _extraHoverTips;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(AncientCardKey)];
    
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
    private ModelId? AncientCard
    {
        get => _ancientCard;
        set
        {
            AssertMutable();
            _ancientCard = value;
            if (_ancientCard != null)
                return;
            var card = SaveUtil.CardOrDeprecated(_ancientCard);
            _extraHoverTips = card.HoverTips.Concat([HoverTipFactory.FromCard(card)]);
            ((StringVar) DynamicVars[AncientCardKey]).StringValue = card.Title;
        }
    }

    public bool SetupForPlayer(Player player)
    {
        if (player == null)
            return false;
        
        AssertMutable();
        
        AncientCard = GetShadowCrystalCard(player).Id;
        return true;
    }

    public override async Task AfterObtained()
    {
        if (AncientCard == null)
            SetupForPlayer(Owner);
        var card = Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(AncientCard), Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
    }
    
    private CardModel GetShadowCrystalCard(Player owner)
    {
        ModelId? replacement = null;
        if (ShadowCrystalCards.TryGetValue(owner.Character.Id, out var shadowCard))
        {
            replacement = shadowCard;
        } 
        
        if (replacement == null) 
            return Owner.RunState.CreateCard<Doubt>(owner);
        
        var cardModel = owner.RunState.CreateCard(ModelDb.GetById<CardModel>(replacement), owner);
        
        return cardModel;
    }
    
    protected override void AfterCloned()
    {
        base.AfterCloned();
        _extraHoverTips = [];
    }
}