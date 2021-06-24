using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if(EditorUtility.InstanceIDToObject(instanceID) is DialogueSO)
        {
            ShowEditorWindow();
            return true;
        }

        return false;
    }

    private DialogueSO selectedDialogue;
    private GUIStyle nodeStyle;
    private Vector2 draggingOffset;
    private Optional<DialogueNode> draggingNode = Optional<DialogueNode>.None();

    private void OnEnable()
    {
        Selection.selectionChanged += () => {
            selectedDialogue = Selection.activeObject as DialogueSO;
            Repaint();
        };

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnGUI()
    {
        if(selectedDialogue == null)
        {
            EditorGUILayout.LabelField("No Dialogue selected.");
            return;
        }

        ProcessEvents();
        foreach(var node in selectedDialogue.DialogueNodes)
        {
            RenderDialogueNode(node);
        }
    }

    private void ProcessEvents()
    {
        if(Event.current.type == EventType.MouseDown)
        {
            var mousePosition = Event.current.mousePosition;
            draggingNode = GetDraggingNode(mousePosition);
            draggingNode
                .Some(dialogueNode =>
                    draggingOffset = dialogueNode.rectPosition.position - mousePosition
                );
        }

        if(Event.current.type == EventType.MouseUp)
        {
            draggingNode = Optional<DialogueNode>.None();
        }

        draggingNode.Some(dialogueNode => {
            if(Event.current.type != EventType.MouseDrag)
                return;

            Undo.RecordObject(selectedDialogue, "Move dialogue node");
            dialogueNode.rectPosition.position = Event.current.mousePosition + draggingOffset;
            GUI.changed = true;
        });
    }

    private Optional<DialogueNode> GetDraggingNode(Vector2 mousePosition)
    {
        var node = selectedDialogue.DialogueNodes
            .LastOrDefault(node => node.rectPosition.Contains(mousePosition));

        if(node == null) return Optional<DialogueNode>.None();

        return Optional<DialogueNode>.Some(node);
    }

    private void RenderDialogueNode(DialogueNode node)
    {
        GUILayout.BeginArea(node.rectPosition, nodeStyle);

        EditorGUILayout.LabelField($"Node: {node.id}", EditorStyles.whiteBoldLabel);

        EditorGUI.BeginChangeCheck();

        var newText = EditorGUILayout.TextField(node.text);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(selectedDialogue, "Update dialogue node");
            node.text = newText;
        }

        GUILayout.EndArea();
    }
}
