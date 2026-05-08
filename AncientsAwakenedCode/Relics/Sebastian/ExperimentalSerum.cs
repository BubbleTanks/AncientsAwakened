using AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class ExperimentalSerum : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private ModelId _ancientCard;
    
    private IEnumerable<IHoverTip> _extraHoverTips = Array.Empty<IHoverTip>();

    [SavedProperty]
    private ModelId AncientCard
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

    private static Dictionary<ModelId, ModelId> ExperimentalCards;
    
    public override bool IsAllowed(IRunState runState)
    {
        return SetupForPlayer(LocalContext.GetMe(runState));
    }
    
    public bool SetupForPlayer(Player player)
    {
        if (ExperimentalCards == null)
        {
            ExperimentalCards = new Dictionary<ModelId, ModelId>()
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
        }

        if (ExperimentalCards.TryGetValue(player.Character.Id, out ModelId card))
        {
            AncientCard = card;
            return true;
        }

        return false;
    }
    
    public override async Task AfterObtained()
    {
        //AncientCard = ExperimentalCards[Owner.Character.Id];
        CardModel card = Owner.RunState.CreateCard(SaveUtil.CardOrDeprecated(AncientCard), Owner);
        if (card == null) return;
        CardCmd.Upgrade(card);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
    }
}