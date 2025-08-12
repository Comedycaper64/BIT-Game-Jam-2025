using UnityEngine;

public class EnemyContactDamager : MonoBehaviour
{
    private bool damagerActive = false;

    [SerializeField]
    private int damageGiven = 1;

    public void ToggleDamager(bool toggle)
    {
        damagerActive = toggle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!damagerActive)
        {
            return;
        }
        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && healthSystem.GetIsPlayer()
        )
        {
            healthSystem.TakeDamage(damageGiven);
        }
    }
}
