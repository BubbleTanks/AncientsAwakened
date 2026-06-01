using AncientsAwakened.AncientsAwakenedCode.Patches;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class BloodstainedSnow : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(10), new ("Relics", 2)];
    
    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, Owner.Deck.Cards.Count)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        List<CardTransformation> transformations = (await CardSelectCmd.FromDeckForTransformation(Owner, prefs, c => new CardTransformation(c, GetRandomCurses()))).Select(c => new CardTransformation(c, GetRandomCurses())).ToList();
        foreach (var transformation in transformations)
        {
            NoncurseToCurseTransformPatch.CursableField.Set(transformation.Original, true);
        }
        await CardCmd.Transform(transformations, Owner.PlayerRng.Transformations);
        await Cmd.Wait(0.75f);
        
        List<Reward> list = [];
        decimal maxHP = 0;
        foreach (var _ in Owner.Deck.Cards.Where(c => c.Rarity == CardRarity.Curse))
        {
            maxHP += DynamicVars.MaxHp.BaseValue;
            list.Add(new RelicReward(Owner));
            list.Add(new RelicReward(Owner));
        }

        await CreatureCmd.GainMaxHp(Owner.Creature, maxHP);
        await RewardsCmd.OfferCustom(Owner, list);
    }
    
    private IEnumerable<CardModel> GetRandomCurses()
    {
        return ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.CanBeGeneratedByModifiers).OrderBy((Func<CardModel, ModelId>) (c => c.Id));
    }
}