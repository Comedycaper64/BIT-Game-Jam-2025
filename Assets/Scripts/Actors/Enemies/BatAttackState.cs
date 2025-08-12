public class BatAttackState : State
{
    private float attackTimer = 0f;
    private float attackTime = 2f;

    public BatAttackState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.enemyMovement.PerformAttack();
        stateMachine.smAnimator.SetTrigger("Attack");
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        attackTimer += deltaTime;

        if (attackTimer >= attackTime)
        {
            stateMachine.SwitchState(new BatChaseState(stateMachine));
        }
    }
}
