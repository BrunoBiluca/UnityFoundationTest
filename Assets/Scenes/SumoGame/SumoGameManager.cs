using Assets.UnityFoundation.GameManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoGameManager : BaseGameManager<SumoGameManager>
{
    public int Score { get; private set; }

    private void Start()
    {
        SumoGameScoreUI.Instance.UpdateScore(Score);
    }

    public void EnemyFell()
    {
        SumoGameScoreUI.Instance.UpdateScore(Score++);
    }

    public void PlayerFell()
    {
        SumoGameOverUI.Instance.Show(Score);
    }
}
