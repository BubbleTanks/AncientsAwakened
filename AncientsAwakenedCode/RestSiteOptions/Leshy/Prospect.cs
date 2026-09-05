using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions.Leshy;

public sealed class Prospect(Player owner) : AncientsAwakenedRestSiteOption(owner)
{
    public override async Task<bool> OnSelect()
    {
        await PlayerCmd.GainGold(Owner.RunState.Rng.Niche.NextInt(250,301), Owner);
        return true;
    }
}