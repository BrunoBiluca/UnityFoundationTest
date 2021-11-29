using Assets.UnityFoundation.Code.TimeUtils;
using Assets.UnityFoundation.Systems.QuestSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestFinishedPopup : MonoBehaviour
{
    private Timer popupDisplayTimer;

    void Start()
    {
        popupDisplayTimer = new Timer(4f, () => gameObject.SetActive(false));

        var text = GetComponent<TextMeshProUGUI>();

        QuestManager.Instance.QuestList.OnQuestFinished
            += (QuestStatus status) => {
                gameObject.SetActive(true);
                text.text = $"Quest {status.Quest.Title} finished!";

                popupDisplayTimer.Start();
            };

        gameObject.SetActive(false);
    }
}
