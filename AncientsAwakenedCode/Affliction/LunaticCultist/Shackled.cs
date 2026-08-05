using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Affliction.LunaticCultist;

public class Shackled : AfflictionModel, ICustomModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Unplayable)];
     
    public override bool CanAfflictUnplayableCards => false;
}