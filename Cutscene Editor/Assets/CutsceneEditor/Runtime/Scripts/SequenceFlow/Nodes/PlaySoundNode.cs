using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable, SequenceNode]
public class PlaySoundNode : SequenceNode
{
    public override string displayName => "PlaySound";
    public AudioClip clip;

    public override float height => 100f;

    public override IEnumerator Execute(SequenceContext context)
    {
        if (context.AudioSource != null && clip != null)
            context.AudioSource.PlayOneShot(clip);

        yield return null;
    }

    public override bool OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        clip = (AudioClip)EditorGUILayout.ObjectField("clip", clip, typeof(AudioClip), false);

        if (EditorGUI.EndChangeCheck())
        {
            GUI.changed = true; // tells editor to save + redraw
            return true;
        }

        return false;
    }
}
