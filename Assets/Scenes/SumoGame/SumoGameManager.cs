using Assets.UnityFoundation.GameManagers;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SumoGameManager : BaseGameManager<SumoGameManager>
{
    [SerializeField] private Transform startPanel;

    private const string PLAYER_SAVE_FILE = "player_score.dat";

    public long Score { get; private set; }
    public long HighScore { get; private set; }
    public string User { get; private set; }

    protected override void OnAwake()
    {
        Time.timeScale = 0f;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += delegate {
            startPanel = GameObject.Find("start_panel").transform;
            SetupScores(); 
        };

        startPanel
            .Find("user_input")
            .GetComponent<TMP_InputField>()
            .onValueChanged
            .AddListener(userName => User = userName);

        startPanel
            .Find("start_button")
            .GetComponent<Button>()
            .onClick
            .AddListener(() => {
                PlayerPrefs.SetString("user_name", User);
                GameSaver.Instance.Save(PLAYER_SAVE_FILE, 0L);
                SetupScores();
            });
    }

    private void SetupScores()
    {
        startPanel.gameObject.SetActive(false);

        Score = 0;
        User = PlayerPrefs.GetString("user_name");
        HighScore = GameSaver.Instance.Load<long>(PLAYER_SAVE_FILE);

        LeaderBoardRequest.Instance.GetUserPersonalRecord(
            User,
            (personalRecord) => {
                if(personalRecord == null) return;

                GameSaver.Instance.Save(PLAYER_SAVE_FILE, personalRecord.Score);

                HighScore = personalRecord.Score;
                SumoGameScoreUI.Instance.UpdateHighScore(HighScore);
            },
            (error) => { }
        );

        LeaderBoardRequest.Instance.GetHighestScores(
            1,
            (personalRecord) => {
                if(personalRecord.Count == 0) { 
                    SumoGameScoreUI.Instance.UpdateHighestScore(User, Score);
                    return;
                }

                SumoGameScoreUI.Instance.UpdateHighestScore(
                    personalRecord.First().User,
                    personalRecord.First().Score
                );
            },
            (error) => { }
        );

        SumoGameScoreUI.Instance.UpdateHighScore(HighScore);
        SumoGameScoreUI.Instance.UpdateScore(Score);
        SumoGameScoreUI.Instance.UpdateUser(User);

        Time.timeScale = 1f;
        SumoEnemySpawner.Instance.InvokeEnemies();
        PowerupSpawner.Instance.InvokePowerups();
    }

    public void EnemyFell()
    {
        SumoGameScoreUI.Instance.UpdateScore(Score++);
    }

    public void PlayerFell()
    {
        LeaderBoardRequest.Instance.AddScore(
            NewLeaderBoardScore.Builder()
                .WithScore(Score)
                .WithUser(User),
            (score) => {
                Debug.Log($"Created new score {score.Id} with {score.Score}");
            },
            (error) => { }
        );

        if(Score > HighScore)
        {
            GameSaver.Instance.Save(PLAYER_SAVE_FILE, Score);
            HighScore = Score;
        }            

        SumoGameOverUI.Instance.Show(Score, HighScore);
    }
}
