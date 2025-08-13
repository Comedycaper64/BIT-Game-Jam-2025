using UnityEngine;

public class BirdStateMachine : StateMachine
{
    [SerializeField]
    private GameObject birdVisual;

    [SerializeField]
    private Collider2D birdCollider;

    [SerializeField]
    private AudioClip birdSpawnSFX;

    public override void SpawnEnemy()
    {
        SwitchState(new BirdSpawnState(this));
        AudioManager.PlaySFX(birdSpawnSFX, 1f, 0, transform.position);
    }

    public override void ToggleInactive(bool toggle)
    {
        birdVisual.SetActive(!toggle);
        birdCollider.enabled = !toggle;
    }
}
