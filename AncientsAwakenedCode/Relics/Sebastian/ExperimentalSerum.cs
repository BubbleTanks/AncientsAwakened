using AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class ExperimentalSerum : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    [SavedProperty]
    private CardModel AncientCard;

    public IEnumerable<IHoverTip> _extraHoverTips = Array.Empty<IHoverTip>();

    private static Dictionary<ModelId, CardModel> ExperimentalCards => new Dictionary<ModelId, CardModel>
    {
        {
            ModelDb.Character<Ironclad>().Id,
            ModelDb.Card<Cinderborn>()
        },
        {
            ModelDb.Character<Silent>().Id,
            ModelDb.Card<SleightOfHand>()
        },
        {
            ModelDb.Character<Regent>().Id,
            ModelDb.Card<NebulaHammer>()
        },
        {
            ModelDb.Character<Necrobinder>().Id,
            ModelDb.Card<NecroticBurst>()
        },
        {
            ModelDb.Character<Defect>().Id,
            ModelDb.Card<Electrolyze>()
        }
    };
    
    public bool SetupForPlayer(Player player)
    {
        AncientCard = ExperimentalCards[player.Character.Id];
        // _extraHoverTips = AncientCard.HoverTips.Concat((IEnumerable<IHoverTip>)HoverTipFactory.FromCard(AncientCard, true));

        if (AncientCard == null)
        {
            return false;
        }

        return true;
    }
    
    public override async Task AfterObtained()
    {
        AncientCard = ExperimentalCards[Owner.Character.Id];
        CardModel card = Owner.RunState.CreateCard(AncientCard, Owner);
        if (card == null) return;
        CardCmd.Upgrade(card);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
    }
}