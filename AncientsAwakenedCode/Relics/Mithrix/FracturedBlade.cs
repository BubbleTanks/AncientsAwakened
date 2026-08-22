using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public class FracturedBlade : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool isFirstTurn;

    public override bool ShouldTakeExtraTurn(Player player)
    {
        if (player != Owner)
            return false;

        if (isFirstTurn)
        {
            isFirstTurn = false;
            Flash();
            return true;
        }
        
        return false;
    }
    
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return Task.CompletedTask;
        isFirstTurn = true;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && isFirstTurn)
        {
            await CreatureCmd.LoseMaxHp(context, Owner.Creature, 1, false);
        }
    }
}