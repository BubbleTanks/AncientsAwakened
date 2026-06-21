using AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public class FlawlessHammer : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    private List<IHoverTip> _extraHoverTips = [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            if (_extraHoverTips.Count == 0)
            {
                UpdateHoverTips(Owner);
            }
            return _extraHoverTips;
        }
    }

    public void SetupForPlayer(Player player)
    {
        UpdateHoverTips(player);
    }
    
    private void UpdateHoverTips(Player player)
    {
        _extraHoverTips.Clear();
        if (player.RunState.Players.Count > 1 && (AncientConfigs.MultiplayerFlawlessHammer || MultiplayerHammerInDeck(player)))
        {
            BreakBeneathMe card = (BreakBeneathMe) ModelDb.Card<BreakBeneathMe>().ToMutable();
            card.IsMultiplayer = true;
            _extraHoverTips.AddRange(HoverTipFactory.FromCard(card));
            _extraHoverTips.AddRange(card.HoverTips);
        }
        else
        {
            _extraHoverTips.AddRange(HoverTipFactory.FromCardWithCardHoverTips<BreakBeneathMe>());
        }
    }

    private bool MultiplayerHammerInDeck(Player player)
    {
        if (player.Deck.Cards.FirstOrDefault(c => c.Id == ModelDb.Card<BreakBeneathMe>().Id) is { } card)
        {
            return ((BreakBeneathMe) card).IsMultiplayer;
        }
        return false;
    }
    
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        var card = Owner.RunState.CreateCard<BreakBeneathMe>(Owner);
        if(AncientConfigs.MultiplayerFlawlessHammer && Owner.RunState.Players.Count > 1)
            card.IsMultiplayer = true;
        UpdateHoverTips(Owner);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card, PileType.Deck)], 2F);
    }
}