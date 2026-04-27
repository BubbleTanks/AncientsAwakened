using BaseLib.Abstracts;
using BaseLib.Extensions;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using Godot;

namespace AncientsAwakened.AncientsAwakenedCode.Powers;

public abstract class AncientsAwakenedPower : CustomPowerModel
{
    //Loads from AncientsAwakened/images/powers/your_power.png
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