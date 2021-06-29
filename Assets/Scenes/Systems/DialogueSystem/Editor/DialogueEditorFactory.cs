using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEditorFactory
{
    private readonly DialogueEditor editor;

    public DialogueEditorFactory(DialogueEditor editor)
    {
        this.editor = editor;
    }

    public void Button(string text, IDialogueEditorAction action)
    {
        if(GUILayout.Button(text))
        {
            editor.Actions.Add(action);
        }
    }

    public void Button(string text, Action callback)
    {
        if(GUILayout.Button(text))
        {
            callback();
        }
    }
}
