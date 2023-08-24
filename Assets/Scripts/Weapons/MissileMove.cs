using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove : BulletMove
{
    #region Components
    private Transform _player;
    #endregion

    #region Fileds
    [Header("Параметры  ракеты")]
    [Range(2f, 15f)]
    [SerializeField] private float _rotateSpeed = 3f;
    [Range(2f, 15f)]
    [SerializeField] private float _followDuration = 5f;
    private WaitForSeconds _physicsTimeStep;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _physicsTimeStep = new WaitForSeconds(Time.fixedDeltaTime);
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        StartCoroutine(StartFollow(_followDuration));
    }

    private void OnDisable()
    {
        StopCoroutine(StartFollow(_followDuration));
    }

    #region Methods Rotate to player
    private IEnumerator StartFollow(float followDuration)
    {
        while (followDuration > 0)
        {
            followDuration -= Time.fixedDeltaTime;

            if (_player != null)
            {
                Vector3 temp = _player.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(temp), _rotateSpeed * Time.fixedDeltaTime);
            }

            _rb.velocity = transform.forward * Speed;

            yield return _physicsTimeStep;
        }
    }
    #endregion
}
