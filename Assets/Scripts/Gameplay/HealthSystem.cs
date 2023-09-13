using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Constants
    private const string Bullet = "Bullet";
    private const string EnemyBullet = "Enemy bullet";
    private const string Untagget = "Untagget";
    #endregion

    #region Components
    [Header("Компоненты")]
    [SerializeField, Tooltip("Эффект попадания")] 
    private GameObject _hitEffect;
    [SerializeField, Tooltip("Полоска здоровья")] 
    private GameObject _healthBar;
    #endregion

    #region Fields
    [SerializeField, Tooltip("Является противником")] 
    private bool _isEnemy = true;
    private float _currentHealth;
    private string _tagName = EnemyBullet;
    private DeathSystem _deathScript;
    [Header("Значения")]
    [SerializeField, Range(1, 100), Tooltip("Максимальное здоровье")] 
    private float _maxHealth = 10f;
    #endregion

    #region Properties
    public bool IsEnemy => _isEnemy;
    #endregion

    public void TakeDamage(float damage, Collider other)
    {
        //if (!_isEnemy)
        //{
        //    LevelManager.Instance.PlayerHit();
        //}
        CreateHitDamageFX(other);
        _currentHealth -= damage;
        CheckHealth();
        UpdateUI();
    }

    public void CreateHitDamageFX(Collider other)
    {
        Vector3 triggerPosition = other.ClosestPointOnBounds(transform.position);
        Vector3 direction = triggerPosition - transform.position;

        GameObject fx = PoolingManager.Instance.UseObject(_hitEffect, triggerPosition, Quaternion.LookRotation(direction));

        PoolingManager.Instance.ReturnObject(fx, 1f);
    }

    private void Start()
    {
        if (TryGetComponent(out DeathSystem deathSystem))
            _deathScript = deathSystem;
    }

    private void OnEnable()
    {
        _tagName = _isEnemy ? Bullet : EnemyBullet;
        _currentHealth = _maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tagName))
        {
            float damage = float.Parse(other.name);
            TakeDamage(damage, other);

            PoolingManager.Instance.ReturnObject(other.gameObject);
        }
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0f)
        {
            _healthBar?.transform.parent.gameObject.SetActive(false);
            _deathScript?.Death();

            if (_isEnemy)
                gameObject.tag = Untagget;
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
