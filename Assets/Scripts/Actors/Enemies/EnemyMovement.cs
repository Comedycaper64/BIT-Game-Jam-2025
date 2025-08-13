using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed = 10f;

    [SerializeField]
    protected LayerMask environmentMask;

    [SerializeField]
    protected Rigidbody2D enemyRB;

    public void KnockbackEnemy(Vector2 knockbackDirection) { }

    public abstract void ApproachPlayer(Vector2 playerPosition);

    public abstract void PerformAttack(Vector2 playerPosition);
    public abstract void InterruptAttack();
}
