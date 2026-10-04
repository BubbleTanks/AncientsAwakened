using AncientsAwakened.AncientsAwakenedCode.Enchantments.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Audio;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;
[Pool(typeof(EventRelicPool))]
public sealed class SnowFeather : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    private static readonly ModSound PickupSound = new("res://AncientsAwakened/audio/weird-route-jingle.mp3");
    
    private bool _wasDisabled;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new("Stronger", 2M)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromEnchantment<Stronger>(2);
    
    [SavedProperty]
    public bool WasDisabled
    {
        get => _wasDisabled;
        set
        {
            AssertMutable();
            _wasDisabled = value;
            
            if (value)
            {
                Status = RelicStatus.Disabled;
            }
        }
    }
    
    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player != Owner || !options.Flags.HasFlag(CardCreationFlags.IsCardReward) || WasDisabled)
            return false;
        
        var stronger = ModelDb.Enchantment<Stronger>();
        
        foreach (var cardReward in cardRewards)
        {
            var card1 = cardReward.Card;
            if (!stronger.CanEnchant(card1)) 
                continue;
            var card2 = Owner.RunState.CloneCard(card1);
            CardCmd.Enchant<Stronger>(card2, DynamicVars["Stronger"].BaseValue);
            cardReward.ModifyCard(card2, this);
        }
        
        return true;
    }
    
    // this is the function that controls the weird route jingle sound incase you wanna make it a config option.
    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (WasDisabled)
            return Task.CompletedTask;
        
        if (card.Owner != Owner || !LocalContext.IsMe(Owner))
            return Task.CompletedTask;
        
        if (oldPileType != PileType.None || card.Pile?.Type != PileType.Deck)
            return Task.CompletedTask;
        
        if (card.Enchantment is not Stronger)
            return Task.CompletedTask;
        
        ModAudio.PlaySoundInRun(PickupSound);
        return Task.CompletedTask;
    }
}