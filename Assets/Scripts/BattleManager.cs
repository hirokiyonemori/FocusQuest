using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Battle Settings")]
    [SerializeField] private float battleInterval = 3f; // バトルメッセージの更新間隔
    [SerializeField] private int expPerBattle = 10;
    [SerializeField] private int goldPerBattle = 5;

    [Header("UI References")]
    [SerializeField] private Text battleLogText;
    [SerializeField] private ScrollRect battleLogScrollRect;

    [Header("Battle Messages")]
    [SerializeField]
    private string[] attackMessages = {
        "勇者が攻撃！",
        "勇者が剣を振る！",
        "勇者が魔法を唱える！",
        "勇者が必殺技を発動！"
    };

    [SerializeField]
    private string[] victoryMessages = {
        "敵を倒した！",
        "勝利！",
        "敵を撃破！",
        "完璧な勝利！"
    };

    [SerializeField]
    private string[] levelUpMessages = {
        "レベルアップ！",
        "新しい力が目覚めた！",
        "パワーアップ！"
    };

    private bool isBattling = false;
    private Coroutine battleCoroutine;
    private List<string> battleLog = new List<string>();
    private int currentLevel;

    private void Start()
    {
        currentLevel = PlayerStats.Instance.Level;
    }

    public void StartBattle()
    {
        if (!isBattling)
        {
            isBattling = true;
            battleCoroutine = StartCoroutine(BattleLoop());
        }
    }

    public void StopBattle()
    {
        if (isBattling)
        {
            isBattling = false;
            if (battleCoroutine != null)
            {
                StopCoroutine(battleCoroutine);
            }
        }
    }

    private IEnumerator BattleLoop()
    {
        while (isBattling)
        {
            yield return new WaitForSeconds(battleInterval);

            if (isBattling)
            {
                PerformBattle();
            }
        }
    }

    private void PerformBattle()
    {
        // 攻撃メッセージ
        string attackMessage = attackMessages[Random.Range(0, attackMessages.Length)];
        AddBattleLog(attackMessage);

        // 勝利メッセージ
        string victoryMessage = victoryMessages[Random.Range(0, victoryMessages.Length)];
        AddBattleLog(victoryMessage);

        // 経験値とゴールドを追加
        PlayerStats.Instance.AddExperience(expPerBattle);
        PlayerStats.Instance.AddGold(goldPerBattle);

        // レベルアップチェック
        if (PlayerStats.Instance.Level > currentLevel)
        {
            string levelUpMessage = levelUpMessages[Random.Range(0, levelUpMessages.Length)];
            AddBattleLog($"<color=yellow>{levelUpMessage} レベル {PlayerStats.Instance.Level} になりました！</color>");
            currentLevel = PlayerStats.Instance.Level;
        }

        // 経験値とゴールドの獲得メッセージ
        AddBattleLog($"<color=green>経験値 +{expPerBattle}</color>");
        AddBattleLog($"<color=yellow>ゴールド +{goldPerBattle}</color>");
        AddBattleLog(""); // 空行で区切り
    }

    private void AddBattleLog(string message)
    {
        battleLog.Add(message);

        // ログが多くなりすぎないように制限
        if (battleLog.Count > 50)
        {
            battleLog.RemoveAt(0);
        }

        UpdateBattleLogDisplay();
    }

    private void UpdateBattleLogDisplay()
    {
        if (battleLogText != null)
        {
            battleLogText.text = string.Join("\n", battleLog);

            // スクロールを一番下に移動
            if (battleLogScrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                battleLogScrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }

    public void ClearBattleLog()
    {
        battleLog.Clear();
        UpdateBattleLogDisplay();
    }
}
