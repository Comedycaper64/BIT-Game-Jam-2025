using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool levelActive = false;
    private float levelSurviveTime = 0f;
    private float timeBetweenEnemySpawns = 4f;
    private float enemySpawnTimer = 0f;

    private const float BIRD_SPAWN_TIME = 15f;
    private const float BOAR_SPAWN_TIME = 30f;

    [SerializeField]
    private LightManager lightManager;

    [SerializeField]
    private EnemySpawner enemySpawner;
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

    private void GameStateCheck() { }

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
        levelActive = false;
        OnGameEnd?.Invoke();
    }
}
