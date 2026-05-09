using BaseLib.Config;

namespace AncientsAwakened.AncientsAwakenedCode.UI;

internal class AncientConfigs : SimpleModConfig
{
    [ConfigSection("Ancient Enabler")]   
    public static bool EnableMithrixAncient { get; set; } = true;
    public static bool EnableSebastianAncient { get; set; } = true;
}