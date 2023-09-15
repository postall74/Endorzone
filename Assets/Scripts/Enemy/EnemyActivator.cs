using UnityEngine;
using UnityEngine.Events;

public class EnemyActivator : MonoBehaviour
{
    #region Fields
    public UnityEvent onEnterScreen;
    public UnityEvent onExitScreen;
    private HealthSystem _healthSystem;
    #endregion

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Activator"))
        {
            onEnterScreen.Invoke();

            //активировать противника
            if (_healthSystem != null)
            {
                _healthSystem.Activation(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Deactivator"))
        {
            onExitScreen.Invoke();

            //диактивировать противника
            if (_healthSystem != null)
            {
                _healthSystem.Activation(false);
            }
        }
    }
}
