using AncientsAwakened.AncientsAwakenedCode.Affliction.LunaticCultist;
using AncientsAwakened.AncientsAwakenedCode.Enchantments.LunaticCultist;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class StardustShackles : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..HoverTipFactory.FromEnchantment<Celestial>(), ..HoverTipFactory.FromAffliction<Shackled>()];

    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        var canonicalEnchantment = ModelDb.Enchantment<Celestial>();
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(Owner,  canonicalEnchantment, 1, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, 1);
            CardCmd.Preview(card);
        }
    }
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner != Owner || Owner.Creature.CombatState.CurrentSide != Owner.Creature.Side || !ModelDb.Affliction<Shackled>().CanAfflict(card) || 
            CombatManager.Instance.History.Entries.OfType<CardAfflictedEntry>().Count(e => e.HappenedThisTurn(Owner.Creature.CombatState) && e.Actor == Owner.Creature && e.Affliction is Shackled) >= 1)
            return;
        var affliction = await CardCmd.AfflictAndPreview<Shackled>([card], 1, CardPreviewStyle.None);
        if (affliction == null || card.Keywords.Contains(CardKeyword.Unplayable))
            return;
        CardCmd.ApplyKeyword(card, CardKeyword.Unplayable);
    }
}