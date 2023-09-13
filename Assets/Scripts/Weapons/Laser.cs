using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Laser : MonoBehaviour
{
    #region Components
    [SerializeField] private ParticleSystem _brustFX;
    private Rigidbody _rb;
    private Collider _coll;
    #endregion

    #region Fields
    [Header("Основные параметры лазера")]
    [Range(0.1f, 7f)]
    [SerializeField] private float _duration = 3f;
    [Range(1f, 10f)]
    [SerializeField] private float _animationSpeed = 2f;
    private bool _isPlaying = false;
    private WaitForSeconds _coroutineDuration;
    #endregion

    #region Properties
    #endregion

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody rb))
            _rb = rb;

        if(TryGetComponent(out Collider collider)) 
            _coll = collider;
    }

    private void Start()
    {
        _coroutineDuration = new WaitForSeconds(_duration);
        _coll.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();

            if (health)
            {
                health.TakeDamage(100f, other);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isPlaying)
        {
            StartCoroutine(FireLaser());
        }
    }

    private IEnumerator FireLaser()
    {
        _coll.enabled = true;
        _isPlaying = true;
        transform.localScale = Vector3.zero;
        _brustFX.Play();

        while (transform.localScale.sqrMagnitude < 1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, _animationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = Vector3.one;
        yield return _coroutineDuration;

        while (transform.localScale.sqrMagnitude > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, _animationSpeed * Time.deltaTime);
            yield return null;
        }

        _brustFX.Stop();
        transform.localScale = Vector3.zero;
        _isPlaying = false;
        _coll.enabled = false;
    }
}
