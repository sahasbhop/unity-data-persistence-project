using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;
    public const string DefaultPlayerName = "ANONYMOUS";
    private string _playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName(string playerName)
    {
        if (playerName == null)
        {
            _playerName = DefaultPlayerName;
            return;
        }

        var s = playerName.Trim().Replace("\u200B", "");
        if (s.Length == 0)
        {
            _playerName = DefaultPlayerName;
            return;
        }

        _playerName = s;
    }

    public string GetPlayerName()
    {
        return _playerName;
    }
}