using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovemet : MonoBehaviour
{
    #region Components
    [Header("Компоненты")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _visualChildModel;
    private Rigidbody _rb;
    #endregion

    #region Fields
    [Header("Основные параметры")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _bankValue = 180f;
    private float _distance;
    private Vector3 _velocity;
    private Vector3 _lastPosition;
    private Vector3 _rotation;
    private Vector3 _touchPosition;
    private Vector3 _screenToWorld;
    #endregion

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _distance = (_cam.transform.position - transform.position).y;
    }

    private void FixedUpdate()
    {
        _velocity = transform.position - _lastPosition;

        Move();

        _lastPosition = transform.position;
    }

    private void Move()
    {
        _touchPosition = Input.mousePosition;
        _touchPosition.z = _distance;

        _screenToWorld = _cam.ScreenToWorldPoint(_touchPosition);

        Vector3 movement = Vector3.Lerp(transform.position, _screenToWorld, _speed * Time.fixedDeltaTime);
        _rb.MovePosition(movement);

        _rotation.z = -_velocity.x * _bankValue;
        _rb.MoveRotation(Quaternion.Euler(_rotation));
    }
}
