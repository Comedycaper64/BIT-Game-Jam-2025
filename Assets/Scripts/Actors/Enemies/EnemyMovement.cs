using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed = 10f;

    [SerializeField]
    protected LayerMask environmentMask;

    [SerializeField]
    protected Transform visualTransform;

    [SerializeField]
    protected Rigidbody2D enemyRB;

    public void KnockbackEnemy(Vector2 knockbackDirection) { }

    protected void ApplyMovementForces(Vector2 moveDirection)
    {
        enemyRB.AddForce(moveDirection * movementSpeed);

        if (moveDirection.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (moveDirection.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public abstract void ApproachPlayer(Vector2 playerPosition);

    public abstract void PerformAttack(Vector2 playerPosition);
    public abstract void InterruptAttack();
}
