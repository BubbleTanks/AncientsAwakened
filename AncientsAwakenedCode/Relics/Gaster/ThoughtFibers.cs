using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class ThoughtFibers : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override async Task AfterObtained()
    {
        var players = RunManager.Instance.State?.Players;
        Log.Info("Got players");
        if (players == null)
        {
            Log.Info("Players not found");
            return;
        }
        Log.Info("Players found");
        
        foreach (Player player1 in players)
        {
            Log.Info("Start loop");
            var options = CardCreationOptions.ForNonCombatWithUniformOdds([player1.Character.CardPool], c => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly);
            if (!options.GetPossibleCards(player1).Any())
            {
                Log.Info("Colorless fallback");
                options = CardCreationOptions.ForNonCombatWithUniformOdds([ModelDb.CardPool<ColorlessCardPool>()], c => c.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly);
            }
            Log.Info("Made options");

            if (!options.GetPossibleCards(player1).Any())
            {
                Log.Info("Options failed");
                continue;
            }
            
            var list = CardFactory.CreateForReward(player1, 1, options).FirstOrDefault()?.Card;
            Log.Info("Grabbed a card");
            
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(list, PileType.Deck));
            Log.Info("Card added");
        }
    }
}