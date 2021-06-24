using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private List<DialogueNode> dialogueNodes;

    public List<DialogueNode> DialogueNodes => dialogueNodes;

    private void Awake()
    {
        if(dialogueNodes != null) return;

        dialogueNodes = new List<DialogueNode> {
            new DialogueNode()
        };
    }
}
