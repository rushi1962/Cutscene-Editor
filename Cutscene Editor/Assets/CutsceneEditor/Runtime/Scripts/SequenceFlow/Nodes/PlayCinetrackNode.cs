using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[Serializable, SequenceNode]
public class PlayCinetrackNode : SequenceNode
{
    public override string displayName => "Play Cutscene";
    public PlayableAsset cutsceneAsset;

    public override IEnumerator Execute(SequenceContext context)
    {
        foreach(var virtualCamera in context.TriggerVolume.GetVirtualCameras())
        {
            virtualCamera.Follow = context.Instigator.transform;
            virtualCamera.LookAt = context.Instigator.transform;
            virtualCamera.gameObject.SetActive(true);
        }
        context.TriggerVolume.GetPlayableDirector().Play(cutsceneAsset);
        yield return new WaitWhile(() => context.TriggerVolume.GetPlayableDirector().state == PlayState.Playing);

        context.TriggerVolume.GetPlayableDirector().Stop();
        foreach (var virtualCamera in context.TriggerVolume.GetVirtualCameras())
        {
            virtualCamera.gameObject.SetActive(false);
        }
    }

    public override bool OnGUI()
    {
        EditorGUI.BeginChangeCheck(); ; // or whatever width you want

        cutsceneAsset = (PlayableAsset)EditorGUILayout.ObjectField("cutscene Asset", cutsceneAsset, typeof(PlayableAsset), false);

        if (EditorGUI.EndChangeCheck())
        {
            GUI.changed = true; // tells editor to save + redraw
            return true;
        }

        return false;
    }
}
