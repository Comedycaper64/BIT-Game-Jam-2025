using UnityEngine;

public class BatStateMachine : StateMachine
{
    private const float FASTER_SPEED = 14;
    private const float FASTER_ATTACK = 2;

    [SerializeField]
    private Collider2D batCollider;

    [SerializeField]
    private EnemyContactDamager enemyContactDamager;

    [SerializeField]
    private AudioClip batSpawnSFX;

    public override void SpawnEnemy()
    {
        SwitchState(new EnemySpawnState(this));
        AudioManager.PlaySFX(batSpawnSFX, 1f, 0, transform.position);
    }

    public override void ToggleInactive(bool toggle)
    {
        batCollider.enabled = !toggle;
        enemyContactDamager.ToggleDamager(!toggle);
    }

    public void SetNewSpeed()
    {
        enemyMovement.SetNewSpeed(FASTER_SPEED);
        attackInterval = FASTER_ATTACK;
    }
}
