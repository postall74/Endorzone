using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    #region Constants
    private const string Player = "Player";
    private const string Enemy = "Enemy";
    private const string EnemyBullet = "Enemy bullet";
    #endregion

    #region Components

    #endregion

    #region Fields
    [Header("Основные параметры")]
    [SerializeField] private int _damage;
    private bool _isDestroy;
    #endregion

    #region Properties

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (_isDestroy)
        {
            return;
        }

        if (other.CompareTag(Player))
        {
            other.GetComponent<HealthSystem>().TakeDamage(_damage, other);
            GetComponent<HealthSystem>().TakeDamage(500, other);
            _isDestroy = true;
        }
    }
}
