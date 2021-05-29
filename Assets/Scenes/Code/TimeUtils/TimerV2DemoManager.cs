using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerV2DemoManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loopCounterText;
    [SerializeField] private TextMeshProUGUI loopCompletionText;
    private int loopCounter;
    private TimerV2 loopTimer;

    [SerializeField] private TextMeshProUGUI runCounterText;
    [SerializeField] private TextMeshProUGUI runOnceCompletionText;
    [SerializeField] private Button runOnceResetButton;
    private int runOnceCounter;
    private TimerV2 runOnceTimer;

    void Start()
    {
        RunLoopTimer();

        RunOnceTimer();
    }

    private void RunLoopTimer()
    {
        loopTimer = new TimerV2(5f, () => {
            loopCounter++;
            loopCounterText.text = $"Count: {loopCounter}";
        })
            .SetName("Loop counter timer")
            .Loop()
            .Start();
    }

    private void RunOnceTimer()
    {
        runOnceTimer = new TimerV2(5f, () => {
            runOnceCounter++;
            runCounterText.text = $"Count: {runOnceCounter}";
        })
            .SetName("Loop run once timer")
            .Start();

        runOnceResetButton.onClick.AddListener(() => {
            runOnceTimer.Start();
        });
    }

    private void Update()
    {
        loopCompletionText.text = loopTimer.Completion.ToString("00") + "%";
        runOnceCompletionText.text = runOnceTimer.Completion.ToString("00") + "%";
    }
}
