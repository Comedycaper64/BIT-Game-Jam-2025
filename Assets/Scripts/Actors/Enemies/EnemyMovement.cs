using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public void KnockbackEnemy(Vector2 knockbackDirection) { }

    public abstract void ApproachPlayer();

    public abstract void PerformAttack();
}
