using System.Collections;
using UnityEngine;

public class BoarMovement : EnemyMovement
{
    private bool approach = false;
    private float boarApproachDistance = 0f;
    private float boarFootStepTime = 1f;

    private float attackWindupTime = 0.5f;

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
        return boarApproachDistance;
    }

    protected override float GetFootStepTime()
    {
        return boarFootStepTime;
    }

    public IEnumerator AttackMovement(Vector2 playerPosition)
    {
        Vector2 playerDirection = (playerPosition - (Vector2)transform.position).normalized;
        enemyRB.AddForce(-playerDirection * 3f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackWindupTime);
        enemyRB.AddForce(playerDirection * 10f, ForceMode2D.Impulse);
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
