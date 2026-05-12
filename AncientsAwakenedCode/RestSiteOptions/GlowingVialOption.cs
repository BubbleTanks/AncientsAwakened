using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions;

public class GlowingVialOption : RestSiteOption, ICustomModel
{
    public const decimal HP_LOSS = 8M;

    public static decimal HpCost(Player player)
    {
        return Hook.ModifyHpLostAfterOsty(player.RunState, player.Creature.CombatState, player.Creature, HP_LOSS, ValueProp.Unpowered | ValueProp.Unblockable, player.Creature, null, out _);
    }
    
    public GlowingVialOption(Player owner)
        : base(owner)
    {
        IsEnabled = GetRemovableCardCount(owner) >= 1 && (owner.Creature.CurrentHp > HpCost(owner) || !Hook.ShouldDie(owner.RunState, owner.Creature.CombatState, owner.Creature, out _));
    }
    
    private static int GetRemovableCardCount(Player player)
    {
        return PileType.Deck.GetPile(player).Cards.Count(c => c.IsRemovable);
    }
    
    public override string OptionId => "ANCIENTSAWAKENED-VIAL";
    
    public override LocString Description
    {
        get
        {
            if (!IsEnabled)
            {
                if(Owner.Creature.CurrentHp <= HP_LOSS) return new LocString("rest_site_ui", $"OPTION_{OptionId}.descriptionHpDisabled");
                return new LocString("rest_site_ui", $"OPTION_{OptionId}.descriptionDisabled");
            }
            LocString description = new LocString("rest_site_ui", $"OPTION_{OptionId}.description");
            return description;
        }
    }
    
    public override async Task<bool> OnSelect()
    {
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1)
        {
            Cancelable = true, RequireManualConfirmation = true
        };
        CardModel original = (await CardSelectCmd.FromDeckForTransformation(Owner, prefs)).FirstOrDefault();
        if (original != null)
        {
            await CardCmd.TransformToRandom(original, Owner.RunState.Rng.Niche, CardPreviewStyle.EventLayout);
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.Creature, new DamageVar(HP_LOSS, ValueProp.Unblockable | ValueProp.Unpowered), null, null);
        }

        if (Owner.Creature.CurrentHp <= HpCost(Owner) && Hook.ShouldDie(Owner.RunState, Owner.Creature.CombatState, Owner.Creature, out _))
        {
            IsEnabled = false;
            var button = NRestSiteRoom.Instance.GetButtonForOption(this);
            if (button != null)
            {
                button.Reload();
                button._isUnclickable = !IsEnabled;
            }
        }
        
        return false;
    }
}