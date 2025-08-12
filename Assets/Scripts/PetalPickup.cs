using System.Collections;
using UnityEngine;

public class PetalPickup : MonoBehaviour
{
    [SerializeField]
    private bool persistent = false;
    private int petalValue = 1;
    private float dropDuration;

    [SerializeField]
    private Collider2D pickupCollider;

    [SerializeField]
    private GameObject pickupVisual;
    private Coroutine disappearCoroutine;

    private void Awake()
    {
        if (!persistent)
        {
            DespawnPetal();
        }
    }

    public void SpawnPetal(int petalValue, float dropDuration)
    {
        this.petalValue = petalValue;
        this.dropDuration = dropDuration;

        pickupCollider.enabled = true;
        pickupVisual.SetActive(true);

        disappearCoroutine = StartCoroutine(DisappearCountdown());
    }

    private IEnumerator DisappearCountdown()
    {
        yield return new WaitForSeconds(dropDuration - 2f);

        for (int i = 0; i < 8; i++)
        {
            if ((i % 2) == 0)
            {
                pickupVisual.SetActive(false);
                yield return new WaitForSeconds(0.125f);
            }
            else
            {
                pickupVisual.SetActive(true);
                yield return new WaitForSeconds(0.375f);
            }
        }

        DespawnPetal();
    }

    private void DespawnPetal()
    {
        pickupCollider.enabled = false;
        pickupVisual.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
        {
            playerManager.IncrementPetalCounter(petalValue);

            DespawnPetal();

            if (disappearCoroutine != null)
            {
                StopCoroutine(disappearCoroutine);
            }
        }
    }
}
