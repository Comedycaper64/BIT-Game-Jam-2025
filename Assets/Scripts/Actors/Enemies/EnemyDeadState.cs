public class EnemyDeadState : State
{
    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ToggleInactive(true);
        stateMachine.smAnimator.SetTrigger("death");
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
