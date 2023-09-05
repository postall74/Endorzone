using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CoinMovement : MonoBehaviour
{
    #region Fields
    [Header("Компоненты")]
    private Rigidbody _rb;
    private Magnet _magnet;
    [Header("Основные параметры")]
    [Range(0.5f, 10f)]
    [SerializeField] private float _speed = 3f;
    private Vector3 _movement;
    private Vector3 _target;
    #endregion

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody rb))
            _rb = rb;
    }

    private void OnEnable()
    {
        _movement = transform.position;
        _movement += Random.insideUnitSphere * _speed;
        _movement.y = 0f;
    }

    private void Start()
    {
        _magnet = FindFirstObjectByType<Magnet>();
    }

    private void FixedUpdate()
    {
        _target = Vector3.Lerp(transform.position, _movement, 1f * Time.fixedDeltaTime);

        if ((_magnet.transform.position - transform.position).sqrMagnitude < Mathf.Pow(_magnet.MagnetRange, 2f))
        {
            _target = Vector3.Lerp(transform.position, _magnet.transform.position, _magnet.MagnetPower * Time.fixedDeltaTime);
        }

        _rb.MovePosition(_target);
        _rb.MoveRotation(Quaternion.Euler(_target));
    }
}
