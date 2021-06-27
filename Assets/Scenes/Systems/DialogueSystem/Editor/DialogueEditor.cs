using Assets.UnityFoundation.Code;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


public class AddDialogueNodeAction : IDialogueEditorAction
{
    private readonly DialogueSO dialogue;
    private readonly DialogueNode parent;
    private Optional<Vector2> position = Optional<Vector2>.None();

    public AddDialogueNodeAction(DialogueSO dialogue, DialogueNode parent)
    {
        this.dialogue = dialogue;
        this.parent = parent;
    }

    public AddDialogueNodeAction SetCreatePosition(Vector2 position)
    {
        this.position = Optional<Vector2>.Some(position);
        return this;
    }

    public void Handle()
    {
        Undo.RecordObject(dialogue, "Add dialogue node");
        var newDialogueNode = dialogue.CreateNode(parent);

        position.Some(pos => newDialogueNode.rect.position = pos);
    }
}
public class RemoveDialogueNodeAction : IDialogueEditorAction
{
    private readonly DialogueSO dialogue;
    private readonly DialogueNode node;

    public RemoveDialogueNodeAction(DialogueSO dialogue, DialogueNode node)
    {
        this.dialogue = dialogue;
        this.node = node;
    }

    public void Handle()
    {
        Undo.RecordObject(dialogue, "Remove dialogue node");
        dialogue.RemoveNode(node);
    }
}

public class LinkDialogueNodesAction : IDialogueEditorAction
{
    private readonly DialogueSO dialogue;
    private readonly DialogueNode parent;
    private readonly DialogueNode child;

    public LinkDialogueNodesAction(DialogueSO dialogue, DialogueNode parent, DialogueNode child)
    {
        this.dialogue = dialogue;
        this.parent = parent;
        this.child = child;
    }

    public void Handle()
    {
        Undo.RecordObject(dialogue, "Link dialogue node");
        dialogue.LinkNodes(parent, child);
    }
}

public class UnlinkDialogueNodesAction : IDialogueEditorAction
{
    private readonly DialogueSO dialogue;
    private readonly DialogueNode parent;
    private readonly DialogueNode child;

    public UnlinkDialogueNodesAction(DialogueSO dialogue, DialogueNode parent, DialogueNode child)
    {
        this.dialogue = dialogue;
        this.parent = parent;
        this.child = child;
    }

    public void Handle()
    {
        Undo.RecordObject(dialogue, "Unlink dialogue node");
        dialogue.UnlinkNodes(parent, child);
    }
}

public interface IDialogueEditorAction
{
    public void Handle();
}

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
    private Optional<DialogueNode> linkingNode = Optional<DialogueNode>.None();
    private Vector2 scrollviewPosition;
    private Vector2 lastMousePosition;
    private Texture2D backgroundTex;
    private readonly List<IDialogueEditorAction> editorActions = new List<IDialogueEditorAction>();

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

        backgroundTex = Resources.Load<Texture2D>("background");
    }

    private void OnGUI()
    {
        if(selectedDialogue == null)
        {
            EditorGUILayout.LabelField("No Dialogue selected.");
            return;
        }

        ProcessEvents();

        scrollviewPosition = GUILayout.BeginScrollView(scrollviewPosition);
        Vector2 scrollviewSize = selectedDialogue.GetViewSize();
        var canvas = GUILayoutUtility.GetRect(scrollviewSize.x + 400, scrollviewSize.y + 400);

        GUI.DrawTextureWithTexCoords(
            canvas,
            backgroundTex, 
            new Rect(
                0, 
                MathX.Remainder(canvas.height / backgroundTex.height), 
                canvas.width / backgroundTex.width, 
                canvas.height / backgroundTex.height
            )
        );

        foreach(var node in selectedDialogue.DialogueNodes)
        {
            RenderConnections(node);
            RenderDialogueNode(node);
        }
        GUILayout.EndScrollView();
        ProcessActions();
    }

    private void ProcessEvents()
    {
        if(Event.current.type == EventType.MouseDown)
        {
            var mousePosition = Event.current.mousePosition + scrollviewPosition;
            draggingNode = GetDraggingNode(mousePosition);
            draggingNode.Some(dialogueNode =>
                draggingOffset = dialogueNode.rect.position - mousePosition
            );

            lastMousePosition = Event.current.mousePosition;
        }

        if(Event.current.type == EventType.MouseUp)
        {
            draggingNode = Optional<DialogueNode>.None();
            return;
        }

        if(draggingNode.IsPresent)
        {
            draggingNode.Some(dialogueNode => {
                if(Event.current.type != EventType.MouseDrag)
                    return;

                Undo.RecordObject(selectedDialogue, "Move dialogue node");
                var mousePosition = Event.current.mousePosition + scrollviewPosition;
                dialogueNode.rect.position = mousePosition + draggingOffset;
                GUI.changed = true;
            });
            return;
        }

        if(Event.current.type == EventType.MouseDrag)
        {
            scrollviewPosition += lastMousePosition - Event.current.mousePosition;
            lastMousePosition = Event.current.mousePosition;
            GUI.changed = true;
        }

        if(Event.current.type == EventType.ContextClick)
        {
            editorActions.Add(
                new AddDialogueNodeAction(selectedDialogue, null)
                    .SetCreatePosition(Event.current.mousePosition)
            );
        }
    }

    private Optional<DialogueNode> GetDraggingNode(Vector2 mousePosition)
    {
        var node = selectedDialogue.DialogueNodes
            .LastOrDefault(node => node.rect.Contains(mousePosition));

        if(node == null) return Optional<DialogueNode>.None();

        return Optional<DialogueNode>.Some(node);
    }

    private void RenderDialogueNode(DialogueNode node)
    {
        GUILayout.BeginArea(node.rect, nodeStyle);

        EditorGUI.BeginChangeCheck();

        var newText = EditorGUILayout.TextArea(node.text);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(selectedDialogue, "Update dialogue node");
            node.text = newText;
        }

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("x"))
            editorActions.Add(new RemoveDialogueNodeAction(selectedDialogue, node));

        RenderLinkingButtons(node);

        if(GUILayout.Button("+"))
            editorActions.Add(new AddDialogueNodeAction(selectedDialogue, node));

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void RenderLinkingButtons(DialogueNode node)
    {
        if(!linkingNode.IsPresent)
        {
            if(GUILayout.Button("link"))
            {
                linkingNode = Optional<DialogueNode>.Some(node);
            }
            return;
        }


        if(node.id == linkingNode.Get().id)
        {
            if(GUILayout.Button("cancel"))
            {
                linkingNode = Optional<DialogueNode>.None();
            }
            return;
        }

        if(
            !linkingNode.Get().nextDialogueNodes.Contains(node.id)
            && !linkingNode.Get().previousDialogueNodes.Contains(node.id)
        )
        {
            if(GUILayout.Button("link"))
            {
                editorActions.Add(
                    new LinkDialogueNodesAction(selectedDialogue, node, linkingNode.Get())
                );
                linkingNode = Optional<DialogueNode>.None();
            }
        }
        else
        {
            if(GUILayout.Button("unlink"))
            {
                editorActions.Add(
                    new UnlinkDialogueNodesAction(selectedDialogue, node, linkingNode.Get())
                );
                linkingNode = Optional<DialogueNode>.None();
            }
        }
    }

    private void RenderConnections(DialogueNode node)
    {
        foreach(var childNode in selectedDialogue.GetChildrenNodes(node))
        {
            if(node.rect.yMax < childNode.rect.yMin)
                RenderConnectionLineBottom(node, childNode);
            else
                RenderConnectionLineRight(node, childNode);
        }
    }

    private void RenderConnectionLineBottom(DialogueNode node, DialogueNode childNode)
    {
        var parentPosition = new Vector2(
            node.rect.center.x, node.rect.yMax
        );
        var childPosition = new Vector2(
            childNode.rect.center.x, childNode.rect.yMin
        );
        var offset = new Vector2(0f, (childPosition.y - parentPosition.y) * .8f);

        Handles.DrawBezier(
            parentPosition,
            childPosition,
            parentPosition + offset,
            childPosition - offset,
            Color.white,
            null,
            4f
        );

        Handles.DrawAAConvexPolygon(
            childPosition + new Vector2(-5f, -2f),
            childPosition + new Vector2(5f, -2f),
            childPosition + new Vector2(0f, 3f)
        );
    }

    private static void RenderConnectionLineRight(DialogueNode node, DialogueNode childNode)
    {
        var parentPosition = new Vector2(
            node.rect.xMax, node.rect.center.y
        );
        var childPosition = new Vector2(
            childNode.rect.xMin, childNode.rect.center.y
        );
        var offset = new Vector2((childPosition.x - parentPosition.x) * .8f, 0f);

        Handles.DrawBezier(
            parentPosition,
            childPosition,
            parentPosition + offset,
            childPosition - offset,
            Color.white,
            null,
            4f
        );

        Handles.DrawAAConvexPolygon(
            childPosition + new Vector2(-2f, -5f),
            childPosition + new Vector2(-2f, 5f),
            childPosition + new Vector2(3f, 0f)
        );
    }

    private void ProcessActions()
    {
        if(editorActions.Count == 0) return;

        editorActions.ForEach(action => action.Handle());
        editorActions.Clear();
        GUI.changed = true;
    }
}
