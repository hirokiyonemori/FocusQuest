using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float focusTime = 25f * 60f; // 25分を秒で表現
    [SerializeField] private float currentTime;

    [Header("Events")]
    public UnityEvent OnTimerComplete;
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerPause;
    public UnityEvent OnTimerReset;

    [Header("Timer State")]
    public bool isRunning = false;
    public bool isPaused = false;

    private void Start()
    {
        currentTime = focusTime;
    }

    private void Update()
    {
        if (isRunning && !isPaused)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                OnTimerComplete?.Invoke();
            }
        }
    }

    public void StartTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
            isPaused = false;
            OnTimerStart?.Invoke();
        }
        else if (isPaused)
        {
            isPaused = false;
        }
    }

    public void PauseTimer()
    {
        if (isRunning && !isPaused)
        {
            isPaused = true;
            OnTimerPause?.Invoke();
        }
    }

    public void ResetTimer()
    {
        isRunning = false;
        isPaused = false;
        currentTime = focusTime;
        OnTimerReset?.Invoke();
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetProgress()
    {
        return 1f - (currentTime / focusTime);
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
