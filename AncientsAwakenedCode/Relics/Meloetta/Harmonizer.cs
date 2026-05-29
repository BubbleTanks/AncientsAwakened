using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;


[Pool(typeof(EventRelicPool))]
public class Harmonizer() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public bool PowerPlayed;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Power || cardPlay.Card.Owner != this.Owner || PowerPlayed)
            return;
        PowerPlayed = true;
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
    
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        PowerPlayed = false;
        return Task.CompletedTask;
    }
}