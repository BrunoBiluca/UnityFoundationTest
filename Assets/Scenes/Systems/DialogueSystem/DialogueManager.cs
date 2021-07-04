using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSO currentDialogue;
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueNode currentDialogueNode;

    public void Start()
    {
        currentDialogueNode = currentDialogue.StartDialogueNode;
        dialogueUI.Display(currentDialogueNode);
        dialogueUI.OnNextLine += NextDialogueNode;
    }

    private void NextDialogueNode(object sender, EventArgs e)
    {
        var nextNodes = currentDialogue
            .GetNextDialogueNodes(currentDialogueNode)
            .ToArray();

        if(nextNodes.Length == 0)
            return;

        currentDialogueNode = nextNodes[0];
        dialogueUI.Display(currentDialogueNode);
    }
}
