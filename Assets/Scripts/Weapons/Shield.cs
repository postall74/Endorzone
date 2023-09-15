using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region Constants
    private const string Enemy = "Enemy";
    private const string EnemyBullet = "Enemy bullet";
    #endregion

    #region Components
    [SerializeField] private GameObject _hitEffect;
    private Collider _coll;
    #endregion

    #region Fields
    [Header("Основные параметры")]
    [SerializeField] private float _duration;
    private WaitForSeconds _delay;
    private bool _isPlaying = false;
    #endregion

    #region Properties
    #endregion

    public void ShieldUp()
    {
        StartCoroutine(ShieldActivate());
    }

    public void CreateHitDamageFX(Collider other)
    {
        Vector3 triggerPosition = other.ClosestPointOnBounds(transform.position);
        Vector3 direction = triggerPosition - transform.position;

        GameObject fx = PoolingManager.Instance.UseObject(_hitEffect, triggerPosition, Quaternion.LookRotation(direction));

        PoolingManager.Instance.ReturnObject(fx, 1f);
    }

    private void Awake()
    {
        if (TryGetComponent(out Collider collider))
            _coll = collider;
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        _delay = new WaitForSeconds(_duration);
        _coll.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Enemy) || other.CompareTag(EnemyBullet))
        {
            CreateHitDamageFX(other);

            HealthSystem health = other.GetComponent<HealthSystem>();

            if (health)
            {
                health.TakeDamage(100f, other);
            }
            else
            {
                PoolingManager.Instance.ReturnObject(other.gameObject);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !_isPlaying)
        {
            ShieldUp();
        }
    }

    private IEnumerator ShieldActivate()
    {
        float inAnimDuration = 0.5f;
        float outAnimDuration = 0.5f;
        _coll.enabled = true;
        _isPlaying = true;

        while (inAnimDuration > 0f)
        {
            inAnimDuration -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.1f);
            yield return null;
        }

        yield return _delay;

        while (outAnimDuration > 0f)
        {
            outAnimDuration -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);
            yield return null;
        }

        _isPlaying = false;
        _coll.enabled = false;
    }
}
