public class BirdAttackState : State
{
    private float attackTimer = 0f;
    private float attackTime = 1f;

    public BirdAttackState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.enemyMovement.PerformAttack(stateMachine.GetPlayerTransform().position);
        stateMachine.smAnimator.SetTrigger("Attack");
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
            stateMachine.SwitchState(new BirdChaseState(stateMachine));
        }
    }
}
