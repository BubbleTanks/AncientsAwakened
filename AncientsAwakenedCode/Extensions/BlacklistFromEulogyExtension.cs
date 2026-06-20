using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

public static class BlacklistFromEulogyExtension
{
    internal static readonly List<ModelId> EulogyBlacklist = [];
    
    /// <summary>
    /// Call this extension in your relic constructor (with an Interop for an optional dependency) if you want to blacklist them from Eulogy Zero spawning.
    /// <example>
    /// <list type="bullet">
    /// <item>
    /// <description>Relics that alter the map in some way (Golden Compass)</description>
    /// </item>
    /// /// <item>
    /// <description>Relics that actively cause issues when spawned in the middle of the map (Shipping Request)</description>
    /// </item>
    /// /// <item>
    /// <description>Relics that are actively useless in act 3 (Also Shipping Request).</description>
    /// </item>
    /// </list>
    /// </example>
    /// </summary>
    public static void BlacklistFromEulogy(this RelicModel model)
    {
        if (EulogyBlacklist.Remove(model.Id))
        {
            AncientsAwakenedMain.Logger.Warn($"Relic {model.Id} attempted to be blacklisted multiple times");
        }
        EulogyBlacklist.Add(model.Id);
    }
}