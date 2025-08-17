using UnityEngine;

public class BirdStateMachine : StateMachine
{
    private const float FASTER_SPEED = 20;
    private const float FASTER_ATTACK = 1;

    [SerializeField]
    private Collider2D birdCollider;

    [SerializeField]
    private AudioClip birdSpawnSFX;

    public override void SpawnEnemy()
    {
        enemyHealth.SetMaxHealth();
        SwitchState(new EnemySpawnState(this));
        AudioManager.PlaySFX(birdSpawnSFX, 1f, 0, transform.position);
    }

    public override void ToggleInactive(bool toggle)
    {
        birdCollider.enabled = !toggle;
    }

    public void SetNewSpeed()
    {
        enemyMovement.SetNewSpeed(FASTER_SPEED);
        attackInterval = FASTER_ATTACK;
    }
}
