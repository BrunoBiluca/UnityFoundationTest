using Assets.UnityFoundation.EditorInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueDictionary : SerializableDictionary<string, DialogueNode> { }

[CreateAssetMenu(fileName = "New dialogue", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    [ReadOnlyInspector]
    public string asdfsd = "read only";

    [SerializeField] private DialogueDictionary dialogueNodes;

    public IEnumerable<DialogueNode> DialogueNodes => dialogueNodes.Values;

#if UNITY_EDITOR
    private void Awake()
    {
        if(dialogueNodes != null) return;

        dialogueNodes = new DialogueDictionary {
            { "a", new DialogueNode("a") }
        };
    }
#endif

    public IEnumerable<DialogueNode> GetChildrenNodes(DialogueNode node)
    {
        if(node.nextDialogueNodes == null) yield return null;

        foreach(var childId in node.nextDialogueNodes)
        {
            if(dialogueNodes.TryGetValue(childId, out DialogueNode childNode))
            {
                yield return childNode;
            }
        }
    }
}
