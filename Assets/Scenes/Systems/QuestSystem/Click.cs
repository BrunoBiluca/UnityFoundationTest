using Assets.UnityFoundation.Systems.QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(
            () => GetComponent<QuestObjectiveHandler>().UpdateObjetiveProgress(1)
        );
    }
}
