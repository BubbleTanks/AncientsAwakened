using AncientsAwakened.AncientsAwakenedCode.Patches;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Potions.Gaster;

public sealed class HotWax : AncientsAwakenedPotion, NPotionPopupPatches.IDisablePotionDiscard
{
    public override PotionRarity Rarity => PotionRarity.Event;
    public override PotionUsage Usage => PotionUsage.AnyTime;
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20M, ValueProp.Unpowered)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        var damage = DynamicVars.Damage;
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(target)!);
        await CreatureCmd.Damage(choiceContext, target, damage.BaseValue, damage.Props, Owner.Creature, null, null);
    }
}