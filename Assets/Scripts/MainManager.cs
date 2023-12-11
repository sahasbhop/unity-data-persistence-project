using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text BestScoreText;
    public Text ScoreText;
    public Text GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        ShowBestScore();
        SetScore(0);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        SetScore(m_Points);
    }

    private void ShowBestScore()
    {
        var highScore = HighScoreManager.LoadHighScore();
        if (highScore.records.Count == 0) return;

        var bestScoreRecord = highScore.records[0];
        BestScoreText.text = $"Best Score : {bestScoreRecord.playerName} : {bestScoreRecord.score}";
        BestScoreText.gameObject.SetActive(true);
    }

    private void SetScore(int point)
    {
        ScoreText.text = $"Score : {GetPlayerName()} : {point}";
    }

    public void GameOver()
    {
        m_GameOver = true;

        HighScoreManager.SubmitPlayerScore(GetPlayerName(), m_Points);
        var highScore = HighScoreManager.LoadHighScore();

        string gameOverText = "GAME OVER\n";

        if (highScore.records.Count > 0)
        {
            gameOverText += $"\nTop {HighScoreManager.MaxRecordsNumber} High Score\n";
        }

        for (int i = 0; i < highScore.records.Count; i++)
        {
            var record = highScore.records[i];
            gameOverText += $"({i + 1}) {record.playerName} : {record.score}\n";
        }

        gameOverText += "\nPress Space to Restart";
        GameOverText.text = gameOverText;
        GameOverText.gameObject.SetActive(true);
    }

    private string GetPlayerName()
    {
        return SessionManager.Instance == null
            ? SessionManager.DefaultPlayerName
            : SessionManager.Instance.GetPlayerName();
    }
}