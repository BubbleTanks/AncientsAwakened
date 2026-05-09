using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions;

public class GlowingVialOption : RestSiteOption, ICustomModel
{
    
    public GlowingVialOption(Player owner)
        : base(owner)
    {
        IsEnabled = GetRemovableCardCount(owner) >= 1;
    }
    
    private static int GetRemovableCardCount(Player player)
    {
        return PileType.Deck.GetPile(player).Cards.Count(c => c.IsRemovable);
    }
    
    public override string OptionId => "AAVIAL";
    
    public override LocString Description
    {
        get
        {
            if (!IsEnabled)
                return new LocString("rest_site_ui", $"OPTION_{OptionId}.descriptionDisabled");
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
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.Creature, new DamageVar(8M, ValueProp.Unblockable | ValueProp.Unpowered), null, null);
        }

        if (Owner.Creature.CurrentHp <= 0)
        {
            return true;
        }
        
        return false;
    }
}