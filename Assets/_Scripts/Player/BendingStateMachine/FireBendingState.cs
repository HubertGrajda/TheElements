
public class FireBendingState : BendingState
{
    public FireBendingState(PlayerBendingStateMachine fsm) : base(fsm)
    {
        foreach (var spell in fsm.FireSpells)
        {
            spells.Add(spell);
        }
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        return false;
    }
}
