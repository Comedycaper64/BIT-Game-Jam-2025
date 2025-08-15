using UnityEngine;
using UnityEngine.UI;

public class PetalCountUI : MonoBehaviour
{
    // [SerializeField]
    // private TextMeshProUGUI petalText;

    [SerializeField]
    private Slider petalSlider;

    private void OnEnable()
    {
        PlayerManager.OnNewPetalCount += UpdatePetalUI;
    }

    private void OnDisable()
    {
        PlayerManager.OnNewPetalCount -= UpdatePetalUI;
    }

    private void UpdatePetalUI(object sender, int newPetal)
    {
        petalSlider.value = newPetal;
    }
}
