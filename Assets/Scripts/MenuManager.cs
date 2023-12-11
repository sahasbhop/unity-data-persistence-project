using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    private void Start()
    {
        Debug.Log("[menu] Start");

        var highScore = HighScoreManager.LoadHighScore();
        Debug.Log($"[menu] highScore: {highScore}");

        string summary = $"Top {HighScoreManager.MaxRecordsNumber} High Score\n";

        for (int i = 0; i < highScore.records.Count; i++)
        {
            var record = highScore.records[i];
            summary = summary + $"({i + 1}) {record.playerName} : {record.score}\n";
        }

        Debug.Log($"[menu] summary: {summary}");
    }

    public void StartGame()
    {
        SessionManager.Instance.SetPlayerName(playerNameText.text);
        SceneManager.LoadScene(1);
    }
}