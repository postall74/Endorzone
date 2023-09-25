using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    #region Fields
    [Header("Параметры для настройки волн врагов")]
    [SerializeField] private int _numWaves = 1;
    [SerializeField] private float _intervalBetweenEnemy = 0.5f;
    [SerializeField] private float _removeAfter = 2f;
    [Space, Header("Список объектов для создания волны")]
    [SerializeField] private List<GameObject> _children = new();

    private GameObject _mainChild;
    private WaitForSeconds _interval;
    private WaitForSeconds _disableAfter;
    #endregion

    private void Start()
    {
        _interval = new WaitForSeconds(_intervalBetweenEnemy);
        _disableAfter = new WaitForSeconds(_removeAfter);
        Initialization();

        StartCoroutine(StartWaves());
        StartCoroutine(CheckCombo());
    }

    /// <summary>
    /// TO-DO: 
    /// Метод инициализации пула объектов и активации/дезактивации объектов из пула, в виде дочерних объектов пула.
    /// </summary>
    private void Initialization()
    {
        _mainChild = transform.GetChild(0).gameObject;
        Vector3 position = _mainChild.transform.position;
        _mainChild.SetActive(false);
        _children.Add(_mainChild);

        for (int i = 1; i < _numWaves; i++)
        {
            GameObject temp = Instantiate(_mainChild, position, _mainChild.transform.rotation);
            _children.Add(temp);
            _children[i].transform.SetParent(transform);
            _children[i].SetActive(false);
        }
    }

    #region Methods Coroutines Emeny waves
    private IEnumerator StartWaves()
    {
        int index = 0;

        while (index < _numWaves)
        {
            _children[index].SetActive(true);
            StartCoroutine(DisableChild(_children[index]));
            index++;
            yield return _interval;
        }
    }

    private IEnumerator DisableChild(GameObject obj)
    {
        yield return _disableAfter;

        if (obj != null)
            obj.SetActive(false);
    }

    private IEnumerator CheckCombo()
    {
        yield return _disableAfter;

        if (transform.childCount == 0)
            UnityEngine.Debug.Log("Enemy combo kill");
        else
            UnityEngine.Debug.Log("Chain lost");
    }
    #endregion
}
