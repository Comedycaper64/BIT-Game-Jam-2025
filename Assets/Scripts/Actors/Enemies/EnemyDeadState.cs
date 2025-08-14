using System;

public class EnemyDeadState : State
{
    public static Action OnEnemyDead;

    public EnemyDeadState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ToggleInactive(true);
        stateMachine.smAnimator.SetTrigger("death");
        OnEnemyDead?.Invoke();
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
