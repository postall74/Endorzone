using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float _bankValue = 180f;
    private RectTransform _healthCanvas;
    private Rigidbody _rb;
    private Vector3 _rotation;
    private Vector3 _lastPosition;
    private Vector3 _velocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _healthCanvas = GetComponentInChildren<RectTransform>();
    }

    private void FixedUpdate()
    {
        _velocity = transform.position - _lastPosition;

        Rotation();
        NotRotation();

        _lastPosition = transform.position;
    }

    private void Rotation()
    {
        _rotation.z = -_velocity.x * _bankValue;
        _rb.MoveRotation(Quaternion.Euler(_rotation));
    }

    private void NotRotation()
    {
        Quaternion healthCanvas = Quaternion.Euler(90, 0, 0);

        _healthCanvas.rotation = healthCanvas;
    }
}
