using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    private int footStepIndex = 0;
    private float footStepTimer = 0f;
    private float footStepTime = 0.75f;

    protected float approachRefreshCD = 0.1f;
    protected float approachRefreshTimer = 0f;

    protected float approachDistance = 2.25f;
    protected float obstacleRaycastDistance = 2f;
    protected float alternateRaycastOffsetDegrees = 45f;

    [SerializeField]
    protected float movementSpeed = 10f;

    protected Vector2 moveDirection = Vector2.zero;

    [SerializeField]
    protected LayerMask environmentMask;

    [SerializeField]
    protected Transform visualTransform;

    [SerializeField]
    protected Rigidbody2D enemyRB;

    [SerializeField]
    protected AudioClip[] footStepSFX;

    [SerializeField]
    protected AudioClip attackSFX;

    public void KnockbackEnemy(Vector2 knockbackDirection) { }

    protected void ApplyMovementForces(Vector2 moveDirection)
    {
        enemyRB.AddForce(moveDirection * movementSpeed);

        if (moveDirection.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (moveDirection.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public abstract void ApproachPlayer(Vector2 playerPosition);

    public abstract void PerformAttack(Vector2 playerPosition);
    public abstract void InterruptAttack();

    protected virtual float GetApproachDistance()
    {
        return approachDistance;
    }

    protected virtual float GetObstacleRaycastDistance()
    {
        return obstacleRaycastDistance;
    }

    protected virtual float GetObstacleOffset()
    {
        return alternateRaycastOffsetDegrees;
    }

    protected virtual float GetFootStepTime()
    {
        return footStepTime;
    }

    protected Vector2 GetApproachDirection(Vector2 playerPosition)
    {
        Vector2 playerDirection = playerPosition - (Vector2)transform.position;
        float playerDistance = Vector2.Distance(playerPosition, transform.position);
        float desiredDistanceRatio = (playerDistance - approachDistance) / playerDistance;
        Vector2 approachDirection = playerDirection * desiredDistanceRatio;

        return approachDirection;
    }

    protected void PlayFootsteps()
    {
        footStepTimer += Time.deltaTime;

        if (footStepTimer >= GetFootStepTime())
        {
            AudioManager.PlaySFX(footStepSFX[footStepIndex], 0.75f, 0, transform.position, false);
            footStepIndex++;
            footStepIndex = footStepIndex % footStepSFX.Length;
            footStepTimer = 0f;
        }
    }

    protected void RefreshApproachDirection(Vector2 playerPosition)
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
