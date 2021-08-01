using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SumoGameOverUI : Singleton<SumoGameOverUI>
{
    private TextMeshProUGUI scoreText;

    protected override void OnAwake()
    {
        transform.Find("retry_button")
            .GetComponent<Button>()
            .onClick
            .AddListener(() => {
            SceneManager.LoadScene("Prototype 4");
        });

        scoreText = transform.Find("score_counter_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    public void Show(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString();
    }
}
