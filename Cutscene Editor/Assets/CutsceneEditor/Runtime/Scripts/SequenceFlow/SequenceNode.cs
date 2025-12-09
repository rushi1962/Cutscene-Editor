using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SequenceNode
{
    public string Guid;
    public Vector2 position;
    public virtual string displayName => GetType().Name;
    public List<string> Outputs = new List<string>();  // GUIDs of connected nodes

    public virtual float height => 60f;

    public virtual IEnumerator Execute(SequenceContext context)
    {
        yield return null;
    }

    public virtual bool OnGUI()
    {
        return false;
    }
}
