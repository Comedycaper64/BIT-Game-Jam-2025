using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.OnAttackEvent += TryAttack;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= TryAttack;
    }

    private void TryAttack()
    {
        ProjectileManager.SpawnProjectile(transform.position, GetMouseDirection(), 1, 3f, true);
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = Input.mousePosition;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0;

        Vector2 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        return mouseDirection;
    }
}
