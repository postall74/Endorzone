using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    #region Components
    protected Rigidbody _rb;
    #endregion

    #region Fields
    [Header("Параметры")]
    [SerializeField] private float _speed = 1f;
    #endregion

    #region Properties
    public float Speed => _speed;
    #endregion

    public void SetSpeedBullet(float speed)
    {
        _speed = speed;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * _speed;
    }
}
