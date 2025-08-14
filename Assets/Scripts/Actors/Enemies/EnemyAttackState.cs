public class EnemyAttackState : State
{
    private float attackTimer = 0f;
    private float attackTime = 1.5f;

    public EnemyAttackState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.enemyMovement.PerformAttack(stateMachine.GetPlayerTransform().position);
        stateMachine.smAnimator.SetTrigger("attack");
        attackTime = stateMachine.GetAttackTime();
    }

    public override void Exit()
    {
        stateMachine.enemyMovement.InterruptAttack();
    }

    public override void Tick(float deltaTime)
    {
        attackTimer += deltaTime;

        if (attackTimer >= attackTime)
        {
            stateMachine.SwitchState(new EnemyChaseState(stateMachine));
        }
    }
}
