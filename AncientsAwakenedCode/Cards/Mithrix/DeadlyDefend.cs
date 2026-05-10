using AncientsAwakened.AncientsAwakenedCode.Pools;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;

[Pool(typeof(SilentCardPool))]
public class DeadlyDefend() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PerfectedPool>();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Shiv>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5, ValueProp.Move), new DynamicVar("Shivs",2), new CardsVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        IEnumerable<CardModel> inHand = await Shiv.CreateInHand(Owner, DynamicVars["Shivs"].IntValue, CombatState);
        foreach (CardModel card in inHand)
            CardCmd.Upgrade(card);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}