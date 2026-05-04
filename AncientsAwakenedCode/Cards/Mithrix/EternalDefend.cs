using AncientsAwakened.AncientsAwakenedCode.Cards;
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

namespace AncientsAwakened.AncientsAwakenedCode.Cards;

[Pool(typeof(NecrobinderCardPool))]
public class EternalDefend() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PerfectedPool>();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon), HoverTipFactory.FromCard<Soul>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(8M), new CardsVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        
        IEnumerable<Soul> cards = Soul.Create(Owner, DynamicVars.Cards.IntValue, CombatState);
        if (IsUpgraded)
        {
            foreach (CardModel card in cards)
                CardCmd.Upgrade(card);
        }
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Draw, true, CardPilePosition.Random));
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(3M);
    }
}