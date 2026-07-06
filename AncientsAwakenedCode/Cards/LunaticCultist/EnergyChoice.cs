using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;

[Pool(typeof(EventCardPool))]
public class EnergyChoice() : AncientsAwakenedCard(-1, CardType.Skill, CardRarity.Ancient, TargetType.None), Starshine.ICardChoice
{
    public const int EnergyValue = 1;
    public const int DrawValue = 2;
    public const int DrawUpgrade = 1;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(EnergyValue), new CardsVar(DrawValue)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OnChosen(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(DrawUpgrade);

    public async Task OnChosen(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}