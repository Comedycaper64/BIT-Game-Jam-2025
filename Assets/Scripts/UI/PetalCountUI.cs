using TMPro;
using UnityEngine;

public class PetalCountUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI petalText;

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
        petalText.text = "Petals: " + newPetal;
    }
}
