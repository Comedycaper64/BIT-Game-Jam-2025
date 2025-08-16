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
    private float lightStoredInitial = 100f;

    [SerializeField]
    private float lightStoredMax = 100f;
    private float breakGraceTime = 5f;

    [SerializeField]
    private Animator[] plantAnimators;

    [SerializeField]
    private AudioClip lightPlantFed;

    [SerializeField]
    private AudioClip lightPlantDie;
    private Coroutine graceCoroutine;

    public Action OnPlantBroken;

    private void Awake()
    {
        lightStored = lightStoredInitial;
    }

    private void UpdatePlantVisual()
    {
        int lightPoints = GetLightPoints();

        foreach (Animator animator in plantAnimators)
        {
            animator.SetBool("lit", false);
        }

        for (int i = 0; i < lightPoints; i++)
        {
            plantAnimators[i].SetBool("lit", true);
        }
    }

    public void FeedLightPlant()
    {
        if (isBroken)
        {
            return;
        }

        lightStored = Mathf.Min(lightStoredMax, lightStored + feedingLightIncrease);

        AudioManager.PlaySFX(lightPlantFed, 1f, 0, transform.position);

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

    public int GetLightPoints()
    {
        if (isBroken)
        {
            return -1;
        }

        return Mathf.CeilToInt(lightStored / 25f);
    }

    private IEnumerator BreakGracePeriod()
    {
        isBreaking = true;

        yield return new WaitForSeconds(breakGraceTime);

        isBreaking = false;
        isBroken = true;

        AudioManager.PlaySFX(lightPlantDie, 1f, 0, transform.position);

        //change sprite to reflect

        OnPlantBroken?.Invoke();
    }
}
