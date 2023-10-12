using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    [Header("Компоненты загрузки")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _progressBar;
    [SerializeField] private Text _loadText;
    #endregion

    #region Fields
    public static SceneLoader Instance;
    [Header("Основные параметры")]
    [SerializeField] private Vector3 _barScale = Vector3.one;
    #endregion

    #region Properties
    #endregion

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(AsyncChangeScene(sceneName));
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        DisablePanel();
    }

    private void DisablePanel()
    {
        _panel.SetActive(false);
    }

    private void UpdateBar(float value)
    {
        _panel.SetActive(true);
        _barScale.x = value;
        _progressBar.localScale = _barScale;
    }

    private IEnumerator AsyncChangeScene(string name)
    {
        yield return null;

        _loadText.text = "Loading";
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;

        Debug.Log("Pro :" + asyncOperation.progress);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01((float)asyncOperation.progress / 0.9f);
            UpdateBar(progress);

            if (Mathf.Approximately(asyncOperation.progress, 0.9f))
            {
                _loadText.text = "Tap to Start";

                if (Input.GetMouseButtonDown(0))
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }
            
            yield return null;
        }

        DisablePanel();
    }
}
