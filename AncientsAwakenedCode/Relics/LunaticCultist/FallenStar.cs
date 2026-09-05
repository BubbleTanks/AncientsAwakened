using AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;
using AncientsAwakened.AncientsAwakenedCode.RestSiteOptions.LunaticCultist;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public sealed class FallenStar : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new HoverTip(new LocString("rest_site_ui", "ANCIENTSAWAKENED-COLLECT.name"), new LocString("rest_site_ui", "ANCIENTSAWAKENED-COLLECT.description")), HoverTipFactory.FromCard<Starshine>()];

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
            return false;
        options.Add(new Collect(player));
        return true;
    }
}