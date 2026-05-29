using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class CelestialSigil() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RitualPower>(1)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Power || cardPlay.Card.Owner != this.Owner)
            return;
        await PowerCmd.Apply<RitualPower>(choiceContext, Owner.Creature, DynamicVars.Power<RitualPower>().BaseValue, Owner.Creature, null);
    }
}