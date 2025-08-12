using UnityEngine;

public class PetalManager : MonoBehaviour
{
    private static int petalIndex = 0;

    [SerializeField]
    private PetalPickup[] localPetals;
    private static PetalPickup[] petals;

    private void OnEnable()
    {
        petals = localPetals;

        petalIndex = 0;
    }

    public static void SpawnPetal(Vector3 dropLocation, int petalValue, float dropDuration)
    {
        PetalPickup spawnedPetal = petals[petalIndex];

        spawnedPetal.transform.position = dropLocation;

        spawnedPetal.SpawnPetal(petalValue, dropDuration);

        petalIndex++;

        if (petalIndex >= petals.Length)
        {
            petalIndex = 0;
        }
    }
}
