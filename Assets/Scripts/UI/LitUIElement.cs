using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitUIElement : MonoBehaviour
{
    [SerializeField]
    private Image[] litRenderers;

    [SerializeField]
    private TextMeshProUGUI[] litText;

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
        foreach (Image renderer in litRenderers)
        {
            renderer.enabled = false;
        }
        foreach (TextMeshProUGUI text in litText)
        {
            text.enabled = false;
        }
    }

    private void ChangeSpriteColour(object sender, Color newColour)
    {
        foreach (Image renderer in litRenderers)
        {
            renderer.color = newColour;
        }

        foreach (TextMeshProUGUI text in litText)
        {
            text.color = newColour;
        }
    }
}
