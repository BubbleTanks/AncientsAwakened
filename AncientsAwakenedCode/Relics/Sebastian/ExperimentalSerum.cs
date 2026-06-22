using AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;

[Pool(typeof(EventRelicPool))]
public class ExperimentalSerum : AncientsAwakenedRelic
{
    private static Dictionary<ModelId, ModelId>? _experimentalCards;

    private static Dictionary<ModelId, ModelId> ExperimentalCards
    {
        get
        {
            if (_experimentalCards == null)
            {
                _experimentalCards = new Dictionary<ModelId, ModelId>();
                foreach (var kv in VanillaExperimentalCards)
                {
                    _experimentalCards.Add(kv.Key, kv.Value);
                }
                foreach (var kv in CustomExperimentalCardExtension.CustomExperimentalCards)
                {
                    _experimentalCards.Add(kv.Key, kv.Value);
                }
            }
            return _experimentalCards;
        }
    }
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;    

    private ModelId? _ancientCard;
    
    private IEnumerable<IHoverTip> _extraHoverTips = Array.Empty<IHoverTip>();

    [SavedProperty]
    private ModelId? AncientCard
    {
        get => _ancientCard;
        set
        {
            AssertMutable();
            _ancientCard = value;
            if (_ancientCard != null)
            {
                var savecard = SaveUtil.CardOrDeprecated(_ancientCard);
                
                _extraHoverTips = savecard.HoverTips.Concat([HoverTipFactory.FromCard(savecard, true)]);

                ((StringVar)DynamicVars["card"]).StringValue = savecard.Title;
            }
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => _extraHoverTips;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("card")];

    private static Dictionary<ModelId, ModelId> VanillaExperimentalCards = new()
    {
        {
            ModelDb.Character<Ironclad>().Id,
            ModelDb.Card<Cinderborn>().Id
        },
        {
            ModelDb.Character<Silent>().Id,
            ModelDb.Card<SleightOfHand>().Id
        },
        {
            ModelDb.Character<Regent>().Id,
            ModelDb.Card<NebulaHammer>().Id
        },
        {
            ModelDb.Character<Necrobinder>().Id,
            ModelDb.Card<NecroticBurst>().Id
        },
        {
            ModelDb.Character<Defect>().Id,
            ModelDb.Card<Electrolyze>().Id
        }
    };
    
    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return SetupForPlayer(owner);
    }
    
    public bool SetupForPlayer(Player player)
    {
        if (ExperimentalCards.TryGetValue(player.Character.Id, out ModelId card))
        {
            AncientCard = card;
            return true;
        }
        return false;
    }
    
    public override async Task AfterObtained()
    {
        CardModel card = Owner.RunState.CreateCard(SaveUtil.CardOrDeprecated(AncientCard), Owner);
        if (card == null) return;
        CardCmd.Upgrade(card);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
    }
}