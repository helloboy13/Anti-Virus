using StarterAssets;
using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private TextMeshProUGUI fineTxt;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InteractionSystem inst;

    [SerializeField] public StarterAssetsInputs gamesInputs;

    private float timeRemaining;
    private bool timerRunning;

    public Action OnTimerFinished;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!timerRunning)
        {
            timerTxt.text = "";
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;

            UpdateUI();
            TimerCompleted();
            OnTimerFinished?.Invoke();
            return;
        }

        UpdateUI();
    }
    public void SetTimer(int minutes, int seconds)
    {
        timeRemaining = (minutes * 60) + seconds;
        timerRunning = false;
    }
    public void StartTimer()
    {
        timeRemaining += inst.time;
        timerRunning = true;
        UpdateUI();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        timerRunning = false;
        timeRemaining = 0;
        UpdateUI();
    }

    public bool IsRunning()
    {
        return timerRunning;
    }

    public float GetRemainingTime()
    {
        return timeRemaining;
    }
    private void TimerCompleted()
    {
        Debug.Log("Timer Complete");

        if (gameManager.inventory[GameManager.ItemType.InsuranceChip] > 0)
        {
            gameManager.inventory[GameManager.ItemType.InsuranceChip]--;
        }
        else
        {
            int fine = Mathf.RoundToInt(gameManager.reward * 0.2f);
            gameManager.money -= fine;
            fineTxt.text = "-" + fine + "$";
            CancelInvoke(nameof(ClearInstruction));
            Invoke(nameof(ClearInstruction), 3f);
        }
        Narrator.Instance.PlayTimeOutMessage();
        TeleportReset.Instance.TeleportToMin();
        ResetObjective.Instance.resetObjective();
        gamesInputs.staminaDrainRate = 30;
        gamesInputs.staminaRegenRate = 15;
    }
    public void Minus10Seconds()
    {
        if (!timerRunning)
            return;

        timeRemaining -= 10f;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;

            UpdateUI();
            TimerCompleted();
            OnTimerFinished?.Invoke();
            return;
        }

        UpdateUI();
    }
    private void ClearInstruction()
    {
        fineTxt.text = "";
    }
    private void UpdateUI()
    {
        if (timerTxt == null)
            return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerTxt.text = $"{minutes:00}:{seconds:00}";
    }
}