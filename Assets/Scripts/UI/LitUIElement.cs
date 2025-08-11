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
    }

    private void OnDisable()
    {
        LightManager.OnLightLevelChange -= ChangeSpriteColour;
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
