using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeMove : MonoBehaviour
{
    #region Const
    private const int CURVE_SEGMENT = 20;
    #endregion

    #region Fields
    [Header("Узлы нод")]
    [SerializeField] private List<Vector3> _nodes = new();
    [Space]
    [Header("Параметры перемещения")]
    [Range(0f, 10f)]
    [SerializeField] private float _speed = 5f;
    [Header("Параметры поворота и крена")]
    [Range(0f, 5f)]
    [SerializeField] private float _rotateSpeed = 2f;
    [Range(0f, 50f)]
    [SerializeField] private float _bankValue = 5f;
    [SerializeField] private bool _isRotate;
    [Header("Параметры движения по кругу")]
    [SerializeField] private bool _loopMove;
    [SerializeField] private int _loopToNode;
    [Space]
    private int _realLoopNode;
    private float _nextAngleGrab;
    private float oldAngle = 0f;
    private Quaternion _rotation;
    private Transform _parent;
    #endregion

    #region Properties
    public List<Vector3> Nodes => _nodes;
    #endregion

    private void OnEnable()
    {
        _parent = GetParentTransformPosition();
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

    #region Methods Calculate Nodes
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
        if (_nodes.Count > 4)
            curvedNodes.Capacity = (_nodes.Count - 3) * CURVE_SEGMENT + 2;
        else
            curvedNodes.Capacity = 1;
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
        float oneMinusT = 1f - t;

        Vector3 result = Mathf.Pow(oneMinusT, 3f) * p0 +
                         3f * Mathf.Pow(oneMinusT, 2f) * t * p1 +
                         3f * oneMinusT * (t * t) * p2 +
                         Mathf.Pow(t, 3f) * p3;
        return result;
    }
    #endregion

    #region Methods Movement object
    private void UpdatePosition(Vector3 targetPosition)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, _speed * Time.deltaTime);
    }

    private void UpdateRotation(Vector3 targetDirection)
    {
        if (targetDirection.sqrMagnitude > 0.01f)
        {
            _rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            float zBank = Mathf.Clamp(_rotation.eulerAngles.y - oldAngle, -10f, 10f);
            Quaternion bank = Quaternion.Euler(0f, 0f, Mathf.Ceil(zBank) * _bankValue);
            _rotation *= bank;
            oldAngle = _rotation.eulerAngles.y;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, _rotateSpeed * Time.deltaTime);
    }

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

            UpdatePosition(path[posId]);

            if (_isRotate)
            {
                Vector3 targetDirection = path[posId] - transform.localPosition;
                UpdateRotation(targetDirection);
            }

            yield return null;
        }
    }
    #endregion
}
