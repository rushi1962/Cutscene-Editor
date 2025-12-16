using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SequenceEditorWindow : EditorWindow
{
    private SequenceAsset currentSequence;

    private Vector2 offset;
    private Vector2 drag;
    private Vector2 mousePosition;

    private SequenceNode selectedNode;
    private SequenceNode startNode = null;

    private bool bHasMadeChanges = false;

    private List<Type> MarkedTypes = new List<Type>();

    public static void OpenSequence(SequenceAsset sequence)
    {
        var window = GetWindow<SequenceEditorWindow>("Sequence Editor");
        window.currentSequence = sequence;
    }

    private void OnEnable()
    {
        RefreshClassList();
    }

    private void RefreshClassList()
    {
        //Get all types in the whole code base
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes());

        //Find classes which have JSONConvertable attribute and are serializable
        MarkedTypes = allTypes
            .Where(t =>
                t.IsClass && t.IsSerializable &&
                t.GetCustomAttribute<SequenceNodeAttribute>() != null)
            .OrderBy(t => t.Name)
            .ToList();
    }


    private void OnGUI()
    {
        if (currentSequence == null)
        {
            EditorGUILayout.HelpBox("No SequenceAsset loaded.", MessageType.Info);
            return;
        }

        //Add '*' to the name if changes have been made to the file
        string label = bHasMadeChanges ? currentSequence.name + "*" : currentSequence.name;
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        //Sequence asset being worked on
        EditorGUILayout.ObjectField("Asset", currentSequence, typeof(SequenceAsset), false);

        mousePosition = Event.current.mousePosition;

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();

        HandleEvents();
    }

    #region Draw&Add
    // -------------------------
    // DRAW GRID LAYOUT
    // -------------------------
    private void DrawGrid(float spacing, float opacity, Color color)
    {
        int width = Mathf.CeilToInt(position.width / spacing);
        int height = Mathf.CeilToInt(position.height / spacing);

        Handles.BeginGUI();
        Handles.color = new Color(color.r, color.g, color.b, opacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % spacing, offset.y % spacing, 0);

        for (int i = 0; i < width; i++)
        {
            Handles.DrawLine(new Vector3(spacing * i, -spacing, 0) + newOffset,
                             new Vector3(spacing * i, position.height + spacing, 0) + newOffset);
        }

        for (int j = 0; j < height; j++)
        {
            Handles.DrawLine(new Vector3(-spacing, spacing * j, 0) + newOffset,
                             new Vector3(position.width + spacing, spacing * j, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    // -------------------------
    // DRAW EXISTING NODES
    // -------------------------
    private void DrawNodes()
    {
        if (currentSequence == null)
            return;

        bool bIsClickInRect = false;
        foreach (var node in currentSequence.GetAllNodes())
        {
            if (selectedNode == node)
            {
                Rect selectionRect = new Rect(node.position.x - 2.5f, node.position.y - 2.5f, 205f, node.height + 5.0f);
                EditorGUI.DrawRect(selectionRect, Color.aliceBlue);
            }

            Rect rect = new Rect(node.position.x, node.position.y, 200, node.height);

            GUILayout.BeginArea(rect, EditorStyles.helpBox);
            GUILayout.Label(node.displayName, EditorStyles.boldLabel);
            GUILayout.Space(20);
            if (node.OnGUI())
            {
                bHasMadeChanges = true;
            }
            GUILayout.EndArea();

            //Port drag movement and node selection
            if (rect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    selectedNode = node;
                    bIsClickInRect = true;
                }
            }

            if (selectedNode == node &&
                Event.current.type == EventType.MouseDrag &&
                Event.current.button == 0)
            {
                node.position += Event.current.delta;
                GUI.changed = true;
                bHasMadeChanges = true;
            }

            // Draw output port
            Rect outPort = GetOutputPortRect(node);
            Rect inPort = GetInputPortRect(node);

            HandlePortClicks(node, inPort, outPort);

            EditorGUI.DrawRect(outPort, Color.yellow);
            EditorGUI.DrawRect(inPort, Color.cyan);

        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !bIsClickInRect)
        {
            selectedNode = null;
        }
    }

    // -------------------------
    // DRAW EXISTING CONNECTIONS
    // -------------------------
    private void DrawConnections()
    {
        if (currentSequence == null || currentSequence.GetAllNodes() == null)
            return;

        foreach (var node in currentSequence.GetAllNodes())
        {
            Vector2 startPos = node.position + new Vector2(200, 60 / 2);

            foreach (string outGuid in node.Outputs)
            {
                SequenceNode target = currentSequence.GetAllNodes()
                    .Find(n => n.Guid == outGuid);

                if (target == null) continue;

                Vector2 endPos = target.position + new Vector2(0, 60 / 2);

                Handles.DrawBezier(
                    startPos,
                    endPos,
                    startPos + Vector2.right * 50,
                    endPos + Vector2.left * 50,
                    Color.white,
                    null,
                    3f
                );

                // Detect right-click on the connection
                if (HandleConnectionClick(startPos, endPos))
                {
                    //DeleteFromConnections(node.Guid);
                    DeleteToConnections(target.Guid);
                    return;
                }
            }
        }

        // Draw live dragging line
        if (startNode != null)
        {
            Vector2 startPos = startNode.position + new Vector2(200, 60 / 2);
            Vector2 endPos = Event.current.mousePosition;

            Handles.DrawBezier(
                startPos,
                endPos,
                startPos + Vector2.right * 50,
                endPos + Vector2.left * 50,
                Color.yellow,
                null,
                2f
            );

            Repaint();
        }
    }

    // -------------------------
    // PORT RECT HELPERS
    // -------------------------
    private Rect GetOutputPortRect(SequenceNode node)
    {
        return new Rect(node.position.x + 200 - 18, node.position.y + 30, 10, 10);
    }

    private Rect GetInputPortRect(SequenceNode node)
    {
        return new Rect(node.position.x + 2, node.position.y + 30, 10, 10);
    }

    // -------------------------
    // PORT NEW NODES
    // -------------------------
    private void AddNode(System.Type nodeType)
    {
        SequenceNode newNode = (SequenceNode)System.Activator.CreateInstance(nodeType);
        newNode.position = mousePosition; // we'll add this variable next
        if (string.IsNullOrEmpty(newNode.Guid)) newNode.Guid = Guid.NewGuid().ToString();
        currentSequence.AddNode(newNode);

        bHasMadeChanges = true;
    }
    #endregion

    #region HandleEvents
    private void HandleEvents()
    {
        #region Drag event
        Event e = Event.current;

        if (e.type == EventType.MouseDrag && e.button == 2)
        {
            drag = e.delta;
            GUI.changed = true;
        }
        else if (e.type == EventType.MouseUp && e.button == 2)
        {
            drag = Vector2.zero;
        }
        #endregion

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            GenericMenu menu = new GenericMenu();
            if (selectedNode != null)
                menu.AddItem(new GUIContent("Delete Node"), false, () => DeleteSelectedNode());

            menu.ShowAsContext();
        }
        else if (e.type == EventType.ContextClick)
        {
            #region Add Nodes
            GenericMenu menu = new GenericMenu();
            
            foreach(var nodeType in MarkedTypes)
            {
                menu.AddItem(new GUIContent("Add/"+nodeType.Name), false, () => AddNode(nodeType));
            }

            menu.ShowAsContext();
            #endregion
        }

        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.S)
        {
            EditorUtility.SetDirty(currentSequence);
            AssetDatabase.SaveAssets();
            bHasMadeChanges = false;
        }
    }

    // -------------------------
    // HANDLE PORT CLICKS
    // -------------------------
    private void HandlePortClicks(SequenceNode node, Rect inputRect, Rect outputRect)
    {
        Event e = Event.current;

        if (GUI.Button(outputRect, "O"))
        {
            startNode = node;
            selectedNode = null;
        }
        if (startNode != null && startNode != node)
        {
            if (GUI.Button(inputRect, "I"))
            {
                // Make connection
                if (!startNode.Outputs.Contains(node.Guid))
                {
                    if (startNode.Outputs.Count > 0)
                        startNode.Outputs.Clear();
                    startNode.Outputs.Add(node.Guid);

                    SequenceConnection newConnection = new SequenceConnection();
                    newConnection.fromNodeGuid = startNode.Guid;
                    newConnection.toNodeGuid = node.Guid;
                    currentSequence.AddConnection(newConnection);

                    bHasMadeChanges = true;
                }

                startNode = null;
                selectedNode = null;
            }
        }

    }

    // -------------------------
    // HANDLE CONNECTIONS CLICKS
    // -------------------------
    private bool HandleConnectionClick(Vector2 start, Vector2 end)
    {
        Event e = Event.current;

        float distance = HandleUtility.DistancePointBezier(
            e.mousePosition,
            start,
            end,
            start + Vector2.right * 50,
            end + Vector2.left * 50
        );

        if (distance < 10f && e.type == EventType.MouseDown && e.button == 1)
        {
            return true; // delete this connection
        }

        return false;
    }
    #endregion

    #region Deletes
    private void DeleteSelectedNode()
    {
        Undo.RecordObject(currentSequence, "Delete Node");
        // Remove this node
        currentSequence.RemoveNode(selectedNode.Guid);

        DeleteAllConnections(selectedNode.Guid);
        selectedNode = null;
    }

    private void DeleteAllConnections(string nodeGUID)
    {
        DeleteToConnections(nodeGUID);
        DeleteFromConnections(nodeGUID);
    }

    private void DeleteFromConnections(string nodeGUID)
    {
        Undo.RecordObject(currentSequence, "Delete Node");
        // Remove connections pointing TO this node
        foreach (var n in currentSequence.GetAllNodes())
            n.Outputs.Remove(nodeGUID);

        currentSequence.RemoveFromConnections(nodeGUID);

        bHasMadeChanges = true;
    }

    private void DeleteToConnections(string nodeGUID)
    {
        Undo.RecordObject(currentSequence, "Delete Node");
        // Remove connections pointing TO this node
        foreach (var n in currentSequence.GetAllNodes())
            n.Outputs.Remove(nodeGUID);

        currentSequence.RemoveToConnections(nodeGUID);

        bHasMadeChanges = true;
    }
    #endregion
}

