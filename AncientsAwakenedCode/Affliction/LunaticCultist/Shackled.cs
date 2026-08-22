using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace AncientsAwakened.AncientsAwakenedCode.Affliction.LunaticCultist;

public class Shackled : AncientsAwakenedAffliction
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Unplayable)];
     
    public override bool CanAfflictUnplayableCards => false;
}