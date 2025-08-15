using System.Collections;
using UnityEngine;

public class BirdMovement : EnemyMovement
{
    private bool approach = false;
    private float birdApproachDistance = 3.5f;
    private float attackWindupTime = 0.5f;
    private float birdObstacleRaycastDistance = 5f;
    private float birdAlternateRaycastOffsetDegrees = 60f;
    private float featherProjectileSpeed = 3f;
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
        AudioManager.PlaySFX(attackSFX, 1f, 0, transform.position);
        attackCoroutine = StartCoroutine(AttackMovement(playerPosition));
    }

    public override void InterruptAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    protected override float GetApproachDistance()
    {
        return birdApproachDistance;
    }

    protected override float GetObstacleOffset()
    {
        return birdObstacleRaycastDistance;
    }

    protected override float GetObstacleRaycastDistance()
    {
        return birdAlternateRaycastOffsetDegrees;
    }

    public IEnumerator AttackMovement(Vector2 playerPosition)
    {
        Vector2 playerDirection = (playerPosition - (Vector2)transform.position).normalized;
        enemyRB.AddForce(playerDirection * 2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackWindupTime);

        ProjectileManager.SpawnProjectile(
            transform.position,
            playerDirection,
            1,
            featherProjectileSpeed
        );
    }

    public override void ApproachPlayer(Vector2 playerPosition)
    {
        approach = true;

        approachRefreshTimer += Time.deltaTime;
        PlayFootsteps();

        if (approachRefreshTimer >= approachRefreshCD)
        {
            RefreshApproachDirection(playerPosition);
            approachRefreshTimer = 0f;
        }
    }
}
