using UnityEngine;

public class BatStateMachine : StateMachine
{
    [SerializeField]
    private GameObject batVisual;

    [SerializeField]
    private Collider2D batCollider;

    [SerializeField]
    private EnemyContactDamager enemyContactDamager;

    public override void SpawnEnemy()
    {
        SwitchState(new BatSpawnState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        batVisual.SetActive(!toggle);
        batCollider.enabled = !toggle;
        enemyContactDamager.ToggleDamager(!toggle);
    }
}
