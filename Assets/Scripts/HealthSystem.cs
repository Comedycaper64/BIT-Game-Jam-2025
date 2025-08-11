using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;

    public abstract void TakeDamage(int damageAmount);

    public bool GetIsPlayer()
    {
        return isPlayer;
    }
}
