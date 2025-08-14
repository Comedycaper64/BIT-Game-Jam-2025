using UnityEngine;

public class EnemyChaseState : State
{
    private float attackRange = 3f;
    private float attackTimer = 0f;
    private float attackTime = 3f;

    public EnemyChaseState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.smAnimator.SetBool("chasing", true);
        attackRange = stateMachine.GetAttackRange();
        attackTime = stateMachine.GetAttackInterval();
    }

    public override void Exit()
    {
        stateMachine.smAnimator.SetBool("chasing", false);
    }

    public override void Tick(float deltaTime)
    {
        Vector2 playerPosition = stateMachine.GetPlayerTransform().position;

        stateMachine.enemyMovement.ApproachPlayer(playerPosition);

        float distanceToPlayer = Vector2.Distance(playerPosition, stateMachine.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            attackTimer += deltaTime;

            if (attackTimer > attackTime)
            {
                stateMachine.SwitchState(new EnemyAttackState(stateMachine));
            }
        }
    }
}
