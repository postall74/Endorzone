using UnityEngine;

[CreateAssetMenu(fileName = "ShootProfile", menuName = "Shoot Profile", order = 1)]
public class ShootProfiles : ScriptableObject
{
    #region Fields
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _interval;
    [SerializeField] private float _destroyRate;
    [SerializeField] private float _spread;
    [SerializeField] private int _amount;
    #endregion

    #region Properties
    public float Speed => _speed;
    public float Damage => _damage;
    public float FireRate => _fireRate;
    public float Interval => _interval;
    public float DestroyRate => _destroyRate;
    public float Spread => _spread;
    public int Amount => _amount;
    #endregion
}
