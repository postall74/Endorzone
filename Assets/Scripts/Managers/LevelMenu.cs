using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    [Header("Основные компоненты")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Text _killText;
    [SerializeField] private Text _rescueText;
    [SerializeField] private Text _untouchedText;

    #endregion

    #region Fields
    [Header("Основные параметры")]
    [Header("Название сцены")]
    [SerializeField] private string _sceneTarget;
    [Header("Параметры цвета достижений")]
    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;
    private Medals _sceneMedal;
    #endregion

    #region Properties
    #endregion

    public void UpdateMenu()
    {
        if (StatsManager.Instance.AchievementList.ContainsKey(_sceneTarget))
            _sceneMedal = StatsManager.Instance.AchievementList[_sceneTarget];

        if (_sceneMedal == null)
        {
            Debug.Log("Err medals load");
            return;
        }

        _killText.color = _sceneMedal.Kill ? _enableColor : _disableColor;
        _rescueText.color = _sceneMedal.Rescue ? _enableColor : _disableColor;
        _untouchedText.color = _sceneMedal.Untouched ? _enableColor : _disableColor;
    }

    private void Start()
    {
        UpdateMenu();
        _playButton.onClick.AddListener(GoToLevel);
    }

    private void GoToLevel()
    {
        if (_sceneTarget == null)
        {
            Debug.Log("Err scene load!");
            return;
        }

        SceneLoader.Instance.ChangeScene(_sceneTarget);
    }
}
