using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HumanRescue : MonoBehaviour
{
    #region Constants
    private const string Player = "Player";
    private const string Enemy = "Enemy";
    private const string EnemyBullet = "Enemy bullet";
    #endregion

    #region Fields
    [SerializeField, Range(0f, 10f), Tooltip("Время на эвакуацию")]
    private float _evacuationTime = 5f;
    [SerializeField, Tooltip("UI элемент таймера")]
    private Image _timerUI;
    [SerializeField]
    private UnityEvent _onRescue;
    private GameObject _player;
    private readonly float _delayTime = 1.5f;
    private Coroutine _rescueCoroutine;
    #endregion

    private void Start()
    {
        LevelManager.instance.RegisterRescue();

        if(GameObject.FindGameObjectWithTag(Player))
            _player = GameObject.FindGameObjectWithTag(Player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           _rescueCoroutine = StartCoroutine(Rescuing(_evacuationTime));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_rescueCoroutine != null)
            {
                StopCoroutine(_rescueCoroutine);
                _rescueCoroutine = null;
            }

            _timerUI.fillAmount = 0f;
        }
    }

    #region Methods UI interface
    private void UpdateUI(float time)
    {
        if (_timerUI != null)
            _timerUI.fillAmount = InvertNormalTime(time, _evacuationTime);
    }

    private float InvertNormalTime(float value1, float value2)
    {
        return 1f - (value1 / value2);
    }
    #endregion

    private IEnumerator Rescuing(float time)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            UpdateUI(time);

            if (_player == null)
            {
                StopAllCoroutines();
                _timerUI.fillAmount = 0f;
            }

            yield return null;
        }

        LevelManager.instance.AddRescue();
        _onRescue.Invoke();
        Destroy(gameObject, _delayTime);
    }
}
