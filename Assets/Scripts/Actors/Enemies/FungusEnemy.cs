using UnityEngine;

public class FungusEnemy : MonoBehaviour
{
    private bool fungusActive = false;

    private float cloudSpawnTime = 10f;
    private float cloudSpawnTimer = 0f;

    [SerializeField]
    private FungusCloud fungusCloud;

    [SerializeField]
    private Transform[] cloudSpawnLocations;

    private void Update()
    {
        if (!fungusActive)
        {
            return;
        }

        cloudSpawnTimer += Time.deltaTime;

        if (cloudSpawnTimer >= cloudSpawnTime)
        {
            //Spawn Fungal Cloud
            cloudSpawnTimer = 0f;
        }
    }

    public void SpawnFungus()
    {
        fungusActive = true;
    }
}
