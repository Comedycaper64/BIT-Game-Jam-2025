using UnityEngine;

public class LitElement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] litRenderers;

    private void OnEnable()
    {
        LightManager.OnLightLevelChange += ChangeSpriteColour;
    }

    private void OnDisable()
    {
        LightManager.OnLightLevelChange -= ChangeSpriteColour;
    }

    private void ChangeSpriteColour(object sender, Color newColour)
    {
        foreach (SpriteRenderer renderer in litRenderers)
        {
            renderer.color = newColour;
        }
    }
}
