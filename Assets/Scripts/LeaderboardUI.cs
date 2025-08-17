using TMPro;
using UnityEngine;

using Dan.Main;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField]
    private LeaderboardEntry[] lbEntries;

    [SerializeField]
    private TMP_InputField nameInput;

    [SerializeField]
    private TextMeshProUGUI playerScore;

    [SerializeField]
    private GameObject leaderboard;

    [SerializeField]
    private GameObject submitWindow;

    private void Start()
    {
        playerScore.text = PlayerOptions.GetHighScore().ToString("0.0");
        foreach (LeaderboardEntry lbentry in lbEntries)
        {
            lbentry.ResetEntry();
        }
        LoadEntries();
    }

    private void LoadEntries()
    {
        Leaderboards.BloomLeaderboard.GetEntries(entries =>
        {
            foreach (LeaderboardEntry lbentry in lbEntries)
            {
                lbentry.ResetEntry();
            }

            int length = Mathf.Min(lbEntries.Length, entries.Length);

            for (int i = 0; i < length; i++)
            {
                Dan.Models.Entry entry = entries[i];
                lbEntries[i].SetEntry(entry.Rank, entry.Username, entry.Score);
            }
        });
    }

    public void UploadEntry()
    {
        if (nameInput.text == "")
        {
            return;
        }

        Leaderboards.BloomLeaderboard.UploadNewEntry(
            nameInput.text,
            Mathf.RoundToInt(PlayerOptions.GetHighScore()),
            isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            }
        );
    }

    public void ToggleSubmitModal(bool toggle)
    {
        submitWindow.SetActive(toggle);
    }

    public void ToggleLeaderboard(bool toggle)
    {
        leaderboard.SetActive(toggle);
        submitWindow.SetActive(false);
    }
}
