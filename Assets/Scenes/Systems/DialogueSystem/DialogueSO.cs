using Assets.UnityFoundation.EditorInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogueDictionary : SerializableDictionary<string, DialogueNode> { }

[CreateAssetMenu(fileName = "New dialogue", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private DialogueDictionary dialogueNodes;

    public IEnumerable<DialogueNode> DialogueNodes => dialogueNodes.Values;

#if UNITY_EDITOR
    private void Awake()
    {
        if(dialogueNodes != null) return;

        var id = Guid.NewGuid().ToString();
        dialogueNodes = new DialogueDictionary {
            {
                id, 
                new DialogueNode(id) 
            }
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

    public DialogueNode CreateNode(DialogueNode parent)
    {
        var id = Guid.NewGuid().ToString();
        var newDialogueNode = new DialogueNode(id);

        dialogueNodes.Add(id, newDialogueNode);

        if(parent != null)
        {
            var birthRect = new Rect(
                parent.rect.center.x,
                parent.rect.yMax + 10f,
                200,
                100
            );

            newDialogueNode.rect = birthRect;
            newDialogueNode.previousDialogueNodes.Add(parent.id);

            parent.nextDialogueNodes.Add(id);
        }

        return newDialogueNode;
    }

    public void LinkNodes(DialogueNode parent, DialogueNode child)
    {
        parent.nextDialogueNodes.Add(child.id);
        child.previousDialogueNodes.Add(parent.id);
    }

    public void UnlinkNodes(DialogueNode parent, DialogueNode child)
    {
        parent.nextDialogueNodes.Remove(child.id);
        child.previousDialogueNodes.Remove(parent.id);
    }

    public void RemoveNode(DialogueNode node)
    {
        dialogueNodes.Remove(node.id);

        node.nextDialogueNodes.ForEach(nodeId => {
            if(dialogueNodes.TryGetValue(nodeId, out DialogueNode next))
            {
                next.previousDialogueNodes.Remove(node.id);
            }
        }); 

        node.previousDialogueNodes.ForEach(nodeId => {
            if(dialogueNodes.TryGetValue(nodeId, out DialogueNode previous))
            {
                previous.nextDialogueNodes.Remove(node.id);
            }
        }); 
    }

    public Vector2 GetViewSize()
    {
        return new Vector2(
            dialogueNodes.Max(node => node.Value.rect.x),
            dialogueNodes.Max(node => node.Value.rect.y)
        );
    }
}
