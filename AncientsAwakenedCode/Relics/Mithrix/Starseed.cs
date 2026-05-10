using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;

  
[Pool(typeof(EventRelicPool))]
public class Starseed : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<Egocentrism>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
    
    public override async Task AfterObtained()
    {
        List<Reward> list = new List<Reward>();
        CardCreationOptions options = CardCreationOptions.ForNonCombatWithUniformOdds([Owner.Character.CardPool], c => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification);
            
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            list.Add(new CardReward(options, 3, Owner));
        }

        await RewardsCmd.OfferCustom(Owner, list);
        await CardPileCmd.AddCurseToDeck<Egocentrism>(Owner);
    }
}