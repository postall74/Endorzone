using UnityEngine;

public class CreateObject : MonoBehaviour
{
    #region Fields
    private Vector3 _position;
    [SerializeField] private GameObject _objectToCreate;
    [SerializeField] private int _createAmount = 1;
    [Header("Свойства автоматического удаления объектов")]
    [SerializeField] private bool _isAutoDestroy;
    [SerializeField] private float _timeToDestroy;
    #endregion

    public void Create()
    {
        _position = transform.position;
        _position.y = 0f;

        for (int i = 0; i < _createAmount; i++)
        {
            GameObject temp = PoolingManager.Instance.UseObject(_objectToCreate, _position, Quaternion.identity);

            if (_isAutoDestroy)
            {
                PoolingManager.Instance.ReturnObject(temp, _timeToDestroy);
            }
        }
    }
}
