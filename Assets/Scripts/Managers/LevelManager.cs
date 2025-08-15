using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool levelActive = false;
    private int difficultyIncreaseIndex = 0;
    private float levelSurviveTime = 0f;
    private float timeBetweenEnemySpawns = 4f;
    private float enemySpawnTimer = 0f;

    // private const float BIRD_SPAWN_TIME = 15f;
    // private const float BOAR_SPAWN_TIME = 30f;
    [SerializeField]
    private float[] difficultyIncreaseTimes;

    [SerializeField]
    private LightManager lightManager;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private AudioClip gameOverSFX;
    public static Action OnGameEnd;

    private void Awake()
    {
        GameStartUI.OnGameStart += StartGame;
        PlayerManager.OnPlayerDead += PlayerDeadGameOver;
        LightManager.OnLightExtinguished += GameOver;
    }

    private void OnDisable()
    {
        GameStartUI.OnGameStart -= StartGame;
        PlayerManager.OnPlayerDead -= PlayerDeadGameOver;
        LightManager.OnLightExtinguished -= GameOver;
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
        }
    }

    private void IncreaseDifficulty()
    {
        int currentBehaviour = (int)enemySpawner.GetSpawnerBehaviour();
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
        //Enable player
        lightManager.BeginLightCheck();
        levelActive = true;
    }

    private void PlayerDeadGameOver(object sender, bool e)
    {
        GameOver();
    }

    private void GameOver()
    {
        AudioManager.PlaySFX(gameOverSFX, 1f, 0, transform.position);

        levelActive = false;
        OnGameEnd?.Invoke();
    }
}
