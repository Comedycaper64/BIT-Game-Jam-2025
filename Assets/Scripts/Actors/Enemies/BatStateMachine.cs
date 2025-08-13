using UnityEngine;

public class BatStateMachine : StateMachine
{
    [SerializeField]
    private GameObject batVisual;

    [SerializeField]
    private Collider2D batCollider;

    [SerializeField]
    private EnemyContactDamager enemyContactDamager;

    [SerializeField]
    private AudioClip batSpawnSFX;

    public override void SpawnEnemy()
    {
        SwitchState(new BatSpawnState(this));
        AudioManager.PlaySFX(batSpawnSFX, 1f, 0, transform.position);
    }

    public override void ToggleInactive(bool toggle)
    {
        batVisual.SetActive(!toggle);
        batCollider.enabled = !toggle;
        enemyContactDamager.ToggleDamager(!toggle);
    }
}
