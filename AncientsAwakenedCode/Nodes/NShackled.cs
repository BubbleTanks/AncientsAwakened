using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;

public partial class NShackled : Control
{
    public override void _Ready()
    {
        var bound = PreloadManager.Cache.GetScene("res://scenes/vfx/ui/card/afflictions/bound/vfx_ui_card_affliction_bound.tscn").Instantiate();
        foreach (Node child in bound.GetChildren())
        {
            if (child is CanvasItem canvasItem)
            {
                canvasItem.Modulate = new Color(0, 0.95f, 2.5f);
            }
        }
        AddChild(bound);
    }
}
