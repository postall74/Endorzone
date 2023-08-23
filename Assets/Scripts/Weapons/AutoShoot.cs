using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AutoShoot : MonoBehaviour
{

    #region Components
    [SerializeField] private ShootProfiles _shootProfiles;
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField] private Transform _firePoint;
    #endregion

    #region Fields
    private float _totalSpread;
    private WaitForSeconds _rate;
    private WaitForSeconds _interval;
    #endregion

    private void OnEnable()
    {
        _interval = new WaitForSeconds(_shootProfiles.Interval);
        _rate = new WaitForSeconds(_shootProfiles.FireRate);

        if (_firePoint == null)
            _firePoint = transform;

        _totalSpread = _shootProfiles.Spread * _shootProfiles.Amount;

        StartCoroutine(ShootingSequence());
    }

    private void OnDisable()
    {
        StopCoroutine(ShootingSequence());
    }

    private IEnumerator ShootingSequence()
    {
        yield return _rate;

        while (true)
        {
            float angle = 0f;

            if (_shootProfiles.Amount > 1)
            {
                for (int i = 0; i < _shootProfiles.Amount; i++)
                {
                    angle = _totalSpread * (i / (float)_shootProfiles.Amount);
                    angle -= (_totalSpread / 2f) - (_shootProfiles.Spread / _shootProfiles.Amount);
                    Shoot(angle);

                    if (_shootProfiles.FireRate > 0f)
                        yield return _rate;
                }
            }

            yield return _interval;
        }
    }

    private void Shoot(float angle)
    {
        GameObject temp = PoolingManager.Instance.UseObject(_bulletPrefabs, _firePoint.position, _firePoint.rotation);
        temp.name = _shootProfiles.Damage.ToString();
        temp.transform.Rotate(Vector3.up, angle);
    }
}
