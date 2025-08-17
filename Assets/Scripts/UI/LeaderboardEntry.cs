using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rankText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void ResetEntry()
    {
        rankText.text = "";
        nameText.text = "";
        scoreText.text = "";
    }

    public void SetEntry(int rank, string name, float score)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
