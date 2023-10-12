using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    #region Constants
    private const string MainMenu = "Menu";
    #endregion

    #region Components
    [Header("Основные компоненты")]
    [SerializeField] private Button _buttonExit;
    [Header("Достижения")]
    [SerializeField] private Text _textKill;
    [SerializeField] private Text _textRescue;
    [SerializeField] private Text _textUntouched;
    #endregion

    #region Fields
    [Header("Основные параметры")]
    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;
    private WaitForSeconds _interval = new WaitForSeconds(0.5f);
    #endregion

    #region Properties
    #endregion

    private void OnEnable()
    {
        _buttonExit.onClick.AddListener(BackToMenu);
        _buttonExit.interactable = false;

        StartCoroutine(ShowAchievement());
    }

    private void BackToMenu()
    {
        SceneLoader.Instance.ChangeScene(MainMenu);
    }

    private IEnumerator ShowAchievement()
    {
        yield return _interval;

        _textKill.color = LevelManager.Instance.Medals.Kill ? _enableColor : _disableColor;
        _textRescue.color = LevelManager.Instance.Medals.Rescue ? _enableColor : _disableColor;
        _textUntouched.color = LevelManager.Instance.Medals.Untouched ? _enableColor : _disableColor;

        yield return _interval;

        _buttonExit.interactable = true;
    }
}
