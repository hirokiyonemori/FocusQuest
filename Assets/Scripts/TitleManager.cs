using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetDataButton;

    private void Start()
    {
        // ボタンのイベント登録
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (resetDataButton != null)
        {
            resetDataButton.onClick.AddListener(ResetPlayerData);
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void ResetPlayerData()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetPlayerData();
            Debug.Log("プレイヤーデータをリセットしました。");
        }
    }
}
