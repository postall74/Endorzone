using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    #region Components
    [SerializeField] private Transform _player;
    #endregion

    #region Fields
    [Header("Основные параметры")]
    [SerializeField] private float _minValueX = -1.45f;
    [SerializeField] private float _maxValueX = 1.45f;
    [Range(0.5f, 5f)]
    [SerializeField] private float _speed = 2f;
    private Vector3 _position;
    #endregion

    private void LateUpdate()
    {
        if (_player == null)
            return;

        _position = transform.localPosition;
        _position.x = _player.localPosition.x;
        _position.x = Mathf.Clamp(_position.x, _minValueX, _maxValueX);

        transform.localPosition = Vector3.Lerp(transform.localPosition, _position, _speed * Time.deltaTime);
    }
}
