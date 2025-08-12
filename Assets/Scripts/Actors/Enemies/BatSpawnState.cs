public class BatSpawnState : State
{
    public BatSpawnState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ToggleInactive(false);
        stateMachine.SwitchState(new BatChaseState(stateMachine));
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
