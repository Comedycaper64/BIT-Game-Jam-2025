public class BirdSpawnState : State
{
    public BirdSpawnState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ToggleInactive(false);
        stateMachine.SwitchState(new BirdChaseState(stateMachine));
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
