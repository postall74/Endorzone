using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    #region Components
    private Rigidbody _rb;
    #endregion

    #region Fields
    [SerializeField] private float speed;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * speed;
    }
}
