using UnityEngine;

public class LitElement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] litRenderers;

    private void OnEnable()
    {
        LightManager.OnLightLevelChange += ChangeSpriteColour;
        LightManager.OnLightExtinguished += DisableSprites;
    }

    private void OnDisable()
    {
        LightManager.OnLightLevelChange -= ChangeSpriteColour;
        LightManager.OnLightExtinguished -= DisableSprites;
    }

    private void DisableSprites()
    {
        foreach (SpriteRenderer renderer in litRenderers)
        {
            renderer.enabled = false;
        }
    }

    private void ChangeSpriteColour(object sender, Color newColour)
    {
        foreach (SpriteRenderer renderer in litRenderers)
        {
            renderer.color = newColour;
        }
    }
}
