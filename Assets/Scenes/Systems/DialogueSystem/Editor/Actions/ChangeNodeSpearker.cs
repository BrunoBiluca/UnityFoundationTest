using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

class ChangeNodeSpearker : IDialogueEditorAction
{
    public static ChangeNodeSpearker create(DialogueNode node, SpearkerSO newSpearker)
    {
        return new ChangeNodeSpearker(node, newSpearker);
    }

    private readonly DialogueNode node;
    private readonly SpearkerSO newSpearker;
    private DialogueEditor editor;

    public ChangeNodeSpearker(DialogueNode node, SpearkerSO newSpearker)
    {
        this.node = node;
        this.newSpearker = newSpearker;
    }
    public void SetDialogueEditor(DialogueEditor editor)
    {
        this.editor = editor;
    }

    public void Handle()
    {
        Undo.RecordObject(node, "Update dialogue node spearker");
        node.Spearker = newSpearker;
        EditorUtility.SetDirty(node);
    }
}
