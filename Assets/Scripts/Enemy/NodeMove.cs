using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMove : MonoBehaviour
{
    #region Const
    private const int CURVE_SEGMENT = 20;
    #endregion

    #region Fields
    [Header("���� ���")]
    [SerializeField] private List<Vector3> _nodes = new();
    [Space]
    [Header("��������� �����������")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotateSpeed = 2f;
    [SerializeField] private float _bankValue = 5f;
    [SerializeField] private bool _isRotate;
    [SerializeField] private bool _loopMove;
    [SerializeField] private int _loopToNode;

    private int _realLoopNode;
    private Transform _parent;
    #endregion

    #region Properties
    public List<Vector3> Nodes => _nodes;
    #endregion

    private void OnEnable()
    {
        StartCoroutine(StartMove());
        GetParentTransformPosition();
    }

    private void OnDisable()
    {
        StopCoroutine(StartMove());
    }

    private void OnDrawGizmos()
    {
        List<Vector3> curvePositions = GetCurveNodes();

        for (int i = 1; i < curvePositions.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_parent.TransformPoint(curvePositions[i - 1]), _parent.TransformPoint(curvePositions[i]));
        }

        for (int i = 1; i < _nodes.Count; i++)
        {
            Color gizmoColor = Color.yellow;
            Color nodeColor = Color.red;
            nodeColor.a = 0.75f;
            gizmoColor.a = 0.5f;
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(_nodes[i - 1], _nodes[i]);
            Gizmos.color = nodeColor;
            Gizmos.DrawSphere(_nodes[i - 1], 0.15f);
        }
    }

    #region Calculate Nodes
    private Transform GetParentTransformPosition()
    {
        if (transform.parent)
            _parent = transform.parent;
        else
            _parent = transform;

        return _parent;
    }

    private List<Vector3> GetCurveNodes()
    {
        GetParentTransformPosition();
        List<Vector3> curvedNodes = new();
        curvedNodes.Add(_parent.InverseTransformPoint(transform.position));

        for (int i = 0; i < _nodes.Count - 3; i += 3)
        {
            Vector3 p0 = _parent.InverseTransformPoint(_nodes[i]);
            Vector3 p1 = _parent.InverseTransformPoint(_nodes[i + 1]);
            Vector3 p2 = _parent.InverseTransformPoint(_nodes[i + 2]);
            Vector3 p3 = _parent.InverseTransformPoint(_nodes[i + 3]);

            if (i == 0)
            {
                p0 = _parent.InverseTransformPoint(transform.position);
                curvedNodes.Add(CalculateBezierPath(p0, p1, p2, p3, 0f));
            }

            for (int j = 0; j < CURVE_SEGMENT; j++)
            {
                float t = j / (float)CURVE_SEGMENT;
                curvedNodes.Add(CalculateBezierPath(p0, p1, p2, p3, t));
            }
        }

        _realLoopNode = (int)(curvedNodes.Count * (_loopToNode / (float)_nodes.Count));
        
        return curvedNodes;
    }

    private Vector3 CalculateBezierPath(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        //((1-t)*(1-t)*(1-t)) * p0 + 3*t * ((1-t)*(1-t)) * p1 + ((3*t) + (3*t)) * (1-t) * p2 + (t*t*t) * p3

        float oneMinusT = 1f - t;

        Vector3 result = Mathf.Pow(oneMinusT, 3f) * p0 +
                         3f * Mathf.Pow(oneMinusT, 2f) * t * p1 +
                         3f * oneMinusT * (t * t) * p2 +
                         Mathf.Pow(t, 3f) * p3;
            
                       /**Mathf.Pow(oneMinusT, 3f) * p0 +
                         (3 * t) * Mathf.Pow(oneMinusT, 2f) * p1 +
                         Mathf.Pow(3 * t, 2f) * oneMinusT * p2 +
                         Mathf.Pow(t, 3f) * p3;*/

        return result;
    }
    #endregion

    #region Movement object
    private IEnumerator StartMove()
    {
        int posId = 0;
        List<Vector3> path = GetCurveNodes();

        while (_loopMove || posId < path.Count - 1)
        {
            if ((path[posId] - transform.localPosition).sqrMagnitude < 0.01f)
            {
                if (_loopMove)
                {
                    if (posId < path.Count - 1)
                        posId++;
                    else
                        posId = _realLoopNode;
                }
                else
                {
                    if (posId < path.Count - 1)
                        posId++;
                }
            }

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, path[posId], _speed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion
}
