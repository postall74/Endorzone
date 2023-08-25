using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AutoMove : MonoBehaviour
{
    #region Fields
    [Header("Параметры")]
    [SerializeField] private Vector3 _moveOffset;
    [SerializeField] private float _duration;

    [Header("Логические параметры")]
    [SerializeField] private bool _onStart;
    [SerializeField] private bool _isReverse;

    [Header("События начала и окончания движения")]
    [SerializeField] private UnityEvent _onStartMove;
    [SerializeField] private UnityEvent _OnMoveDone;

    private Vector3 _targetPosition;
    private Vector3 _initialPosition;
    private float _moveDistance;
    #endregion

    private void Start()
    {
        _initialPosition = transform.localPosition;
        _moveDistance = _moveOffset.magnitude;

        if (_onStart)
            Move(_isReverse);
    }

    #region Methods moving
    public void Move(bool reverse)
    {
        StartCoroutine(StartMove(reverse, _duration));
    }

    private IEnumerator StartMove(bool reverse, float time)
    {
        _targetPosition = reverse ? _initialPosition : _initialPosition + _moveOffset;

        if (reverse)
            transform.localPosition += _moveOffset;

        _onStartMove.Invoke();

        while (transform.localPosition != _targetPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _targetPosition, (_moveDistance / time) * Time.deltaTime);
            yield return null;
        }

        _OnMoveDone.Invoke();
    }
    #endregion
}
