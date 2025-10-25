using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Timer UI")]
    [SerializeField] private Text timerText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resetButton;

    [Header("Player Stats UI")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text expText;
    [SerializeField] private Text goldText;
    [SerializeField] private Slider expSlider;

    [Header("Battle UI")]
    [SerializeField] private Text battleLogText;
    [SerializeField] private ScrollRect battleLogScrollRect;

    private TimerManager timerManager;
    private BattleManager battleManager;
    private PlayerStats playerStats;

    private void Start()
    {
        // コンポーネントの取得
        timerManager = FindObjectOfType<TimerManager>();
        battleManager = FindObjectOfType<BattleManager>();
        playerStats = PlayerStats.Instance;

        // イベントの登録
        if (timerManager != null)
        {
            timerManager.OnTimerStart.AddListener(OnTimerStart);
            timerManager.OnTimerPause.AddListener(OnTimerPause);
            timerManager.OnTimerReset.AddListener(OnTimerReset);
            timerManager.OnTimerComplete.AddListener(OnTimerComplete);
        }

        // ボタンのイベント登録
        if (startButton != null)
            startButton.onClick.AddListener(StartTimer);
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseTimer);
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetTimer);

        // 初期UI更新
        UpdatePlayerStats();
        UpdateTimerDisplay();
        UpdateButtonStates();
    }

    private void Update()
    {
        UpdateTimerDisplay();
        UpdatePlayerStats();
    }

    private void UpdateTimerDisplay()
    {
        if (timerManager != null && timerText != null)
        {
            timerText.text = timerManager.GetFormattedTime();
        }
    }

    private void UpdatePlayerStats()
    {
        if (playerStats == null) return;

        // レベル表示
        if (levelText != null)
        {
            levelText.text = $"Lv. {playerStats.Level}";
        }

        // 経験値表示
        if (expText != null)
        {
            expText.text = $"Exp: {playerStats.ExpProgress}/{playerStats.ExpRequiredForNextLevel}";
        }

        // ゴールド表示
        if (goldText != null)
        {
            goldText.text = $"Gold: {playerStats.Gold}";
        }

        // 経験値スライダー
        if (expSlider != null)
        {
            float progress = (float)playerStats.ExpProgress / playerStats.ExpRequiredForNextLevel;
            expSlider.value = progress;
        }
    }

    private void UpdateButtonStates()
    {
        if (timerManager == null) return;

        bool isRunning = timerManager.isRunning;
        bool isPaused = timerManager.isPaused;

        if (startButton != null)
        {
            startButton.interactable = !isRunning || isPaused;
        }

        if (pauseButton != null)
        {
            pauseButton.interactable = isRunning && !isPaused;
        }

        if (resetButton != null)
        {
            resetButton.interactable = isRunning || isPaused;
        }
    }

    private void StartTimer()
    {
        if (timerManager != null)
        {
            timerManager.StartTimer();
            UpdateButtonStates();
        }
    }

    private void PauseTimer()
    {
        if (timerManager != null)
        {
            timerManager.PauseTimer();
            UpdateButtonStates();
        }
    }

    private void ResetTimer()
    {
        if (timerManager != null)
        {
            timerManager.ResetTimer();
            UpdateButtonStates();
        }
    }

    private void OnTimerStart()
    {
        if (battleManager != null)
        {
            battleManager.StartBattle();
        }
        UpdateButtonStates();
    }

    private void OnTimerPause()
    {
        if (battleManager != null)
        {
            battleManager.StopBattle();
        }
        UpdateButtonStates();
    }

    private void OnTimerReset()
    {
        if (battleManager != null)
        {
            battleManager.StopBattle();
            battleManager.ClearBattleLog();
        }
        UpdateButtonStates();
    }

    private void OnTimerComplete()
    {
        if (battleManager != null)
        {
            battleManager.StopBattle();
        }

        if (playerStats != null)
        {
            playerStats.AddFocusSession();
        }

        UpdateButtonStates();

        // 集中完了メッセージ
        if (battleLogText != null)
        {
            battleLogText.text += "\n<color=cyan>🎉 25分の集中が完了しました！</color>\n";
        }
    }

    // タイトルシーンへの遷移
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // メインシーンへの遷移
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
