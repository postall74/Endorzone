using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    [Header("Основные параметры экранов UI")]
    [SerializeField] private UIScreen[] _screenArray;
    private UIScreen _currentUIScreen;
    private UIScreen _previousUIScreen;
    private bool _isChange = false;
    #endregion

    #region Properties
    #endregion

    public void ChangeScreen(UIScreen newScreen)
    {
        if (_isChange || _currentUIScreen == newScreen)
            return;

        if (newScreen)
        {
            _isChange = true;

            newScreen.OnChanging -= DoneSwitch;
            newScreen.OnChanging += DoneSwitch;

            if (_currentUIScreen)
            {
                _previousUIScreen = _currentUIScreen;
                _previousUIScreen.Hide();
            }

            _currentUIScreen = newScreen;
            _currentUIScreen.Show();

        }
    }

    private void Start()
    {
        _screenArray = GetComponentsInChildren<UIScreen>(true);

        Initialization(0);
    }

    private void Initialization(int defaultUI)
    {
        for (int i = 0; i < _screenArray.Length; i++)
        {
            if (i == defaultUI)
            {
                _screenArray[i].Initialization(true);
                _currentUIScreen = _screenArray[i];
            }
            else
            {
                _screenArray[i].Initialization(false);
            }
        }
    }

    private void DoneSwitch()
    {
        _isChange = false;
    }
}
