using System;
using UnityEditor;

public class LinkDialogueNodes : IDialogueEditorAction
{
    public static LinkDialogueNodes Create(DialogueSO dialogue, DialogueNode node)
    {
        return new LinkDialogueNodes(dialogue, node);
    }

    private readonly DialogueSO dialogue;
    private readonly DialogueNode parent;
    private DialogueEditor editor;

    public LinkDialogueNodes(DialogueSO dialogue, DialogueNode node)
    {
        this.dialogue = dialogue;
        this.parent = node;
    }
    public void SetDialogueEditor(DialogueEditor editor)
    {
        this.editor = editor;
    }

    public void Handle()
    {
        var child = editor.LinkingNodes.LinkingNode.Get();

        Undo.RecordObjects(new UnityEngine.Object[] { child, parent }, "Link dialogue node");
        dialogue.LinkNodes(parent, child);
        EditorUtility.SetDirty(parent);
        EditorUtility.SetDirty(child);

        editor.LinkingNodes.Clear();
    }
}
