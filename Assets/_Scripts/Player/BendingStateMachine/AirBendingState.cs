
public class AirBendingState : BendingState
{
    public AirBendingState(PlayerBendingStateMachine fsm) : base(fsm)
    {
        foreach (var spell in fsm.AirSpells)
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
