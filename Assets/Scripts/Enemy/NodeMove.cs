using System.Collections.Generic;
using UnityEngine;

public class NodeMove : MonoBehaviour
{
    #region Const
    private const int CURVE_SEGMENT = 20;
    #endregion

    #region Fields
    [SerializeField] private List<Vector3> _nodes = new();
    #endregion

    private List<Vector3> GetCurveNodes()
    {
        List<Vector3> curvedNodes = new();
        curvedNodes.Add(transform.position);

        for (int i = 0; i < _nodes.Count - 3; i += 3)
        {
            Vector3 p0 = _nodes[i];
            Vector3 p1 = _nodes[i + 1];
            Vector3 p2 = _nodes[i + 2];
            Vector3 p3 = _nodes[i + 3];

            if (i == 0)
            {
                curvedNodes.Add(CalculateBezierPath(p0, p1, p2, p3, 0f));
            }

            for (int j = 0; j < CURVE_SEGMENT; j++)
            {
                float t = j / (float)CURVE_SEGMENT;
                curvedNodes.Add(CalculateBezierPath(p0, p1, p2, p3, t));
            }
        }

        return curvedNodes;
    }

    private Vector3 CalculateBezierPath(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        //((1-t)*(1-t)*(1-t)) * p0 + 3*t * ((1-t)*(1-t)) * p1 + ((3*t) + (3*t)) * (1-t) * p2 + (t*t*t) * p3

        float oneMinusT = 1f - t;

        Vector3 result = Mathf.Pow(oneMinusT, 3f) * p0 +
                         3 * t * Mathf.Pow(oneMinusT, 2f) * p1 +
                         Mathf.Pow(3 * t, 2f) * oneMinusT * p2 +
                         Mathf.Pow(t, 3f) * p3;

        return result;
    }

    private void OnDrawGizmos()
    {
        List<Vector3> curvePositions = GetCurveNodes();

        for (int i = 1; i < curvePositions.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(curvePositions[i - 1], curvePositions[i]);
        }
    }
}
