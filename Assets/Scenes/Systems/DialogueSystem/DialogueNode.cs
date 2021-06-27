using Assets.UnityFoundation.EditorInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string id;
    public string text;
    public List<string> nextDialogueNodes = new List<string>();
    public List<string> previousDialogueNodes = new List<string>();
    public Rect rect = new Rect(100, 100, 200, 100);

    public DialogueNode(string id)
    {
        this.id = id;
        text = "example";
    }
}
