using BaseLib.Config;

namespace AncientsAwakened.AncientsAwakenedCode.UI;

[ConfigHoverTipsByDefault]
internal class AncientConfigs : SimpleModConfig
{
    [ConfigSection("AncientEnabler")]   
    public static bool EnableMithrixAncient { get; set; } = true;
    public static bool EnableSebastianAncient { get; set; } = true;
    
    [ConfigSection("AncientForcer")]   
    public static bool EnableMithrixForcer { get; set; } = true;
    public static bool EnableSebastianForcer { get; set; } = true;
    
    [ConfigSection("Mithrix")]
    public static bool MultiplayerFlawlessHammer { get; set; } = false;
    public static bool MultiplayerMonsoonCharm { get; set; } = false;
}