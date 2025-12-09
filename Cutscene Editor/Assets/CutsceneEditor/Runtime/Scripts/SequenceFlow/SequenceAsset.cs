using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SequenceAsset", menuName = "Sequence/SequenceAsset")]
public class SequenceAsset : ScriptableObject
{
    [SerializeField] [SerializeReference]
    protected List<SequenceNode> nodes = new List<SequenceNode>();

    [SerializeField]
    protected List<SequenceConnection> connections = new List<SequenceConnection>();

    [SerializeField]
    protected string rootNodeGuid;

    public SequenceAsset()
    {
        SequenceNode rootNode = new RootNode();
        rootNode.position = new Vector2(0, 0);
        if (string.IsNullOrEmpty(rootNode.Guid)) rootNode.Guid = Guid.NewGuid().ToString();
        AddNode(rootNode);
    }

    public SequenceNode GetNode(string guid)
    {
        return nodes.Find(n => n.Guid == guid);
    }

    public string GetRootNode()
    {
        return rootNodeGuid;
    }

    public SequenceConnection GetConnectionWithFromGUID(string fromGuid)
    {
        return connections.Find(c => c.fromNodeGuid == fromGuid);
    }

    public SequenceConnection GetConnectionWithToGUID(string toGuid)
    {
        return connections.Find(c => c.toNodeGuid == toGuid);
    }

    public List<SequenceNode> GetAllNodes()
    {
        return nodes;
    }

    public List<SequenceConnection> GetAllConnections()
    {
        return connections;
    }

    public void AddNode(SequenceNode node)
    {
        if(nodes.Count == 0)
        {
            rootNodeGuid = node.Guid;
        }
        nodes.Add(node);
    }

    public void RemoveNode(string guid)
    {
        if(guid == rootNodeGuid)
        {
            Debug.LogError("Can't remove root node!");
            return;
        }

        SequenceNode node = GetNode(guid);

        nodes.Remove(node);
    }

    public void AddConnection(SequenceConnection connection)
    {
        connections.Add(connection);
    }

    public void RemoveFromConnections(string fromNodeGuid)
    {
        SequenceConnection fromConnection = GetConnectionWithFromGUID(fromNodeGuid);
        connections.Remove(fromConnection);
    }

    public void RemoveToConnections(string toNodeGuid)
    {
        SequenceConnection toConnection = GetConnectionWithToGUID(toNodeGuid);
        connections.Remove(toConnection);
    }
}

[System.Serializable]
public class SequenceConnection
{
    public string fromNodeGuid;
    public string toNodeGuid;
}
