using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level = 1;
    public int experience = 0;
    public int gold = 0;
    public int totalFocusSessions = 0;
}

public class PlayerStats : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Level Settings")]
    [SerializeField] private int baseExpRequired = 100;
    [SerializeField] private float expMultiplier = 1.5f;

    [Header("EasySave3 Settings")]
    [SerializeField] private string saveKey = "PlayerData";

    public static PlayerStats Instance { get; private set; }

    public int Level => playerData.level;
    public int Experience => playerData.experience;
    public int Gold => playerData.gold;
    public int TotalFocusSessions => playerData.totalFocusSessions;

    public int ExpRequiredForNextLevel => GetExpRequiredForLevel(playerData.level + 1);
    public int ExpProgress => playerData.experience - GetExpRequiredForLevel(playerData.level);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddExperience(int exp)
    {
        playerData.experience += exp;
        CheckLevelUp();
        SavePlayerData();
    }

    public void AddGold(int gold)
    {
        playerData.gold += gold;
        SavePlayerData();
    }

    public void AddFocusSession()
    {
        playerData.totalFocusSessions++;
        SavePlayerData();
    }

    private void CheckLevelUp()
    {
        int requiredExp = GetExpRequiredForLevel(playerData.level + 1);

        while (playerData.experience >= requiredExp)
        {
            playerData.level++;
            requiredExp = GetExpRequiredForLevel(playerData.level + 1);

            Debug.Log($"レベルアップ！ レベル {playerData.level} になりました！");
        }
    }

    private int GetExpRequiredForLevel(int level)
    {
        if (level <= 1) return 0;
        return Mathf.RoundToInt(baseExpRequired * Mathf.Pow(expMultiplier, level - 2));
    }

    public void SavePlayerData()
    {
        ES3.Save(saveKey, playerData);
        Debug.Log("プレイヤーデータを保存しました。");
    }

    public void LoadPlayerData()
    {
        if (ES3.KeyExists(saveKey))
        {
            playerData = ES3.Load<PlayerData>(saveKey);
            Debug.Log("プレイヤーデータを読み込みました。");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("新しいプレイヤーデータを作成しました。");
        }
    }

    public void ResetPlayerData()
    {
        playerData = new PlayerData();
        SavePlayerData();
        Debug.Log("プレイヤーデータをリセットしました。");
    }
}
