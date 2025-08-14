using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    [SerializeField]
    private int petalDropValue = 1;

    [SerializeField]
    private float attackRange = 3f;

    [SerializeField]
    private float attackInterval = 3f;

    [SerializeField]
    private float attackTime = 1f;
    private State currentState;
    private Transform playerTransform;
    protected EnemyHealth enemyHealth;
    public EnemyMovement enemyMovement;

    public Animator smAnimator;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        enemyHealth.OnDeath += KillEnemy;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
        ToggleInactive(true);
        ResetEnemy();
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= KillEnemy;
    }

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public virtual void SwitchState(State newState)
    {
        if (currentState?.GetType() == newState.GetType())
        {
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public abstract void SpawnEnemy();

    public virtual void ResetEnemy()
    {
        SwitchState(new EnemyInactiveState(this));
    }

    public virtual void KillEnemy()
    {
        SwitchState(new EnemyDeadState(this));

        PetalManager.SpawnPetal(transform.position, petalDropValue, petalDropValue * 5);
    }

    public abstract void ToggleInactive(bool toggle);

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetAttackInterval()
    {
        return attackInterval;
    }

    public float GetAttackTime()
    {
        return attackTime;
    }
}
