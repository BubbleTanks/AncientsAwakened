using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class AncientScepter : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool IsAllowed(IRunState runState)
    {
        if(IsMutable) return SetupForPlayer(LocalContext.GetMe(runState));
        return true;
    }

    public override bool HasUponPickupEffect => true;
    
    public bool SetupForPlayer(Player player)
    {
        if (player == null)
        {
            return false;
        }
        
        AssertMutable();
        CardModel transcendenceStarterCard = GetTranscendenceStarterCard(player);
        if (transcendenceStarterCard != null)
        {
            return true;
        }
        return false;
    }

    public override async Task AfterObtained()
    {
        IEnumerable<CardTransformation> transformations = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsBasicStrikeOrDefend && c.IsRemovable).ToList()
            .Select(c => new CardTransformation(c, GetTranscendenceTransformedCard(c)));
        List<CardPileAddResult> list = (await CardCmd.Transform(transformations, null, CardPreviewStyle.None)).ToList();
        if (list.Count > 0 && LocalContext.IsMe(Owner))
        {
            NSimpleCardsViewScreen.ShowScreen(list, new LocString("relics", "ANCIENTSAWAKENED-ANCIENT_SCEPTER.infoText"));
        }
    }
    
    private CardModel? GetTranscendenceStarterCard(Player player)
    {
        return player.Deck.Cards.FirstOrDefault((CardModel c) => TranscendenceUpgrades.ContainsKey(c.Id));
    }

    private CardModel GetTranscendenceTransformedCard(CardModel starterCard)
    {
        if (TranscendenceUpgrades.TryGetValue(starterCard.Id, out CardModel value))
        {
            CardModel cardModel = starterCard.Owner.RunState.CreateCard(value, starterCard.Owner);
            if (starterCard.IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }
            if (starterCard.Enchantment != null)
            {
                EnchantmentModel enchantmentModel = (EnchantmentModel)starterCard.Enchantment.MutableClone();
                CardCmd.Enchant(enchantmentModel, cardModel, enchantmentModel.Amount);
            }
            return cardModel;
        }
        return base.Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
    }
    
    private static Dictionary<ModelId, CardModel> TranscendenceUpgrades => new Dictionary<ModelId, CardModel>
    {
        {
            ModelDb.Card<StrikeIronclad>().Id,
            ModelDb.Card<DemonicStrike>()
        },
        {
            ModelDb.Card<DefendIronclad>().Id,
            ModelDb.Card<DemonicDefend>()
        },
        {
            ModelDb.Card<StrikeSilent>().Id,
            ModelDb.Card<DeadlyStrike>()
        },
        {
            ModelDb.Card<DefendSilent>().Id,
            ModelDb.Card<DeadlyDefend>()
        },
        {
            ModelDb.Card<StrikeRegent>().Id,
            ModelDb.Card<CosmicStrike>()
        },
        {
            ModelDb.Card<DefendRegent>().Id,
            ModelDb.Card<CosmicDefend>()
        },
        {
            ModelDb.Card<StrikeNecrobinder>().Id,
            ModelDb.Card<EternalStrike>()
        },
        {
            ModelDb.Card<DefendNecrobinder>().Id,
            ModelDb.Card<EternalDefend>()
        },
        {
            ModelDb.Card<StrikeDefect>().Id,
            ModelDb.Card<EmpoweredStrike>()
        },
        {
            ModelDb.Card<DefendDefect>().Id,
            ModelDb.Card<EmpoweredDefend>()
        }
    };

    public static List<CardModel> TranscendenceCards => TranscendenceUpgrades.Values.ToList();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("StarterCard"), new StringVar("AncientCard")];
}