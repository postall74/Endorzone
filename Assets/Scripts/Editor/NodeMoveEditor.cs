using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeMove))]
public class NodeMoveEditor : Editor
{
    #region Fields
    private NodeMove _source;
    #endregion

    public override void OnInspectorGUI()
    {
        _source = (NodeMove)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Add nodes"))
        {
            _source.Nodes.Add(_source.transform.position);
        }

        if (GUILayout.Button("Remove nodes"))
        {
            _source.Nodes.RemoveAt(_source.Nodes.Count - 1);
        }
    }

    private void OnSceneGUI()
    {
        _source = (NodeMove)target;

        for (int i = 0; i < _source.Nodes.Count; i++)
        {
            _source.Nodes[i] = Handles.PositionHandle(_source.Nodes[i], Quaternion.identity);
            Handles.Label(_source.Nodes[i], "Nodes " + (i + 1));
        }
    }
}
