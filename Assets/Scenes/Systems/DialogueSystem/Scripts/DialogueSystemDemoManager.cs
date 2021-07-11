using Assets.UnityFoundation.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemDemoManager : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private DialogueSO dialogue;

    void Start()
    {
        playAgainButton.onClick.AddListener(() => {
            DialogueManager.Instance.Setup(dialogue);
        });

        DialogueManager.Instance.Setup(dialogue);
    }
}
