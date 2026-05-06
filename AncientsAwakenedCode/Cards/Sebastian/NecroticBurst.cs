using AncientsAwakened.AncientsAwakenedCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(NecrobinderCardPool))]
public class NecroticBurst() : AncientsAwakenedCard(0,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon)];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(4M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.ToList();

        List<CardModel> list2 = list.Where(delegate(CardModel c)
        {
            return !(c is NecroticBurst);
        }).ToList();
        
        foreach (CardModel card in list2)
        {
            CardModel ego = CombatState.CreateCard<NecroticBurst>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(ego);
            }
            await CardCmd.Transform(card, ego);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(2M);
    }
}