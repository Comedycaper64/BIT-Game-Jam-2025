using UnityEngine;

public class BoarStateMachine : StateMachine
{
    [SerializeField]
    private GameObject boarVisual;

    [SerializeField]
    private Collider2D boarCollider;

    [SerializeField]
    private EnemyContactDamager enemyContactDamager;

    [SerializeField]
    private AudioClip boarSpawnSFX;

    public override void SpawnEnemy()
    {
        //SwitchState(new BatSpawnState(this));
        AudioManager.PlaySFX(boarSpawnSFX, 1f, 0, transform.position);
    }

    public override void ToggleInactive(bool toggle)
    {
        boarVisual.SetActive(!toggle);
        boarCollider.enabled = !toggle;
        enemyContactDamager.ToggleDamager(!toggle);
    }
}
