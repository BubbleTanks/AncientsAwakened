using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class DyingFicus : AncientsAwakenedRelic
{
    private const string RemainingCombatsKey = "RemainingCombats";
    private const int CombatsToActivate = 3;
    private int _combatsFinished;
    private bool _gaveCard;

    public override RelicRarity Rarity => RelicRarity.Event;

    public override bool IsUsedUp => GaveCard;

    public override bool ShowCounter => DisplayAmount > 0;

    public override int DisplayAmount => CombatsToActivate - CombatsFinished;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new (RemainingCombatsKey, CombatsToActivate)];

    [SavedProperty]
    private int CombatsFinished
    {
        get => _combatsFinished;
        set
        {
            AssertMutable();
            _combatsFinished = value;
            InvokeDisplayAmountChanged();
        }
    }

    [SavedProperty]
    private bool GaveCard
    {
        get => _gaveCard;
        set
        {
            AssertMutable();
            _gaveCard = value;
            InvokeDisplayAmountChanged();
            if (!_gaveCard)
                return;
            Status = RelicStatus.Disabled;
        }
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        if (GaveCard)
            return Task.CompletedTask;
        ++CombatsFinished;
        DynamicVars[RemainingCombatsKey].BaseValue = decimal.Max(CombatsToActivate - CombatsFinished, 0M);
        return Task.CompletedTask;
    }

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner || room is not CombatRoom || GaveCard || CombatsToActivate - CombatsFinished > 0)
            return false;
        rewards.Add(new CardReward(CardCreationOptions.ForNonCombatWithDefaultOdds([ModelDb.CardPool<ColorlessCardPool>()], _ => true).WithFlags(CardCreationFlags.NoRarityModification), 3, Owner));
        return true;
    }

    public override Task AfterModifyingRewards()
    {
        Flash();
        GaveCard = true;
        return Task.CompletedTask;
    }
}