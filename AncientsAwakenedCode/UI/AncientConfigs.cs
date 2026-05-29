using BaseLib.Config;

namespace AncientsAwakened.AncientsAwakenedCode.UI;

[ConfigHoverTipsByDefault]
internal class AncientConfigs : SimpleModConfig
{
    [ConfigSection("AncientEnabler")]   
    public static bool EnableSebastianAncient { get; set; } = true;
    public static bool EnableLeshyAncient { get; set; } = true;
    public static bool EnableMeloettaAncient { get; set; } = true;
    public static bool EnableMithrixAncient { get; set; } = true;
    public static bool EnableLunaticCultistAncient { get; set; } = true;
    public static bool EnableMountainAncient { get; set; } = true;
    
    [ConfigSection("AncientForcer")]   
    public static bool ForceSebastianEnabler { get; set; } = false;
    public static bool ForceLeshyEnabler { get; set; } = false;
    public static bool ForceMeloettaEnabler { get; set; } = false;
    public static bool ForceMithrixEnabler { get; set; } = false;
    public static bool ForceLunaticCultistEnabler { get; set; } = false;
    public static bool ForceMountainEnabler { get; set; } = false;
    
    
    [ConfigSection("Mithrix")]
    public static bool MultiplayerFlawlessHammer { get; set; } = false;
    public static bool MultiplayerMonsoonCharm { get; set; } = false;
}