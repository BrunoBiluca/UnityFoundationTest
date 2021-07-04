using System.Collections.Generic;
using System.Linq;
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
        var objectsToUndo = new List<UnityEngine.Object> { dialogue, node };
        
        objectsToUndo.AddRange(dialogue.GetNextDialogueNodes(node));
        objectsToUndo.AddRange(dialogue.GetPreviousDialogueNodes(node));
                
        Undo.RecordObjects(objectsToUndo.ToArray(), "Remove dialogue node");

        if(editor.LinkingNodes.LinkingNode.Get() == node)
        {
            editor.LinkingNodes.Clear();
        }
        dialogue.RemoveNode(node);

        Undo.DestroyObjectImmediate(node);
    }
}
