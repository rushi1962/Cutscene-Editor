using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[Serializable, SequenceNode]
public class PlayAnimationNode : SequenceNode
{
    public override string displayName => "Play Animation";

    public string hashName;
    public Animator animator;

    public override float height => 100f;

    public override IEnumerator Execute(SequenceContext context)
    {
        if (animator != null && hashName != "")
            animator.Play(hashName);

        yield return null;
    }

    public override bool OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        hashName = (string)EditorGUILayout.TextField("Hash name", hashName);
        animator = (Animator)EditorGUILayout.ObjectField("Animator", animator, typeof(Animator), true);

        if (EditorGUI.EndChangeCheck())
        {
            GUI.changed = true; // tells editor to save + redraw
            return true;
        }

        return false;
    }
}
