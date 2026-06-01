using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Meloetta;

public class Prized : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;
    
    public override bool CanEnchantCardType(CardType cardType) => cardType is CardType.Attack or CardType.Skill;
    
    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        PileType pileType,
        CardPilePosition position)
    {
        if (card != Card || pileType != PileType.Discard)
            return (pileType, position);
        return (PileType.Draw, CardPilePosition.Random);
    }
}