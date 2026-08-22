using AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions.LunaticCultist;

public class CollectOption(Player owner) : AncientsAwakenedRestSiteOption(owner)
{
    public override async Task<bool> OnSelect()
    {
        var card = Owner.RunState.CreateCard<Starshine>(Owner);
        var result = await CardPileCmd.Add(card, PileType.Deck);
        CardCmd.PreviewCardPileAdd(result);
        return true;
    }
}