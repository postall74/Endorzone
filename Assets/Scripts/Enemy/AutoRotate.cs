using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    #region Fields
    [Header("Параметры поворота турели")]
    [SerializeField] private Vector3 _rotateSpeed;
    [SerializeField] private bool _isEndless;
    [SerializeField] private bool _onStart;
    [Header("Целевое вращение")]
    [SerializeField] private Vector3 _angleRotation;
    [SerializeField] private float _speed;
    #endregion

    public void StartRotate()
    {
        StartCoroutine(DoRotate());
    }

    private void Start()
    {
        if (_onStart)
        {
            StartCoroutine(DoRotate());
        }
    }

    private void OnDisable()
    {
        StopCoroutine(DoRotate());
    }

    #region Methods Rotation objects
    private IEnumerator DoRotate()
    {
        Quaternion targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles + _angleRotation);


        if (_isEndless)
        {
            while (_isEndless)
            {
                transform.Rotate(_rotateSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (transform.rotation != targetRotation)
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, _speed * Time.deltaTime);
                yield return null;
            }
        }
    }
    #endregion
}
