using UnityEngine;

public class BoarStateMachine : StateMachine
{
    private const float FASTER_SPEED = 36;

    [SerializeField]
    private Collider2D boarCollider;

    [SerializeField]
    private EnemyContactDamager enemyContactDamager;

    [SerializeField]
    private AudioClip boarSpawnSFX;

    public override void SpawnEnemy()
    {
        enemyHealth.SetMaxHealth();
        SwitchState(new EnemySpawnState(this));
        AudioManager.PlaySFX(boarSpawnSFX, 0.25f, 0, transform.position);
    }

    public override void ToggleInactive(bool toggle)
    {
        boarCollider.enabled = !toggle;
        enemyContactDamager.ToggleDamager(!toggle);
    }

    public void SetNewSpeed()
    {
        enemyMovement.SetNewSpeed(FASTER_SPEED);
    }
}
