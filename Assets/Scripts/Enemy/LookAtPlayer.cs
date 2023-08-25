using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    #region Fields
    [Header("Параметры"), Range(0f, 10f)]
    [SerializeField] private float _rotateSpeed = 2f;

    private Vector3 _lookDirection;
    private Transform _player;
    #endregion

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_player != null)
        {
            _lookDirection = _player.position - transform.position;
            _lookDirection.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_lookDirection), _rotateSpeed * Time.deltaTime);
        }
    }
}
