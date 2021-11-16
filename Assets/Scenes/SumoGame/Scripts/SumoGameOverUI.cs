using Assets.UnityFoundation.Code.Common;
using Assets.UnityFoundation.Systems.LeaderBoardSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SumoGameOverUI : Singleton<SumoGameOverUI>
{
    private TextMeshProUGUI scoreText;

    private TextMeshProUGUI personalRecordText;

    private TextMeshProUGUI highestScoreUserNameText;
    private TextMeshProUGUI highestScoreText;

    protected override void OnAwake()
    {
        transform.Find("retry_button")
            .GetComponent<Button>()
            .onClick
            .AddListener(() => {
                SceneManager.LoadScene("sumo_gameplay_scene");
            });

        scoreText = transform.Find("score_counter_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();

        personalRecordText = transform.Find("personal_record_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();

        highestScoreUserNameText = transform.Find("highest_score_counter_panel")
            .Find("user_text")
            .GetComponent<TextMeshProUGUI>();

        highestScoreText = transform.Find("highest_score_counter_panel")
            .Find("counter")
            .GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    public void Show(long score, long personalRecordScore)
    {
        gameObject.SetActive(true);

        scoreText.text = score.ToString();

        personalRecordText.text = personalRecordScore.ToString();

        LeaderBoardRequest.Instance.GetHighestScores(
            1,
            (scores) => {
                if(scores.Count == 0) return;

                var score = scores[0];
                highestScoreUserNameText.text = $"From {score.User}";
                highestScoreText.text = score.Score.ToString();
            },
            (error) => { }
        );
    }
}
