using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void StartGame()
    {
        SessionManager.Instance.playerName = playerNameText.text;
        SceneManager.LoadScene(1);
    }
}