using UnityEngine;

public class FungusCloud : MonoBehaviour
{
    private bool cloudActive = false;

    public void ToggleCloud(bool toggle)
    {
        cloudActive = toggle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player health, damage
        // if boar health, damage
    }
}
