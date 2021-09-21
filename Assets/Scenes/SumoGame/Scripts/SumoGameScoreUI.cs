using Assets.UnityFoundation.Code.Common;
using TMPro;

public class SumoGameScoreUI : Singleton<SumoGameScoreUI>
{
    private TextMeshProUGUI playerNameText;
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

        playerNameText = transform.Find("player_text").GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    public void UpdateUser(string user)
    {
        playerNameText.text = $"Playing: {user}";
    } 

    public void UpdateHighScore(long highScore)
    {
        recordText.text = highScore.ToString();
    }

    public void UpdateScore(long score)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString();
    }

    public void UpdateHighestScore(string name, long score) {
        highestScoreUserText.text = name;
        highestScoreCounterText.text = score.ToString();
    }
}
