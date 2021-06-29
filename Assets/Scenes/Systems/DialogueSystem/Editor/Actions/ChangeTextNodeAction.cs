using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeTextNodeAction : IDialogueEditorAction
{
    public static ChangeTextNodeAction create(DialogueNode node, string newText)
    {
        return new ChangeTextNodeAction(node, newText);
    }

    private readonly DialogueNode node;
    private readonly string newText;
    private DialogueEditor editor;

    public ChangeTextNodeAction(DialogueNode node, string newText)
    {
        this.node = node;
        this.newText = newText;
    }
    public void SetDialogueEditor(DialogueEditor editor)
    {
        this.editor = editor;
    }

    public void Handle()
    {
        Undo.RecordObject(node, "Update dialogue node");
        node.Text = newText;
        EditorUtility.SetDirty(node);

        editor.LinkingNodes.Clear();
    }
}
