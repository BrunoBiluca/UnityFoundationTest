using UnityEditor;

public class UnlinkDialogueNodes : IDialogueEditorAction
{
    public static UnlinkDialogueNodes Create(DialogueSO dialogue, DialogueNode node)
    {
        return new UnlinkDialogueNodes(dialogue, node);
    }

    private readonly DialogueSO dialogue;
    private readonly DialogueNode parent;
    private DialogueEditor editor;

    public UnlinkDialogueNodes(DialogueSO dialogue, DialogueNode node)
    {
        this.dialogue = dialogue;
        parent = node;
    }
    public void SetDialogueEditor(DialogueEditor editor)
    {
        this.editor = editor;
    }

    public void Handle()
    {
        var child = editor.LinkingNodes.LinkingNode.Get();
        Undo.RecordObject(dialogue, "Unlink dialogue node");
        dialogue.UnlinkNodes(parent, child);
        EditorUtility.SetDirty(parent);
        EditorUtility.SetDirty(child);

        editor.LinkingNodes.Clear();
    }

}
