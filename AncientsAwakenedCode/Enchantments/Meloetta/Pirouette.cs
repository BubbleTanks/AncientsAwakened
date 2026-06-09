using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Meloetta;

public class Pirouette : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2M, ValueProp.Move), new CardsVar(3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        IEnumerable<CardModel> cardOptions = PileType.Draw.GetPile(Card.Owner).Cards.ToList().StableShuffle(Card.Owner.RunState.Rng.CombatCardSelection).Take(DynamicVars.Cards.IntValue);
        CardModel card2 = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Card.Owner), Card.Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), c => cardOptions.Contains(c))).FirstOrDefault();
        if (card2 != null)
            await CardPileCmd.Add(card2, PileType.Hand);
    }
    
    public override Decimal EnchantBlockAdditive(Decimal originalBlock) => DynamicVars.Block.BaseValue;
    
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