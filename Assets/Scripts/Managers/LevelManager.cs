using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool levelActive = false;
    private int difficultyIncreaseIndex = 0;
    private float levelSurviveTime = 0f;
    private const float ENEMY_SPAWN_CD_START = 3f;
    private const float ENEMY_SPAWN_CD_DECREASE = 0.2f;
    private float timeBetweenEnemySpawns;
    private float enemySpawnTimer = 0f;

    [SerializeField]
    private float[] difficultyIncreaseTimes;

    [SerializeField]
    private LightManager lightManager;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private AudioClip gameOverSFX;
    public static EventHandler<bool> OnGameEndLight;
    public static EventHandler<float> OnSetGameTime;

    private void Awake()
    {
        GameStartUI.OnGameStart += StartGame;
        PlayerManager.OnPlayerDead += PlayerDeadGameOver;
        LightManager.OnLightExtinguished += LightsOutGameOver;
    }

    private void OnDisable()
    {
        GameStartUI.OnGameStart -= StartGame;
        PlayerManager.OnPlayerDead -= PlayerDeadGameOver;
        LightManager.OnLightExtinguished -= LightsOutGameOver;
    }

    private void Update()
    {
        if (!levelActive)
        {
            return;
        }

        levelSurviveTime += Time.deltaTime;

        EnemySpawnCheck();

        GameStateCheck();
    }

    private void GameStateCheck()
    {
        if (difficultyIncreaseIndex >= difficultyIncreaseTimes.Length)
        {
            return;
        }

        if (levelSurviveTime > difficultyIncreaseTimes[difficultyIncreaseIndex])
        {
            IncreaseDifficulty();
            difficultyIncreaseIndex++;

            if (difficultyIncreaseIndex >= (difficultyIncreaseTimes.Length - 1))
            {
                lightManager.IncreaseLightDrainSpeed();
            }
        }
    }

    private void IncreaseDifficulty()
    {
        int currentBehaviour = (int)enemySpawner.GetSpawnerBehaviour();
        timeBetweenEnemySpawns -= ENEMY_SPAWN_CD_DECREASE;
        currentBehaviour++;
        enemySpawner.UpdateSpawnerBehaviour((SpawnerBehaviour)currentBehaviour);
    }

    private void EnemySpawnCheck()
    {
        enemySpawnTimer += Time.deltaTime;

        if (enemySpawnTimer > timeBetweenEnemySpawns)
        {
            enemySpawner.SpawnEnemy();
            enemySpawnTimer = 0f;
        }
    }

    private void StartGame()
    {
        InputManager.Instance.GameStart();
        lightManager.BeginLightCheck();
        levelActive = true;
        timeBetweenEnemySpawns = ENEMY_SPAWN_CD_START;
    }

    private void PlayerDeadGameOver(object sender, bool e)
    {
        GameOver(false);
    }

    private void LightsOutGameOver()
    {
        GameOver(true);
    }

    private void GameOver(bool lightExtinguished)
    {
        AudioManager.PlaySFX(gameOverSFX, 1f, 0, transform.position);

        levelActive = false;
        OnSetGameTime?.Invoke(this, levelSurviveTime);
        OnGameEndLight?.Invoke(this, lightExtinguished);
    }
}
