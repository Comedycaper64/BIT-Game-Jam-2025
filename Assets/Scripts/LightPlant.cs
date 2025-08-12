using System;
using System.Collections;
using UnityEngine;

public class LightPlant : MonoBehaviour
{
    private bool isBroken = false;
    private bool isBreaking = false;
    private float feedingLightIncrease = 10f;
    private float lightStored;

    [SerializeField]
    private float lightStoredMax = 100f;
    private float breakGraceTime = 5f;
    private Coroutine graceCoroutine;

    public Action OnPlantBroken;

    private void Awake()
    {
        lightStored = lightStoredMax;
    }

    private void UpdatePlantVisual()
    {
        //sprite reflects lightStored
    }

    public void FeedLightPlant()
    {
        if (isBroken)
        {
            return;
        }

        lightStored = Mathf.Min(lightStoredMax, lightStored + feedingLightIncrease);

        if (graceCoroutine != null)
        {
            isBreaking = false;
            StopCoroutine(graceCoroutine);
        }

        UpdatePlantVisual();
    }

    public void DrainLight(float drainAmount)
    {
        if (isBroken || isBreaking)
        {
            return;
        }

        lightStored = Mathf.Max(lightStored - drainAmount, 0f);

        if (lightStored <= 0f)
        {
            graceCoroutine = StartCoroutine(BreakGracePeriod());
        }

        UpdatePlantVisual();
    }

    public float GetStoredLight()
    {
        return lightStored;
    }

    private IEnumerator BreakGracePeriod()
    {
        isBreaking = true;

        yield return new WaitForSeconds(breakGraceTime);

        isBreaking = false;
        isBroken = true;

        //change sprite to reflect

        OnPlantBroken?.Invoke();
    }
}
