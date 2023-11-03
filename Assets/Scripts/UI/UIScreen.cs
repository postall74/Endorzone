using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    public System.Action OnChanging = delegate { };
    //[Header("Параметры основных эффектов")]
    //[SerializeField] private float _appearSpeed = 0.5f;
    //[SerializeField] private float _hideSpeed = 0.5f;
    [Header("Группа холста")]
    private CanvasGroup _canvasGroup;
    #endregion

    #region Properties
    #endregion

    public void Initialization(bool show)
    {
        _canvasGroup.alpha = show ? 1f : 0f;
        _canvasGroup.interactable = show;
        _canvasGroup.blocksRaycasts = show;
    }

    public void Show()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        StartCoroutine(ModifyAlpha(1f/*, _appearSpeed*/));
    }

    public void Hide()
    {
        StartCoroutine(ModifyAlpha(0f/*, _hideSpeed*/, () =>
        {
            Initialization(false);
        }));
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator ModifyAlpha(float alphaTarget, /*float speed,*/ UnityAction callback = null)
    {
        while (_canvasGroup.alpha != alphaTarget)
        {
            //_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, alphaTarget, speed * Time.deltaTime);

            //if (_canvasGroup.alpha > alphaTarget - 0.1f && _canvasGroup.alpha < alphaTarget + 0.1f)
                _canvasGroup.alpha = alphaTarget;

            yield return null;
        }

        callback?.Invoke();
        OnChanging();
    }
}
