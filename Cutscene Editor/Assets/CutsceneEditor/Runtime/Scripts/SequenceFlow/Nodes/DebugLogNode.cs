using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable, SequenceNode]
public class DebugLogNode : SequenceNode
{
    public override string displayName => "Debug Log";
    public string message = "Hello from DebugLogNode";

    public override float height => 100f;

    public override IEnumerator Execute(SequenceContext context)
    {
        Debug.Log("[Sequence] " + message);
        yield return null;
    }

    public override bool OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        message = EditorGUILayout.TextField("message", message);

        if (EditorGUI.EndChangeCheck())
        {
            GUI.changed = true; // tells editor to save + redraw
            return true;
        }

        return false;
    }
}
