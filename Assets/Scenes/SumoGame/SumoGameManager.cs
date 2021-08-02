using Assets.UnityFoundation.GameManagers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SumoGameManager : BaseGameManager<SumoGameManager>
{
    private const string PLAYER_SAVE_FILE = "player_score.dat";

    public int Score { get; private set; }
    public int HighScore { get; private set; }

    public struct HighestScore
    {
        public string User { get; set; }
        public int Score { get; set; }
    }

    private void Start()
    {
        SetupScores();
        SceneManager.sceneLoaded += delegate { SetupScores(); };
    }

    private void SetupScores()
    {
        Score = 0;
        HighScore = GameSaver.Instance.Load<int>(PLAYER_SAVE_FILE);

        SumoGameScoreUI.Instance.UpdateHighScore(HighScore);
        SumoGameScoreUI.Instance.UpdateScore(Score);
    }

    public void EnemyFell()
    {
        SumoGameScoreUI.Instance.UpdateScore(Score++);
    }

    public void PlayerFell()
    {
        if(Score > HighScore)
            GameSaver.Instance.Save(PLAYER_SAVE_FILE, Score);

        SumoGameOverUI.Instance.Show(Score);
    }
}
