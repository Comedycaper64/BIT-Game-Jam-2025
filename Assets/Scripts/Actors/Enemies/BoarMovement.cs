using System.Collections;
using UnityEngine;

public class BoarMovement : EnemyMovement
{
    private bool approach = false;
    private float approachDistance = 0f;
    private float approachRefreshCD = 0.1f;
    private float approachRefreshTimer = 0f;
    private float attackWindupTime = 0.5f;
    private float obstacleRaycastDistance = 3f;
    private float alternateRaycastOffsetDegrees = 45f;
    private Vector2 moveDirection = Vector2.zero;
    private Coroutine attackCoroutine;

    private void Start()
    {
        approachRefreshTimer = Random.Range(0f, 0.09f);
    }

    private void FixedUpdate()
    {
        if (approach)
        {
            ApplyMovementForces(moveDirection);
            approach = false;
        }
    }

    public override void PerformAttack(Vector2 playerPosition)
    {
        approach = false;

        attackCoroutine = StartCoroutine(AttackMovement(playerPosition));
    }

    public override void InterruptAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    public IEnumerator AttackMovement(Vector2 playerPosition)
    {
        Vector2 playerDirection = (playerPosition - (Vector2)transform.position).normalized;
        enemyRB.AddForce(-playerDirection * 3f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackWindupTime);
        enemyRB.AddForce(playerDirection * 10f, ForceMode2D.Impulse);
    }

    private Vector2 GetApproachDirection(Vector2 playerPosition)
    {
        Vector2 playerDirection = playerPosition - (Vector2)transform.position;
        float playerDistance = Vector2.Distance(playerPosition, transform.position);
        float desiredDistanceRatio = (playerDistance - approachDistance) / playerDistance;
        Vector2 approachDirection = playerDirection * desiredDistanceRatio;

        return approachDirection;
    }

    public override void ApproachPlayer(Vector2 playerPosition)
    {
        approach = true;

        approachRefreshTimer += Time.deltaTime;

        if (approachRefreshTimer >= approachRefreshCD)
        {
            RefreshApproachDirection(playerPosition);
            approachRefreshTimer = 0f;
        }
    }

    private void RefreshApproachDirection(Vector2 playerPosition)
    {
        Vector2 approachDirection = GetApproachDirection(playerPosition).normalized;

        //raycast in front of bat. If no hit, then set move direction to approach direction
        if (
            !Physics2D.Raycast(
                transform.position,
                approachDirection,
                obstacleRaycastDistance,
                environmentMask
            )
        )
        {
            moveDirection = approachDirection;
            return;
        }

        //if hit, get alternate approach 45degrees left, then 45 right.

        Vector2 altRightDirection =
            Quaternion.AngleAxis(alternateRaycastOffsetDegrees, -Vector3.forward)
            * approachDirection;

        if (
            !Physics2D.Raycast(
                transform.position,
                altRightDirection,
                obstacleRaycastDistance,
                environmentMask
            )
        )
        {
            moveDirection = altRightDirection;
            return;
        }

        Vector2 altLeftDirection =
            Quaternion.AngleAxis(-alternateRaycastOffsetDegrees, -Vector3.forward)
            * approachDirection;

        if (
            !Physics2D.Raycast(
                transform.position,
                altLeftDirection,
                obstacleRaycastDistance,
                environmentMask
            )
        )
        {
            moveDirection = altLeftDirection;
            return;
        }
        //if no paths, go back
        moveDirection = -approachDirection;
    }
}
