
public class EarthBendingState : BendingState
{
    public EarthBendingState(PlayerBendingStateMachine fsm) : base(fsm)
    {
        foreach (var spell in fsm.EarthSpells)
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
