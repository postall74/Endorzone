using System.Collections;
using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    #region Components
    [SerializeField] private ShootProfiles _shootProfiles;
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField, Tooltip("Точка выстрела")] 
    private Transform _firePoint;
    #endregion

    #region Fields
    private float _totalSpread;
    private WaitForSeconds _rate;
    private WaitForSeconds _interval;
    #endregion

    private void OnEnable()
    {
        if (_shootProfiles == null)
        {
            UnityEngine.Debug.Log("ShootProfiles is not assigned.");
            return;
        }

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

            for (int i = 0; i < _shootProfiles.Amount; i++)
            {
                angle = _totalSpread * (i / (float)_shootProfiles.Amount);
                angle -= (_totalSpread / 2f) - (_shootProfiles.Spread / _shootProfiles.Amount);
                Shoot(angle);

                if (_shootProfiles.FireRate > 0f)
                    yield return _rate;
            }

            yield return _interval;
        }
    }

    private void Shoot(float angle)
    {
        if (_bulletPrefabs == null)
        {
            UnityEngine.Debug.Log("Bullet prefab is not assigned.");
            return;
        }

        Quaternion bulletRotation = Quaternion.Euler(_firePoint.eulerAngles.x, _firePoint.eulerAngles.y, 0f);

        GameObject temp = PoolingManager.Instance.UseObject(_bulletPrefabs, _firePoint.position, bulletRotation);
        temp.name = _shootProfiles.Damage.ToString();
        temp.transform.Rotate(Vector3.up, angle);
        temp.GetComponent<BulletMove>().SetSpeedBullet(_shootProfiles.Speed);
        PoolingManager.Instance.ReturnObject(temp, _shootProfiles.DestroyRate);
    }
}
