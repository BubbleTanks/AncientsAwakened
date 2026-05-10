using AncientsAwakened.AncientsAwakenedCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(RegentCardPool))]
public class NebulaHammer() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ForgeVar(5)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
    }
    
    protected override PileType GetResultPileTypeForCardPlay()
    {
        PileType resultPileType = base.GetResultPileTypeForCardPlay();
        return resultPileType != PileType.Discard ? resultPileType : PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(2M);
    }
}