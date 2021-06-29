using UnityEditor;

public class RemoveDialogueNode : IDialogueEditorAction
{
    private readonly DialogueSO dialogue;
    private readonly DialogueNode node;
    private DialogueEditor editor;

    public RemoveDialogueNode(DialogueSO dialogue, DialogueNode node)
    {
        this.dialogue = dialogue;
        this.node = node;
    }
    public void SetDialogueEditor(DialogueEditor editor)
    {
        this.editor = editor;
    }

    public void Handle()
    {
        Undo.RecordObject(dialogue, "Remove dialogue node");
        dialogue.RemoveNode(node);
        Undo.DestroyObjectImmediate(node);
    }
}
