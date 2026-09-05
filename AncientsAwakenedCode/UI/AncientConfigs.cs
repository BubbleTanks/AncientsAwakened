using BaseLib.Config;
using Godot;
using MegaCrit.Sts2.Core.Localization;

namespace AncientsAwakened.AncientsAwakenedCode.UI;

[ConfigHoverTipsByDefault]
internal class AncientConfigs : SimpleModConfig
{
    [ConfigSection("Mithrix")]
    public static bool MultiplayerFlawlessHammer { get; set; } = true;
    public static bool MultiplayerMonsoonCharm { get; set; } = false;
    
    [ConfigSection("Sebastian")]
    public static bool MultiplayerOpScanner { get; set; } = false;
    
    public override void SetupConfigUI(Control optionContainer)
    {
        var collapsibleSection = CreateCollapsibleSection(
            new LocString("settings_ui", "ANCIENTSAWAKENED-INFO.title").GetFormattedText());
        optionContainer.AddChild(collapsibleSection);
        collapsibleSection.ContentContainer.AddChild(CreateDescription(new LocString("settings_ui", "ANCIENTSAWAKENED-INFO.description").GetFormattedText()));
        optionContainer.AddChild(CreateDivider());
        base.SetupConfigUI(optionContainer);
    }
    
    private static Control CreateDivider()
    {
        var separator = new HSeparator
        {
            CustomMinimumSize = new Vector2(0, 28),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };

        var margin = new MarginContainer
        {
            CustomMinimumSize = new Vector2(0, 28),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        margin.AddThemeConstantOverride("margin_left", 12);
        margin.AddThemeConstantOverride("margin_right", 12);
        margin.AddThemeConstantOverride("margin_top", 12);
        margin.AddThemeConstantOverride("margin_bottom", 12);
        margin.AddChild(separator);
        return margin;
    }
    
    private static Control CreateDescription(string text)
    {
        var lineCount = text.Count(c => c == '\n') + 1;
        float minimumHeight = Math.Max(72, 38 * lineCount + 24);

        var margin = new MarginContainer
        {
            CustomMinimumSize = new Vector2(0, minimumHeight),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        margin.AddThemeConstantOverride("margin_left", 12);
        margin.AddThemeConstantOverride("margin_right", 12);
        margin.AddThemeConstantOverride("margin_bottom", 8);

        var description = CreateRawLabelControl(text, 24);
        description.CustomMinimumSize = new Vector2(
            description.CustomMinimumSize.X,
            Math.Max(description.CustomMinimumSize.Y, minimumHeight - 8));
        margin.AddChild(description);
        return margin;
    }
}