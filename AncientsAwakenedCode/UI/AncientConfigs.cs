using BaseLib.Config;

namespace AncientsAwakened.AncientsAwakenedCode.UI;

[ConfigHoverTipsByDefault]
internal class AncientConfigs : SimpleModConfig
{
    [ConfigSection("Mithrix")]
    public static bool MultiplayerFlawlessHammer { get; set; } = false;
    public static bool MultiplayerMonsoonCharm { get; set; } = false;
}