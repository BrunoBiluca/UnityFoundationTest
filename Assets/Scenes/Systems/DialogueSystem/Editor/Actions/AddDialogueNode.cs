using UnityEditor;
using UnityEngine;

public class AddDialogueNode : IDialogueEditorAction
{
    private readonly DialogueSO dialogue;
    private readonly DialogueNode parent;
    private Optional<Vector2> position = Optional<Vector2>.None();
    private DialogueEditor editor;

    public AddDialogueNode(DialogueSO dialogue, DialogueNode parent)
    {
        this.dialogue = dialogue;
        this.parent = parent;
    }

    public AddDialogueNode SetCreatePosition(Vector2 position)
    {
        this.position = Optional<Vector2>.Some(position);
        return this;
    }

    public void SetDialogueEditor(DialogueEditor editor)
    {
        this.editor = editor; 
    }

    public void Handle()
    {
        Undo.RecordObject(dialogue, "Add Dialogue Node");

        var newDialogueNode = ScriptableObject.CreateInstance<DialogueNode>().Setup();
        Undo.RegisterCreatedObjectUndo(newDialogueNode, "Create dialogue node");

        dialogue.CreateNode(newDialogueNode, parent);

        position.Some(pos => newDialogueNode.Position = pos);
    }
}
