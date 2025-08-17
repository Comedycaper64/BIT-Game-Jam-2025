using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove;
    private bool dashAvailable;
    private int footstepIndex = 0;
    private float dashModifier = 1f;
    private float dashTimer = 0f;
    private float footStepTimer = 0f;
    private const float TIME_BETWEEN_STEPS = 1.5f;
    private PlayerStats stats;
    private Coroutine dashCoroutine;
    private InputManager inputManager;

    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator playerHeadAnimator;

    [SerializeField]
    private Slider dashSlider;

    [SerializeField]
    private AudioClip dashSFX;

    [SerializeField]
    private AudioClip[] footstepSFX;

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

                float dashRechargeTime = stats.GetDashRechargeTime();

                dashSlider.value = dashTimer / dashRechargeTime;

                if (dashTimer >= dashRechargeTime)
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

        if (movementValue.sqrMagnitude > 0f)
        {
            playerAnimator.SetBool("moving", true);
            playerHeadAnimator.SetBool("moving", true);

            footStepTimer += Time.deltaTime;

            if (footStepTimer >= (TIME_BETWEEN_STEPS / stats.GetMovementSpeed()))
            {
                AudioManager.PlaySFX(
                    footstepSFX[footstepIndex],
                    0.75f,
                    0,
                    transform.position,
                    false
                );
                footstepIndex++;
                footstepIndex = footstepIndex % footstepSFX.Length;
                footStepTimer = 0f;
            }
        }
        else
        {
            playerAnimator.SetBool("moving", false);
            playerHeadAnimator.SetBool("moving", false);
        }

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
