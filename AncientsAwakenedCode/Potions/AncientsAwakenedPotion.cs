using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;

namespace AncientsAwakened.AncientsAwakenedCode.Potions;

public abstract class AncientsAwakenedPotion : CustomPotionModel
{
    
    public override string? CustomPackedImagePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
            return ResourceLoader.Exists(path) ? path : null;
        }
    }
}