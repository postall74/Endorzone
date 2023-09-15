using System;
using System.Collections;
using UnityEngine;

public class MissileMove : BulletMove
{
    #region Constants
    private const string Player = "Player";
    private const string Enemy = "Enemy";
    private const string EnemyBullet = "Enemy bullet";
    #endregion

    #region Components
    private Transform _target;
    #endregion

    #region Fileds
    [Header("Параметры  ракеты")]
    [Range(2f, 15f)]
    [SerializeField] private float _rotateSpeed = 3f;
    [Range(2f, 15f)]
    [SerializeField] private float _followDuration = 5f;
    [SerializeField] private bool _isPlayer = false;
    private WaitForSeconds _physicsTimeStep;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _physicsTimeStep = new WaitForSeconds(Time.fixedDeltaTime);
    }

    private void Start()
    {
        if (!_isPlayer)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(Player);

            if (playerObject != null)
                _target = FindTarget(Player);
        }

    }

    private void OnEnable()
    {
        StartCoroutine(StartFollow(_followDuration));

        if (_isPlayer)
        {
            GameObject enemyObject = GameObject.FindGameObjectWithTag(Enemy);

            if (enemyObject != null)
                _target = FindTarget(Enemy);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #region Methods Rotate to targer
    private Transform FindTarget(string targetName)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetName);
        Array.Sort(targets, delegate (GameObject a, GameObject b)
        {
            return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position));
        });

//#if UNITY_EDITOR
//        foreach (GameObject target in targets)
//        {
//            Debug.Log(Vector3.Distance(transform.position, target.transform.position));
//        }
//#endif

        return targets[0].transform;
    }

    private IEnumerator StartFollow(float followDuration)
    {
        while (followDuration > 0)
        {
            followDuration -= Time.fixedDeltaTime;

            if (_target != null)
            {
                Vector3 temp = _target.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(temp), _rotateSpeed * Time.fixedDeltaTime);
            }

            _rb.velocity = transform.forward * Speed;

            yield return _physicsTimeStep;
        }
    }
    #endregion
}
