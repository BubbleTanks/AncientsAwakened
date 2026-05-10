using AncientsAwakened.AncientsAwakenedCode.RestSiteOptions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;


[Pool(typeof(EventRelicPool))]
public class GlowingVial : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    private ICollection<RestSiteOption> _options;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new HoverTip(new LocString("rest_site_ui", "OPTION_AAVIAL.name"), new LocString("rest_site_ui", "OPTION_AAVIAL.description"))];

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
            return false;
        _options = options;
        options.Add(new GlowingVialOption(player));
        return true;
    }
    
    public override bool ShouldDisableRemainingRestSiteOptions(Player player)
    {
        if (Owner.Creature.CurrentHp >= GlowingVialOption.HP_LOSS)
        {
            if (_options == null)
            {
                return true;
            }
            foreach (var option in _options)
            {
                if (option is GlowingVialOption)
                {
                    option.IsEnabled = true;
                    if (NRestSiteRoom.Instance != null)
                    {
                        var button = NRestSiteRoom.Instance.GetButtonForOption(option);
                        if (button != null)
                        {
                            button._isUnclickable = false;
                            button.Reload();
                        }
                    }
                }
            }
        }
        return true;
    }
    
}