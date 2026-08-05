using BaseLib.Common.Rewards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Powers.LunaticCultist;

public class GuidancePower : AncientsAwakenedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCombatEnd(CombatRoom room)
    {
        for (int index = 0; index < Amount; ++index)
        {
            room.AddExtraReward(Owner.Player, new CardUpgradeReward(Owner.Player)
            {
                Amount = 1,
            });
        }
        return Task.CompletedTask;
    }
}