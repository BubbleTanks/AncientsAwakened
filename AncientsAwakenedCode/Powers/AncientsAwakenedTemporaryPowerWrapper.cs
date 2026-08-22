using BaseLib.Abstracts;
using BaseLib.Extensions;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Powers;

public abstract class AncientsAwakenedTemporaryPowerWrapper<TModel, TPower> : CustomTemporaryPowerModelWrapper<TModel, TPower>
    where TModel : AbstractModel
    where TPower : PowerModel
{
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}