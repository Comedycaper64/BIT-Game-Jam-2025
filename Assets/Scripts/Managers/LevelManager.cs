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

    private void Awake()
    {
        GameStartUI.OnGameStart += StartGame;
    }

    private void OnDisable()
    {
        GameStartUI.OnGameStart -= StartGame;
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
}
