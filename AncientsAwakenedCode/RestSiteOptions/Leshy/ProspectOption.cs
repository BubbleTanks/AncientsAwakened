using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions.Leshy;

public class ProspectOption : RestSiteOption, ICustomModel
{
    
    public ProspectOption(Player owner)
        : base(owner)
    {
        IsEnabled = true;
    }

    public override string OptionId => "ANCIENTSAWAKENED-PROSPECT";
    
    public override LocString Description
    {
        get
        {
            LocString description = new LocString("rest_site_ui", $"OPTION_{OptionId}.description");
            return description;
        }
    }
    
    public override async Task<bool> OnSelect()
    {
        await PlayerCmd.GainGold(Owner.RunState.Rng.Niche.NextInt(250,300), Owner);
        return true;
    }
}