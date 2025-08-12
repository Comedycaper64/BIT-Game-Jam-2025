using UnityEngine;

public class BatChaseState : State
{
    private float batAttackRange = 3f;
    private float attackTimer = 0f;
    private float attackTime = 3f;

    public BatChaseState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.smAnimator.SetBool("chasing", true);
    }

    public override void Exit()
    {
        stateMachine.smAnimator.SetBool("chasing", false);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.enemyMovement.ApproachPlayer();

        float distanceToPlayer = Vector2.Distance(
            stateMachine.GetPlayerTransform().position,
            stateMachine.transform.position
        );

        if (distanceToPlayer <= batAttackRange)
        {
            attackTimer += deltaTime;

            if (attackTimer > attackTime)
            {
                stateMachine.SwitchState(new BatAttackState(stateMachine));
            }
        }
    }
}
