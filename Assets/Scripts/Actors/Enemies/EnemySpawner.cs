using UnityEngine;

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
    private int batIndex = 0;
    private int birdIndex = 0;
    private int boarIndex = 0;

    private const int HAWK_THRESHOLD = 50;
    private const int BOAR_THRESHOLD = 80;

    [SerializeField]
    private BatStateMachine[] batEnemies;

    [SerializeField]
    private BirdStateMachine[] birdEnemies;

    [SerializeField]
    private BoarStateMachine[] boarEnemies;

    private SpawnerBehaviour spawnerBehaviour = SpawnerBehaviour.birds;

    public void SpawnEnemy()
    {
        if (!TryGenerateSpawnLocation(out Vector3 spawnLocation))
        {
            return;
        }

        RandomlyChooseSpawnedEnemy(spawnLocation);
    }

    private bool TryGenerateSpawnLocation(out Vector3 spawnLocation)
    {
        spawnLocation = Vector3.zero;

        //Find spawn area on the outskirts of camera vision
        // Making sure that the spawn location isn't in an impassable object

        return true;
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
    }
}
