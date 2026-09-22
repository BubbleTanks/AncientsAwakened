using AncientsAwakened.AncientsAwakenedCode.Potions.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class MeltingCandle : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPotion<HotWax>()];
    
    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, DynamicVars.Cards.IntValue);
        foreach (CardModel original in (await CardSelectCmd.FromDeckForTransformation(Owner, prefs)).ToList())
        {
            CardModel cardForTransform = CardFactory.CreateRandomCardForTransform(original, false, Owner.RunState.Rng.Niche);
            CardCmd.Upgrade(cardForTransform);
            await CardCmd.Transform(original, cardForTransform);
        }
        
        await PotionCmd.TryToProcure<HotWax>(Owner);
        await PotionCmd.TryToProcure<HotWax>(Owner);
    }
}