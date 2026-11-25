using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class TwistedCoin : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private bool _hasTriggered;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(222)];
    
    [SavedProperty]
    private bool HasTriggered
    {
        get => _hasTriggered;
        set
        {
            AssertMutable();
            _hasTriggered = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (HasTriggered || room is not { RoomType: RoomType.Shop })
            return;
        Flash();
        if (Status != RelicStatus.Active)
        {
            Status = RelicStatus.Active;
            return;
        }
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        HasTriggered = true;
    }
}