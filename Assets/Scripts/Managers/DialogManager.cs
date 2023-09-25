using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    public static DialogManager Instance;
    [Header("Основные параметры")]
    [SerializeField] private Text _dialogMessage;
    [SerializeField] private Text _yesText;
    [SerializeField] private Text _noText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private GameObject _panel;
    #endregion

    #region Properties
    #endregion

    public void ShowDialog(string message, UnityAction yesAction, UnityAction noAction = null, string yesText = "Yes", string noText = "No")
    {
        _noButton.gameObject.SetActive(true);
        _dialogMessage.text = message;
        _yesText.text = yesText;
        _noText.text = noText;

        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();

        if (noAction != null)
        {
            _noButton.onClick.AddListener(noAction);
        }

        _noButton.onClick.AddListener(DisablePanel);

        if (yesAction != null)
        {
            _yesButton.onClick.AddListener(yesAction);
            _yesButton.onClick.AddListener(DisablePanel);
        }

        _panel.SetActive(true);
    }

    public void ShowMessage(string message)
    {
        _noButton.gameObject.SetActive(false);
        _dialogMessage.text = message;
        _yesText.text = "Ok";
        _yesButton.onClick.RemoveAllListeners();
        _yesButton.onClick.AddListener(DisablePanel);
        _panel.SetActive(true);
    }

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    private void DisablePanel()
    {
        _panel.SetActive(false);
    }
}
