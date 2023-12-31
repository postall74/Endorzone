using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{

    #region Fields
    public static LevelManager Instance;

    [SerializeField] private Medals _medals = new Medals();

    [SerializeField] private int _totalEnemy;
    [SerializeField] private int _enemyKilled;
    [SerializeField] private int _totalRescue;
    [SerializeField] private int _humanRescue;
    [SerializeField] private UnityEvent _onGameEnd;

    private string _sceneName;
    #endregion

    #region Properties
    public UnityEvent OnGameEnd => _onGameEnd;
    public Medals Medals => _medals;
    #endregion

    public void RegisterEnemy()
    {
        _totalEnemy++;
    }

    public void RegisterRescue()
    {
        _totalRescue++;
    }

    public void AddEnemyKill(string name)
    {
        _enemyKilled++;
        Debug.Log(name + " is killed");
    }

    public void AddRescue()
    {
        _humanRescue++;
    }

    public void PlayerHit()
    {
        _medals.SetUntouched(false);
    }

    public void GameEnd()
    {
        StartCoroutine(CountDelay());
    }

    private void Awake()
    {
        Instance = this;
        _medals.SetUntouched(true);
        _sceneName = SceneManager.GetActiveScene().name;
    }

    private IEnumerator CountDelay()
    {
        yield return new WaitForSeconds(0.25f);

        if (_enemyKilled >= _totalEnemy)
        {
            _medals.SetKill(true);
        }

        if (_humanRescue >= _totalRescue)
        {
            _medals.SetRescue(true);
        }

        StatsManager.Instance.AddMedals(_sceneName , _medals);

        _onGameEnd.Invoke();
    }
}

[System.Serializable]
public class Medals
{
    #region Fields
    [SerializeField] private bool _rescue;
    [SerializeField] private bool _kill;
    [SerializeField] private bool _untouched;
    #endregion

    #region Properties
    public bool Rescue => _rescue;
    public bool Kill => _kill;
    public bool Untouched => _untouched;
    #endregion

    public void SetRescue(bool rescue) => _rescue = rescue;

    public void SetKill(bool kill) => _kill = kill;

    public void SetUntouched(bool untouched) => _untouched = untouched;
}
