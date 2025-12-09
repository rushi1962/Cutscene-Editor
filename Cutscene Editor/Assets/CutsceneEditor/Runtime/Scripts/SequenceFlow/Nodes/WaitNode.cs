using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable, SequenceNode]
public class WaitNode : SequenceNode
{
    public override string displayName => "Wait";
    public float duration = 1f;

    public override float height => 100f;

    public override IEnumerator Execute(SequenceContext context)
    {
        yield return new WaitForSeconds(duration);
    }

    public override bool OnGUI()
    {
        EditorGUI.BeginChangeCheck();; // or whatever width you want

        duration = EditorGUILayout.FloatField("Duration", duration);

        if (EditorGUI.EndChangeCheck())
        {
            GUI.changed = true; // tells editor to save + redraw
            return true;
        }

        return false;            
    }
}
