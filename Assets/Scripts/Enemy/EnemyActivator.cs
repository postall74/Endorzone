using UnityEngine;
using UnityEngine.Events;

public class EnemyActivator : MonoBehaviour
{
    #region Fields
    public UnityEvent onEnterScreen;
    public UnityEvent onExitScreen;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Activator"))
        {
            onEnterScreen.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Deactivator"))
        {
            onExitScreen.Invoke();
        }
    }
}
