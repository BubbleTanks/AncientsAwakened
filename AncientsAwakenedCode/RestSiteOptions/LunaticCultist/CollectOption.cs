using AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions.LunaticCultist;

public class CollectOption : RestSiteOption, ICustomModel
{
    
    public CollectOption(Player owner)
        : base(owner)
    {
    }

    public override string OptionId => "ANCIENTSAWAKENED-COLLECT";
    
    public override LocString Description
    {
        get
        {
            LocString description = new LocString("rest_site_ui", $"OPTION_{OptionId}.description");
            return description;
        }
    }
    
    public override async Task<bool> OnSelect()
    {
        var card = Owner.RunState.CreateCard<Starshine>(Owner);
        var result = await CardPileCmd.Add(card, PileType.Deck);
        CardCmd.PreviewCardPileAdd(result);
        return true;
    }
}