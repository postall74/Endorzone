using UnityEngine;
using UnityEngine.Events;

public class CoinPickUp : MonoBehaviour
{
    #region Fields
    [SerializeField] private UnityEvent _onPickedUp;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            PoolingManager.Instance.ReturnObject(other.gameObject);
            _onPickedUp.Invoke();
            // TO-DO add some sort of coin counter
        }
    }
}
