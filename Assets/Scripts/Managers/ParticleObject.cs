using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particlesSystem;

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
        particlesSystem.gameObject.SetActive(false);
    }

    private void ChangeSpriteColour(object sender, Color newColour)
    {
        var main = particlesSystem.main;
        main.startColor = newColour;
    }

    public void PlayParticleEffect()
    {
        particlesSystem.Play();
    }
}
