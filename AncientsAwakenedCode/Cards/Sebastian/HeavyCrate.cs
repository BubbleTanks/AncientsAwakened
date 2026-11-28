using AncientsAwakened.AncientsAwakenedCode.Powers.Sebastian;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;

[Pool(typeof(QuestCardPool))]
public sealed class HeavyCrate() : AncientsAwakenedCard(2, CardType.Quest, CardRarity.Quest, TargetType.AnyEnemy)
{
    private enum HeavyCrateRewards
    {
        None,
        Gold,
        Potion, 
        Relic,
        CardReward,
        RareCardReward
    }
    
    private int[] _rewards;
    
    [SavedProperty]
    private int[] Rewards
    {
        get => _rewards;
        set
        {
            AssertMutable();
            _rewards = value;
            DynamicVars["RewardCount"].BaseValue = value.Length;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20M, ValueProp.Unpowered), new IntVar("RewardCount", 0)];
    public override int MaxUpgradeLevel => 0;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    public void InitializeRewards()
    {
        if(_rewards != null)
            return;
        var list = new List<HeavyCrateRewards>();
        list.AddRange([
            HeavyCrateRewards.Gold, 
            HeavyCrateRewards.Gold, 
            HeavyCrateRewards.Gold, 
            HeavyCrateRewards.Gold, 
            HeavyCrateRewards.Potion, 
            HeavyCrateRewards.Potion, 
            HeavyCrateRewards.CardReward, 
            HeavyCrateRewards.CardReward, 
            HeavyCrateRewards.CardReward, 
            HeavyCrateRewards.Relic, 
            HeavyCrateRewards.Relic, 
            HeavyCrateRewards.Relic, 
            HeavyCrateRewards.RareCardReward
        ]);
        Rewards = list.Select(r => (int)r).ToArray();
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // potentially change this to CreatureCmd.Damage if it causes issues
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithValueProp(ValueProp.Unpowered).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").WithHitVfxSpawnedAtBase().Execute(choiceContext);
        if (Rewards.Length > 0 && DeckVersion != null)
        {
            var list = _rewards.ToList();
            var reward = (HeavyCrateRewards) list[Owner.RunState.Rng.Niche.NextInt(list.Count)];
            if(CombatState?.RunState.CurrentRoom is not CombatRoom combatRoom)
                return;
            switch (reward)
            {
                case HeavyCrateRewards.None:
                    break;
                case HeavyCrateRewards.Gold:
                    combatRoom.AddExtraReward(Owner, new GoldReward(50, 60, Owner));
                    await PowerCmd.Apply<CrateGoldRewardPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;
                case HeavyCrateRewards.Potion:
                    combatRoom.AddExtraReward(Owner, new PotionReward(Owner));
                    await PowerCmd.Apply<CratePotionRewardPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;
                case HeavyCrateRewards.Relic:
                    combatRoom.AddExtraReward(Owner, new RelicReward(Owner));
                    await PowerCmd.Apply<CrateRelicRewardPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;
                case HeavyCrateRewards.CardReward:
                    combatRoom.AddExtraReward(Owner, new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, Owner));
                    await PowerCmd.Apply<CrateCardRewardPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;
                case HeavyCrateRewards.RareCardReward:
                    combatRoom.AddExtraReward(Owner, new CardReward(CardCreationOptions.ForNonCombatWithUniformOdds([Owner.Character.CardPool], c => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification), 3, Owner));
                    await PowerCmd.Apply<CrateRareCardRewardPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            list.Remove((int)reward);

            if (list.Count == 0)
            {
                List<CardModel> cardToRemove = []; 
                foreach (var card in Owner.Deck.Cards)
                {
                    if (card is not WeighedDown) 
                        continue;
                    cardToRemove.Add(card);
                    break;
                }
                await CardPileCmd.RemoveFromDeck(cardToRemove);
            }
            
            Rewards = list.ToArray();
            ((HeavyCrate)DeckVersion).Rewards = list.ToArray();
        }
    }
    
    protected override void AddExtraArgsToDescription(LocString description)
    {
        description.Add("HasDeckVersion", DeckVersion != null);
    }
}