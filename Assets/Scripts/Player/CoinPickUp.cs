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

            StatsManager.Instance.AddMoney(1);
        }
    }
}
