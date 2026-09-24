using AncientsAwakened.AncientsAwakenedCode.Extensions;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class DarkSpark : AncientsAwakenedRelic
{
    internal const int AdditionalFloors = 3;

    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Floors", AdditionalFloors), new BoolVar("FourPlusMP", false)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override async Task AfterObtained()
    {
        await RunManager.Instance.GenerateMap();
    }
    
    public DarkSpark()
    {
        this.BlacklistFromEulogy();
    }
    
    public void SetupForPlayer(Player player)
    {
        if(player.RunState.Players.Count >= 4 && !AncientConfigs.UncapDarkSpark)
            ((BoolVar)DynamicVars["FourPlusMP"]).BoolVal = true;
    }
}