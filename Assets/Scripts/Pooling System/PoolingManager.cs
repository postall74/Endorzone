using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;

    #region Fields
    [SerializeField] private PoolItem[] _poolItems;

    private Dictionary<int, Queue<GameObject>> _poolQueue = new Dictionary<int, Queue<GameObject>>();
    private Dictionary<int, bool> _growCapabilityBool = new Dictionary<int, bool>();
    private Dictionary<int, Transform> _parents = new Dictionary<int, Transform>();
    #endregion

    #region Properties
    public PoolItem[] PoolItems => _poolItems;
    #endregion

    public GameObject UseObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        int objId = obj.GetInstanceID();

        GameObject temp = _poolQueue[objId].Dequeue();

        if (temp.activeInHierarchy)
        {
            if (_growCapabilityBool[objId])
            {
                _poolQueue[objId].Enqueue(temp);
                temp = Instantiate(obj, _parents[objId]);
                temp.transform.position = position;
                temp.transform.rotation = rotation;
                temp.SetActive(true);
            }
            else
                temp = null;
        }
        else
        {
            temp.transform.position = position;
            temp.transform.rotation = rotation;
            temp.SetActive(true);
        }

        _poolQueue[objId].Enqueue(temp);
        return temp;
    }

    public void ReturnObject(GameObject obj, float delay = 0f)
    {
        if (delay == 0f)
        {
            obj.SetActive(false);
        }
        else
        {
           StartCoroutine(DelayReturn(obj, delay));
        }
    }

    private void Awake()
    {
        Instance = this;
        PoolInit();
    }

    private void PoolInit()
    {
        GameObject poolGroup = new GameObject("Pool Group");

        for (int i = 0; i < _poolItems.Length; i++)
        {
            GameObject uniquePool = new GameObject(_poolItems[i]._object.name + " Group");
            uniquePool.transform.SetParent(poolGroup.transform);

            int objId = _poolItems[i]._object.GetInstanceID();
            _poolItems[i]._object.SetActive(false);

            _poolQueue.Add(objId, new Queue<GameObject>());
            _growCapabilityBool.Add(objId, _poolItems[i]._isGrowCapability);
            _parents.Add(objId, uniquePool.transform);

            for (int j = 0; j < _poolItems[i]._amount; j++)
            {
                GameObject temp = Instantiate(_poolItems[i]._object, uniquePool.transform);
                _poolQueue[objId].Enqueue(temp);
            }
        }
    }

    private IEnumerator DelayReturn(GameObject obj, float delay = 0f)
    {
        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        obj.SetActive(false);

    }
}

[System.Serializable]
public class PoolItem
{
    #region Fields
    public GameObject _object;
    public int _amount;
    public bool _isGrowCapability;
    #endregion
}
