using UnityEngine;
using UnityEngine.Tilemaps;

public class LitGrid : MonoBehaviour
{
    [SerializeField]
    private Tilemap[] litRenderers;

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
        foreach (Tilemap tilemap in litRenderers)
        {
            tilemap.color = newColour;
        }
    }
}
