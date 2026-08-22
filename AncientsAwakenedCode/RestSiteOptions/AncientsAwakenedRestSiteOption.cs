using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions;

public abstract class AncientsAwakenedRestSiteOption(Player owner) : RestSiteOption(owner), ICustomModel
{
    public override string OptionId => $"{AncientsAwakenedMain.ModId.ToUpperInvariant()}-{GetType().Name.ToSnakeCase().ToUpperInvariant()}";
    
    public override LocString Description => new("rest_site_ui", $"{OptionId}.description");
    
    // Both of these are referenced by RestSiteOptionPatch.cs because the base methods are not virtual.
    public virtual LocString CustomTitle => new("rest_site_ui", $"{OptionId}.name");
    public virtual string CustomIconPath
    {
        get
        {
            var path = $"{OptionId.RemovePrefix().ToLowerInvariant()}.png".RestSiteImagePath();
            return ResourceLoader.Exists(path) ? path : "placeholder.png".RestSiteImagePath();
        }
    }
}