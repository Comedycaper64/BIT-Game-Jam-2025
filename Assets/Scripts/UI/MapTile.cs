using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    [SerializeField]
    private Image tileSprite;

    public void ToggleTileVisual(bool toggle)
    {
        tileSprite.enabled = toggle;
    }

    public void SetSprite(Sprite newSprite)
    {
        tileSprite.sprite = newSprite;
    }

    public void SetColour(Color newColour)
    {
        tileSprite.color = newColour;
    }
}
