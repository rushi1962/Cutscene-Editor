using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class SetPlayerControllerEnabledNode : SequenceNode
{
    public override string displayName => "Set Player Controller Enabled";
    public bool isEnabled;

    public override IEnumerator Execute(SequenceContext context)
    {
        GameManager.Instance.SetPlayerControllerEnabled(isEnabled);
        yield return null;
    }

    public override bool OnGUI()
    {
        EditorGUI.BeginChangeCheck(); ; // or whatever width you want

        isEnabled = EditorGUILayout.Toggle("Is Enabled", isEnabled);

        if (EditorGUI.EndChangeCheck())
        {
            GUI.changed = true; // tells editor to save + redraw
            return true;
        }

        return false;
    }
}
