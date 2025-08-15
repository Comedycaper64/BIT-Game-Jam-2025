using System.Collections;
using UnityEngine;

public class FungusCloud : MonoBehaviour
{
    private bool cloudActive = false;
    private const int CLOUD_DAMAGE = 1;
    private const float CLOUD_SPAWN_TIME = 2f;
    private const float CLOUD_ACTIVE_TIME = 6f;

    [SerializeField]
    private GameObject cloudVisual;
    private Coroutine cloudSpawnCoroutine;

    private void OnDisable()
    {
        if (cloudSpawnCoroutine != null)
        {
            StopCoroutine(cloudSpawnCoroutine);
        }
    }

    public void ToggleCloud(bool toggle)
    {
        if (toggle)
        {
            cloudSpawnCoroutine = StartCoroutine(SpawnCloud());
        }
        else
        {
            cloudActive = false;
            cloudVisual.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!cloudActive)
        {
            return;
        }

        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && (healthSystem.GetIsPlayer() || healthSystem.GetComponent<BoarMovement>())
        )
        {
            healthSystem.TakeDamage(CLOUD_DAMAGE);
        }
    }

    private IEnumerator SpawnCloud()
    {
        float spawnFlash = CLOUD_SPAWN_TIME / 4f;

        for (int i = 0; i < 8; i++)
        {
            if ((i % 2) == 0)
            {
                cloudVisual.SetActive(false);
                yield return new WaitForSeconds(spawnFlash * 0.75f);
            }
            else
            {
                cloudVisual.SetActive(true);
                yield return new WaitForSeconds(spawnFlash * 0.25f);
            }
        }

        cloudActive = true;

        cloudSpawnCoroutine = StartCoroutine(TimedCloudPresence());
    }

    private IEnumerator TimedCloudPresence()
    {
        yield return new WaitForSeconds(CLOUD_ACTIVE_TIME);

        ToggleCloud(false);
    }
}
