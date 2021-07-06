using Assets.UnityFoundation.EditorInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DialogueDictionary : SerializableDictionary<string, DialogueNode> { }

[CreateAssetMenu(fileName = "New dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private DialogueDictionary dialogueNodes;
    [SerializeField] private string startDialogueNodeName;

    public IEnumerable<DialogueNode> DialogueNodes => dialogueNodes.Values;

    public DialogueNode StartDialogueNode => dialogueNodes[startDialogueNodeName];

#if UNITY_EDITOR
    private void Awake()
    {
        if(dialogueNodes != null) return;
        dialogueNodes = new DialogueDictionary();
        var dialogueNode = CreateNode(CreateInstance<DialogueNode>().Setup(), null);
        startDialogueNodeName = dialogueNode.name;
    }
#endif

    public Optional<DialogueNode> Get(string dialogueNodeName)
    {
        if(dialogueNodes.TryGetValue(dialogueNodeName, out DialogueNode dialogueNode))
        {
            return Optional<DialogueNode>.Some(dialogueNode);
        }

        return Optional<DialogueNode>.None();
    }

    public IEnumerable<DialogueNode> GetNextDialogueNodes(DialogueNode node)
    {
        if(node.NextDialogueNodes == null) yield return null;

        foreach(var childId in node.NextDialogueNodes)
        {
            if(dialogueNodes.TryGetValue(childId, out DialogueNode childNode))
            {
                yield return childNode;
            }
        }
    }

    public IEnumerable<DialogueNode> GetPreviousDialogueNodes(DialogueNode node)
    {
        if(node.PreviousDialogueNodes == null) yield return null;

        foreach(var nodeId in node.PreviousDialogueNodes)
        {
            if(dialogueNodes.TryGetValue(nodeId, out DialogueNode dialogueNode))
            {
                yield return dialogueNode;
            }
        }
    }

    public DialogueNode CreateNode(DialogueNode newDialogueNode, DialogueNode parent)
    {
        dialogueNodes.Add(newDialogueNode.name, newDialogueNode);

        if(parent != null)
        {
            newDialogueNode.Position = new Vector2(
                parent.Rect.center.x,
                parent.Rect.yMax + 10f
            );
            newDialogueNode.PreviousDialogueNodes.Add(parent.name);

            parent.NextDialogueNodes.Add(newDialogueNode.name);
        }

        return newDialogueNode;
    }

    public void LinkNodes(DialogueNode parent, DialogueNode child)
    {
        parent.NextDialogueNodes.Add(child.name);
        child.PreviousDialogueNodes.Add(parent.name);
    }

    public void UnlinkNodes(DialogueNode parent, DialogueNode child)
    {
        parent.NextDialogueNodes.Remove(child.name);
        child.PreviousDialogueNodes.Remove(parent.name);
    }

    public void RemoveNode(DialogueNode node)
    {
        dialogueNodes.Remove(node.name);

        node.NextDialogueNodes.ForEach(nodeId => {
            if(dialogueNodes.TryGetValue(nodeId, out DialogueNode next))
            {
                next.PreviousDialogueNodes.Remove(node.name);
            }
        }); 

        node.PreviousDialogueNodes.ForEach(nodeId => {
            if(dialogueNodes.TryGetValue(nodeId, out DialogueNode previous))
            {
                previous.NextDialogueNodes.Remove(node.name);
            }
        });
    }

    public bool IsStartLine(DialogueNode node)
    {
        return node.name == startDialogueNodeName;
    }

    public Vector2 GetViewSize()
    {
        if(dialogueNodes.Count == 0) return Vector2.zero;

        return new Vector2(
            dialogueNodes.Max(node => node.Value.Rect.x),
            dialogueNodes.Max(node => node.Value.Rect.y)
        );
    }

    public void OnBeforeSerialize()
    {
        if(string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
            return;

        foreach(var node in dialogueNodes)
        {
            if(!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(node.Value)))
                continue;

            AssetDatabase.AddObjectToAsset(node.Value, this);
        }
    }

    public void OnAfterDeserialize()
    {
    }
}
