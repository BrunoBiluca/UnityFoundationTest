using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string id;
    public string text;
    public string[] nextDialogueNodes;
    public Rect rectPosition = new Rect(100, 100, 200, 100);

}
