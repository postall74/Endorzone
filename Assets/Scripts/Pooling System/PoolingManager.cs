using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;

    public GameObject UseObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        GameObject temp = Instantiate(obj, position, rotation);
        temp.SetActive(true);
        return temp;
    }

    public void ReturnObject(GameObject obj, float delay = 0f)
    {
        Destroy(obj, delay);
    }

    private void Awake()
    {
        Instance = this;
    }
}
