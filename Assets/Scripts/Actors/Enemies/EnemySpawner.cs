using UnityEngine;
using UnityEngine.UIElements;

public enum SpawnerBehaviour
{
    bats,
    birds,
    boars,
    fastBats,
    fastBirds
}

public class EnemySpawner : MonoBehaviour
{
    private int spawnGridIndex = 0;
    private int batIndex = 0;
    private int birdIndex = 0;
    private int boarIndex = 0;
    private int aliveEnemies = 0;
    private int enemySpawnLimit;

    private const int SPAWN_AVAILABILITY_CHECKS = 5;
    private const float SPAWN_AVAILABILITY_RADIUS = 0.25f;

    private const int HAWK_THRESHOLD = 50;
    private const int BOAR_THRESHOLD = 80;

    private const int ENEMY_LIMIT_EASY = 4;
    private const int ENEMY_LIMIT_MEDIUM = 5;
    private const int ENEMY_LIMIT_HARD = 6;

    [SerializeField]
    private Transform spawnGrid;

    [SerializeField]
    private Transform[] spawnLocations;

    [SerializeField]
    private LayerMask environmentLayermask;

    [SerializeField]
    private BatStateMachine[] batEnemies;

    [SerializeField]
    private BirdStateMachine[] birdEnemies;

    [SerializeField]
    private BoarStateMachine[] boarEnemies;

    private SpawnerBehaviour spawnerBehaviour = SpawnerBehaviour.birds;

    private void Start()
    {
        spawnGridIndex = Random.Range(0, spawnLocations.Length);
        enemySpawnLimit = ENEMY_LIMIT_EASY;
        aliveEnemies = 0;
    }

    private void OnEnable()
    {
        EnemyDeadState.OnEnemyDead += ReduceEnemyCount;
    }

    private void OnDisable()
    {
        EnemyDeadState.OnEnemyDead -= ReduceEnemyCount;
    }

    public void SpawnEnemy()
    {
        if (aliveEnemies >= enemySpawnLimit)
        {
            return;
        }

        if (!TryGenerateSpawnLocation(out Vector3 spawnLocation))
        {
            return;
        }

        RandomlyChooseSpawnedEnemy(spawnLocation);
        aliveEnemies++;
    }

    private bool TryGenerateSpawnLocation(out Vector3 spawnLocation)
    {
        spawnLocation = Vector3.zero;

        spawnGrid.position = PlayerIdentifier.PlayerTransform.position;

        for (int i = 0; i < SPAWN_AVAILABILITY_CHECKS; i++)
        {
            Vector3 testSpawn = spawnLocations[spawnGridIndex].position;

            spawnGridIndex++;

            if (spawnGridIndex >= spawnLocations.Length)
            {
                spawnGridIndex = 0;
            }

            if (
                !Physics2D.OverlapCircle(testSpawn, SPAWN_AVAILABILITY_RADIUS, environmentLayermask)
            )
            {
                spawnLocation = testSpawn;
                return true;
            }
        }

        return false;
    }

    private void RandomlyChooseSpawnedEnemy(Vector3 spawnLocation)
    {
        int randomSelection = Random.Range(0, 100);

        if (
            (randomSelection >= BOAR_THRESHOLD)
            && ((int)spawnerBehaviour >= (int)SpawnerBehaviour.boars)
        )
        {
            SpawnBoar(spawnLocation);
            return;
        }

        if (
            (randomSelection >= HAWK_THRESHOLD)
            && ((int)spawnerBehaviour >= (int)SpawnerBehaviour.birds)
        )
        {
            SpawnHawk(spawnLocation);
            return;
        }

        SpawnBat(spawnLocation);
    }

    private void SpawnBat(Vector3 spawnPosition)
    {
        BatStateMachine spawnedBat = batEnemies[batIndex];

        spawnedBat.transform.position = spawnPosition;

        // if ((int)spawnerBehaviour >= (int)SpawnerBehaviour.fastBirds)
        // {
        //     //Spawn Fast birds
        // }

        spawnedBat.SpawnEnemy();

        batIndex++;

        if (batIndex >= batEnemies.Length)
        {
            batIndex = 0;
        }
    }

    private void SpawnHawk(Vector3 spawnPosition)
    {
        BirdStateMachine spawnedBird = birdEnemies[birdIndex];

        spawnedBird.transform.position = spawnPosition;

        // if ((int)spawnerBehaviour >= (int)SpawnerBehaviour.fastBats)
        // {
        //     //Spawn Fast Bats
        // }

        spawnedBird.SpawnEnemy();

        birdIndex++;

        if (birdIndex >= birdEnemies.Length)
        {
            birdIndex = 0;
        }
    }

    private void SpawnBoar(Vector3 spawnPosition)
    {
        BoarStateMachine spawnedBoar = boarEnemies[boarIndex];

        spawnedBoar.transform.position = spawnPosition;

        spawnedBoar.SpawnEnemy();

        boarIndex++;

        if (boarIndex >= boarEnemies.Length)
        {
            boarIndex = 0;
        }
    }

    public void UpdateSpawnerBehaviour(SpawnerBehaviour newBehaviour)
    {
        spawnerBehaviour = newBehaviour;

        if (spawnerBehaviour == SpawnerBehaviour.boars)
        {
            enemySpawnLimit = ENEMY_LIMIT_MEDIUM;
        }
        else if (spawnerBehaviour == SpawnerBehaviour.fastBirds)
        {
            enemySpawnLimit = ENEMY_LIMIT_HARD;
        }
    }

    private void ReduceEnemyCount()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
    }
}
