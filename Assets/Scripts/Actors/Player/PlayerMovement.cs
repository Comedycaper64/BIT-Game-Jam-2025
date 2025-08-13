using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove;
    private bool dashAvailable;
    private float dashModifier = 1f;
    private float dashTimer = 0f;
    private PlayerStats stats;
    private Coroutine dashCoroutine;
    private InputManager inputManager;

    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    private AudioClip dashSFX;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;

        ToggleCanMove(true);

        PlayerManager.OnPlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnDashEvent -= TryDash;

        PlayerManager.OnPlayerDead -= OnPlayerDead;

        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
    }

    private void Update()
    {
        if (canMove)
        {
            if (!dashAvailable)
            {
                dashTimer += Time.deltaTime;

                if (dashTimer >= stats.GetDashRechargeTime())
                {
                    dashAvailable = true;
                    dashTimer = 0f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveRB();
        }
    }

    private Vector2 MoveRB()
    {
        Vector2 movementValue = inputManager.MovementValue.normalized;
        playerRb.MovePosition(
            playerRb.position
                + new Vector2(movementValue.x, movementValue.y)
                    * stats.GetMovementSpeed()
                    * dashModifier
                    * Time.fixedDeltaTime
        );

        if (movementValue.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movementValue.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        return movementValue;
    }

    private void TryDash()
    {
        if (!dashAvailable)
        {
            return;
        }

        dashCoroutine = StartCoroutine(ApplyDash());

        AudioManager.PlaySFX(dashSFX, 1f, 0, transform.position);

        dashAvailable = false;
    }

    private IEnumerator ApplyDash()
    {
        dashModifier = stats.GetDashSpeedModifier();
        yield return new WaitForSeconds(stats.GetDashTime());
        dashModifier = 1f;
    }

    public void ToggleCanMove(bool enable)
    {
        if (enable)
        {
            if (!canMove)
            {
                InputManager.Instance.OnDashEvent += TryDash;
            }
        }
        else
        {
            InputManager.Instance.OnDashEvent -= TryDash;
        }

        canMove = enable;
    }

    private void OnPlayerDead(object sender, bool playerDead)
    {
        ToggleCanMove(!playerDead);
    }
}
