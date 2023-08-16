using UnityEngine;
using UnityEngine.Events;

public class DeathSystem : MonoBehaviour
{
    #region Components
    [Header("Компоненты")]
    private Collider[] _colliders;
    [SerializeField] private CreateObject[] _spawnObjects;
    #endregion 

    #region Fields
    private bool _isDestroy = true;
    [Header("Значения")]
    [SerializeField] private float _delayDestroy;
    [SerializeField] private UnityEvent _onDeathEvent;
    #endregion

    #region Properties
    public bool IsDestroy => _isDestroy;
    public UnityEvent OnDeathEvent => _onDeathEvent;
    #endregion

    public void Death()
    {
        for (int i = 0; i < _spawnObjects.Length; i++)
        {

        }

        if (_isDestroy)
        {
            PoolingManager.Instance.ReturnObject(gameObject, _delayDestroy);
        }

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }
    }

    private void Start()
    {
        _colliders = GetComponents<Collider>();
    }


}
