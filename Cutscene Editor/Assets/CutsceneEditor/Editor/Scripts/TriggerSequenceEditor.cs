using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerSequence))]
public class TriggerSequenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var trigger = (TriggerSequence)target;

        if (trigger.GetSequenceAsset() != null)
        {
            if (GUILayout.Button("Open Sequence Editor"))
            {
                SequenceEditorWindow.OpenSequence(trigger.GetSequenceAsset());
            }
        }
    }
}
