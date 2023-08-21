using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Constants
    private const string _bullet = "Bullet";
    private const string _enemyBullet = "Enemy bullet";
    #endregion

    #region Components
    [Header("Коомпоненты")]
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GameObject _healthBar;
    #endregion

    #region Fields
    private bool _isEnemy = true;
    private float _currentHealth;
    private string _tagName;
    private DeathSystem _deathScript;
    [Header("Значения")]
    [SerializeField] private float _maxHealth = 10f;
    #endregion

    #region Properties
    public bool IsEnemy => _isEnemy;
    #endregion

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        CheckHealth();
        UpdateUI();
    }

    private void Start()
    {
        _deathScript = GetComponent<DeathSystem>();
    }

    private void OnEnable()
    {
        if (_isEnemy)
            _tagName = _bullet;
        else
            _tagName = _enemyBullet;

        _currentHealth = _maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tagName))
        {
            Vector3 triggerPosition = other.ClosestPointOnBounds(transform.position);
            Vector3 direction = triggerPosition - transform.position;

            GameObject fx = PoolingManager.Instance.UseObject(_hitEffect, triggerPosition, Quaternion.LookRotation(direction));

            PoolingManager.Instance.ReturnObject(fx, 1f);

            float damage = float.Parse(other.name);
            TakeDamage(damage);

            PoolingManager.Instance.ReturnObject(other.gameObject);
        }
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0f)
        {
            if (_deathScript != null)
                _deathScript.Death();
        }
    }

    private void UpdateUI()
    {
        if (_healthBar != null)
        {
            Vector3 scale = Vector3.one;
            float value = _currentHealth / _maxHealth;
            scale.x = value;
            _healthBar.transform.localScale = scale;
        }
    }
}
