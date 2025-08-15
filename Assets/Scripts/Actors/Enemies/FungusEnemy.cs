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

    [SerializeField]
    private Animator fungusAnimator;

    [SerializeField]
    private AudioClip fungusSpawnSFX;

    [SerializeField]
    private AudioClip fungusAttackSFX;

    private void Awake()
    {
        fungusCloud.ToggleCloud(false);
    }

    private void Update()
    {
        if (!fungusActive)
        {
            return;
        }

        cloudSpawnTimer += Time.deltaTime;

        if (cloudSpawnTimer >= cloudSpawnTime)
        {
            SpawnCloud();
            cloudSpawnTimer = 0f;
        }
    }

    private void SpawnCloud()
    {
        //aniamtor trigger for attack
        AudioManager.PlaySFX(fungusAttackSFX, 1f, 0, transform.position);
        int randomInt = Random.Range(0, cloudSpawnLocations.Length);
        fungusCloud.transform.position = cloudSpawnLocations[randomInt].position;
        fungusCloud.ToggleCloud(true);
    }

    public void SpawnFungus()
    {
        fungusActive = true;
        AudioManager.PlaySFX(fungusSpawnSFX, 1f, 0, transform.position);
    }
}
