using AncientsAwakened.AncientsAwakenedCode.Cards.Meloetta;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;


[Pool(typeof(EventRelicPool))]
public class RelicSong() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<Crescendo>(Owner), PileType.Deck)], 2F);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<Diminuendo>(Owner), PileType.Deck)], 2F);
    }
    
    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        return player != Owner || Owner.PlayerCombatState.TurnNumber > 1 ? count : count + DynamicVars.Cards.BaseValue;
    }
}