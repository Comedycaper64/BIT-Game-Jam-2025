using UnityEngine;

public class BirdChaseState : State
{
    private float birdAttackRange = 5f;
    private float attackTimer = 0f;
    private float attackTime = 3f;

    public BirdChaseState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.smAnimator.SetBool("chasing", true);
        //Attack time = statemachine.get attack time
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

        if (distanceToPlayer <= birdAttackRange)
        {
            attackTimer += deltaTime;

            if (attackTimer > attackTime)
            {
                stateMachine.SwitchState(new BirdAttackState(stateMachine));
            }
        }
    }
}
