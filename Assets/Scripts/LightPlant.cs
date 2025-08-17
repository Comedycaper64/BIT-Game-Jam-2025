using System;
using System.Collections;
using UnityEngine;

public class LightPlant : MonoBehaviour
{
    private bool isFilled = false;
    private bool lastPlant = false;
    private bool isBroken = false;
    private bool isBreaking = false;
    private float feedingLightIncrease = 25f;
    private float lightStored;
    private float drainStopTime = 3f;

    [SerializeField]
    private float lightStoredInitial = 100f;

    [SerializeField]
    private float lightStoredMax = 100f;
    private float breakGraceTime = 5f;

    [SerializeField]
    private Collider2D plantCollider;

    [SerializeField]
    private GameObject[] lightDithers;

    [SerializeField]
    private Animator[] plantAnimators;

    [SerializeField]
    private AudioClip lightPlantFed;

    [SerializeField]
    private AudioClip lightPlantDie;
    private Coroutine graceCoroutine;
    private Coroutine stopDrainCoroutine;

    public Action OnPlantBroken;

    private void Awake()
    {
        lightStored = lightStoredInitial;
    }

    private void UpdatePlantVisual()
    {
        int lightPoints = GetLightPoints();

        for (int i = 0; i < plantAnimators.Length; i++)
        {
            plantAnimators[i].SetBool("lit", false);
            lightDithers[i].SetActive(false);
        }

        for (int i = 0; i < lightPoints; i++)
        {
            plantAnimators[i].SetBool("lit", true);
            lightDithers[i].SetActive(true);
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

        if (stopDrainCoroutine != null)
        {
            StopCoroutine(stopDrainCoroutine);
        }

        if (!lastPlant)
        {
            stopDrainCoroutine = StartCoroutine(StopDraining());
        }

        if (graceCoroutine != null)
        {
            isBreaking = false;
            StopCoroutine(graceCoroutine);
        }

        UpdatePlantVisual();
    }

    public void DrainLight(float drainAmount)
    {
        if (isBroken || isBreaking || isFilled)
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

    public void RemoveDrainStop()
    {
        lastPlant = true;
    }

    private IEnumerator StopDraining()
    {
        isFilled = true;

        yield return new WaitForSeconds(drainStopTime);

        isFilled = false;
    }

    private IEnumerator BreakGracePeriod()
    {
        isBreaking = true;

        yield return new WaitForSeconds(breakGraceTime);

        isBreaking = false;
        isBroken = true;

        AudioManager.PlaySFX(lightPlantDie, 1f, 0, transform.position);

        foreach (Animator animator in plantAnimators)
        {
            animator.SetTrigger("die");
        }
        plantCollider.enabled = false;
        OnPlantBroken?.Invoke();
    }
}
