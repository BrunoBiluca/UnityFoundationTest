using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SumoGameScoreUI : Singleton<SumoGameScoreUI>
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI recordText;

    private TextMeshProUGUI highestScoreUserText;
    private TextMeshProUGUI highestScoreCounterText;

    protected override void OnAwake()
    {
        scoreText = transform.Find("score_counter_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();

        recordText = transform.Find("personal_record_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();

        highestScoreUserText = transform.Find("highest_score_counter_panel")
            .Find("user_text")
            .GetComponent<TextMeshProUGUI>();

        highestScoreCounterText = transform.Find("highest_score_counter_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
