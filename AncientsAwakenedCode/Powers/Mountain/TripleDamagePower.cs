using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Powers.Mountain;

public class TripleDamagePower : AncientsAwakenedPower
{
    public override PowerType Type => PowerType.Buff;

    public override int DisplayAmount => Amount * 300;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override Task BeforeAttack(AttackCommand command)
    {
        if (!(command.ModelSource is CardModel modelSource) || modelSource.Owner.Creature != Owner || modelSource.Type != CardType.Attack || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        Data internalData = GetInternalData<Data>();
        if (internalData.commandToModify != null)
            return Task.CompletedTask;
        internalData.commandToModify = command;
        return Task.CompletedTask;
    }

    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (cardSource == null || cardSource.Owner.Creature != Owner || !props.IsPoweredAttack())
            return 1M;
        Data internalData = GetInternalData<Data>();
        return internalData.commandToModify != null && cardSource != internalData.commandToModify.ModelSource ? 1M : (Decimal) Math.Pow(3.0, Amount);
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        Data internalData = GetInternalData<Data>();
        if (command != internalData.commandToModify)
            return;
        internalData.commandToModify = null;
        await PowerCmd.Remove(this);
    }

    private class Data
    {
        public AttackCommand? commandToModify;
    }
}