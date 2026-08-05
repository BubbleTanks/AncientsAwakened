using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Utils;

public static class CompatabilityUtils
{
    public static AttackCommand FromOstyCompatability(this AttackCommand command, Creature osty, CardModel card, CardPlay? cardPlay)
    {
        return _fromOsty.Invoke<AttackCommand>(command, osty, card, cardPlay)!;
    }
    
    private static VariableMethod _fromOsty = new(
        (typeof(AttackCommand), "FromOsty",
            [typeof(Creature), typeof(CardModel), typeof(CardPlay)],
            [0, 1, 2]),
        (typeof(AttackCommand), "FromOsty",
            [typeof(Creature), typeof(CardModel)],
            [0, 1])
    );
    
    public static class CreatureCmd_
    {
        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target,
            DamageVar damageVar, CardModel cardSource, CardPlay cardPlay)
            => await Damage(choiceContext, target, damageVar.BaseValue, damageVar.Props, cardSource, cardPlay);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target,
            decimal amount, ValueProp props, CardModel cardSource, CardPlay cardPlay)
            => await Damage(choiceContext, [target], amount, props, cardSource.Owner.Creature, cardSource, cardPlay);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext,
            IEnumerable<Creature> targets, DamageVar damageVar, Creature dealer)
            => await Damage(choiceContext, targets, damageVar.BaseValue, damageVar.Props, dealer);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext,
            IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature dealer)
            => await Damage(choiceContext, targets, amount, props, dealer, null, null);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target,
            DamageVar damageVar, Creature dealer)
            => await Damage(choiceContext, target, damageVar.BaseValue, damageVar.Props, dealer);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target,
            decimal amount, ValueProp props, Creature dealer)
            => await Damage(choiceContext, [target], amount, props, dealer, null, null);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target,
            DamageVar damageVar, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
            => await Damage(choiceContext, [target], damageVar.BaseValue, damageVar.Props, dealer, cardSource,
                cardPlay);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext, Creature target,
            decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
            => await Damage(choiceContext, [target], amount, props, dealer, cardSource, cardPlay);

        /// <inheritdoc cref="Damage(PlayerChoiceContext, IEnumerable{Creature}, decimal, ValueProp, Creature, CardModel, CardPlay)"/>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext,
            IEnumerable<Creature> targets, DamageVar damageVar, Creature? dealer, CardModel? cardSource,
            CardPlay? cardPlay)
            => await Damage(choiceContext, targets, damageVar.BaseValue, damageVar.Props, dealer, cardSource, cardPlay);

        /// <summary>
        /// Compatibility method to use instead of CreatureCmd.Damage that works on both main and beta branch.
        /// </summary>
        public static async Task<IEnumerable<DamageResult>> Damage(PlayerChoiceContext choiceContext,
            IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
        {
            return await _damage.Invoke<Task<IEnumerable<DamageResult>>>(null, choiceContext, targets, amount, props, dealer, cardSource, cardPlay)!;
        }

        private static VariableMethod _damage = new(
            (typeof(CreatureCmd), "Damage",
                [
                    typeof(PlayerChoiceContext), typeof(IEnumerable<Creature>), typeof(decimal), typeof(ValueProp),
                    typeof(Creature), typeof(CardModel), typeof(CardPlay)
                ],
                [0, 1, 2, 3, 4, 5, 6]),
            (typeof(CreatureCmd), "Damage",
                [
                    typeof(PlayerChoiceContext), typeof(IEnumerable<Creature>), typeof(decimal), typeof(ValueProp),
                    typeof(Creature), typeof(CardModel)
                ],
                [0, 1, 2, 3, 4, 5])
        );
    }
}