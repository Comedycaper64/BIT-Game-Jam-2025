using UnityEngine;
using UnityEngine.Tilemaps;

public class LitGrid : MonoBehaviour
{
    [SerializeField]
    private Tilemap[] litRenderers;

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
        foreach (Tilemap renderer in litRenderers)
        {
            renderer.gameObject.SetActive(false);
        }
    }

    private void ChangeSpriteColour(object sender, Color newColour)
    {
        foreach (Tilemap tilemap in litRenderers)
        {
            tilemap.color = newColour;
        }
    }
}
