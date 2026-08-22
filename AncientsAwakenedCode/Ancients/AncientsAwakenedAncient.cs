using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public abstract class AncientsAwakenedAncient : CustomAncientModel
{
    public override string? CustomScenePath    
    {
        get
        {
            AncientsAwakenedMain.Logger.Info(Id.Entry);
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.tscn".AncientScenePath();
            AncientsAwakenedMain.Logger.Info(path);
            return ResourceLoader.Exists(path) ? path : null;
        }
    }

    public override string CustomMapIconPath
    {
        get
        {
            AncientsAwakenedMain.Logger.Info(Id.Entry);
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".AncientMapIconImagePath();
            AncientsAwakenedMain.Logger.Info(path);
            return ResourceLoader.Exists(path) ? path : "placeholder.png".AncientMapIconImagePath();
        }
    }

    public override string CustomMapIconOutlinePath
    {
        get
        {
            AncientsAwakenedMain.Logger.Info(Id.Entry);
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".AncientMapIconImagePath();
            AncientsAwakenedMain.Logger.Info(path);
            return ResourceLoader.Exists(path) ? path : "placeholder_outline.png".AncientMapIconImagePath();
        }
    }

    public override string CustomRunHistoryIconPath
    {
        get
        {
            AncientsAwakenedMain.Logger.Info(Id.Entry);
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".AncientRunHistoryIconImagePath();
            AncientsAwakenedMain.Logger.Info(path);
            return ResourceLoader.Exists(path) ? path : "placeholder.png".AncientRunHistoryIconImagePath();
        }
    }

    public override string CustomRunHistoryIconOutlinePath
    {
        get
        {
            AncientsAwakenedMain.Logger.Info(Id.Entry);
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".AncientRunHistoryIconImagePath();
            AncientsAwakenedMain.Logger.Info(path);
            return ResourceLoader.Exists(path) ? path : "placeholder_outline.png".AncientRunHistoryIconImagePath();
        }
    }
}