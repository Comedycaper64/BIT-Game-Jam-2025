public class EnemySpawnState : State
{
    public EnemySpawnState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ToggleInactive(false);
        stateMachine.SwitchState(new EnemyChaseState(stateMachine));
        stateMachine.smAnimator.SetTrigger("spawn");
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
