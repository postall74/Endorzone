using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeMove))]
public class NodeMoveEditor : Editor
{
    #region Fields
    private NodeMove _source;
    #endregion

    public override void OnInspectorGUI()
    {
        _source = (NodeMove)target;

        if (_source == null)
        {
            Debug.LogError("NodeMove component is missing.");
            return;
        }

        base.OnInspectorGUI();

        if (GUILayout.Button("Add nodes"))
            _source.Nodes.Add(_source.transform.position);

        if (GUILayout.Button("Remove nodes"))
        {
            if (_source.Nodes.Count > 0)
                _source.Nodes.RemoveAt(_source.Nodes.Count - 1);
        }
    }

    private void OnSceneGUI()
    {
        _source = (NodeMove)target;

        if (_source == null)
        {
            Debug.LogError("NodeMove component is missing.");
            return;
        }

        for (int i = 0; i < _source.Nodes.Count; i++)
        {
            _source.Nodes[i] = Handles.PositionHandle(_source.Nodes[i], Quaternion.identity);
            Handles.Label(_source.Nodes[i], "Nodes " + (i + 1));
        }
    }
}
