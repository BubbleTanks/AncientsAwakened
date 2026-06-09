using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Meloetta;

public class Aria : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3M, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {    
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel card2 = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Card.Owner), Card.Owner, prefs)).FirstOrDefault();
        if (card2 != null) 
            await CardPileCmd.Add(card2, PileType.Draw, CardPilePosition.Top);
    }
    
    public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props) => !props.IsPoweredAttack() ? 0M : DynamicVars.Damage.BaseValue;
    
    private LocString SelectionScreenPrompt
    {
        get
        {
            LocString str = new LocString("enchantments", Id.Entry + ".selectionScreenPrompt");
            if (!str.Exists())
                throw new InvalidOperationException($"No selection screen prompt for {Id}.");
            DynamicVars.AddTo(str);
            return str;
        }
    }
}