using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class StatsManager : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    public static StatsManager Instance;
    [Header("Основные параметры")]
    [SerializeField] private int _lives = 35;
    [SerializeField] private int _money;
    [Space]
    [Header("Списки настроек основных параметров прокачки")]
    [SerializeField] private List<ShootProfiles> _bulletUpgradeList = new();
    [SerializeField] private List<ShootProfiles> _missileUpgradeList = new();
    [SerializeField] private List<MegaBombData> _megaBombUpgradeList = new();
    [SerializeField] private List<LaserData> _laserUpgradeList = new();
    [SerializeField] private List<ShieldData> _shieldUpgradeList = new();
    [SerializeField] private List<float> _healthUpgradeList = new();
    private Dictionary<string, Medals> _achievementList = new Dictionary<string, Medals>();
    private Dictionary<string, DateTime> _statsTimer = new Dictionary<string, DateTime>();
    [Header("Обновление  данных таймера")]
    [SerializeField] private List<StatsUpgradeInfo> _stats = new();
    #endregion

    #region Properties
    public Dictionary<string, Medals> AchievementList => _achievementList;
    public Dictionary<string, DateTime> StatsTimer => _statsTimer;
    public int Money => _money;
    public int Lives => _lives;
    #endregion

    public void AddMoney(int value)
    {
        _money += value;

        //TO-DO: UI update system
    }

    public T GetStatsValue<T>(string statsName, List<T> statsList)
    {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].Name == statsName)
            {
                return statsList[_stats[i].Level - 1];
            }
        }

        return default(T);
    }

    public float[] GetUpgradeTime(string statsName)
    {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].Name == statsName)
            {
                return _stats[i].UpgradeTime;
            }
        }

        return null;
    }

    public StatsUpgradeInfo GetStats(string statsName)
    {
        for (int i = 0; i < _stats.Count; i++)
        {
            if (_stats[i].Name == statsName)
            {
                return _stats[i];
            }
        }

        return null;
    }

    public void SaveProgress()
    {
        SaveData saveData = new();

        saveData.SetLives(_lives);
        saveData.SetMoney(_money);
        saveData.SetAchievementList(_achievementList);
        saveData.SetStatsTimer(_statsTimer);
        saveData.SetStats(_stats);

        SaveSystem.Save(saveData);
    }

    public void LoadProgress()
    {
        SaveData loadData = SaveSystem.Load<SaveData>();

        _lives = loadData.Lives;
        _money = loadData.Money;
        _achievementList = loadData.AchievementList;
        _statsTimer = loadData.StatsTimer;
        _stats = loadData.Stats;

        UpdateItemDisplay();
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
    }

    private void UpdateItemDisplay()
    {
        UpgradeItem[] items = FindObjectsOfType<UpgradeItem>();

        foreach (UpgradeItem item in items)
        {
            item.UpdateItemDisplay();
        }
    }
}

[System.Serializable]
public class StatsUpgradeInfo
{
    #region Fields
    [SerializeField] private string _name;
    [SerializeField] private int _level;
    [SerializeField] private float[] _upgradeTime;
    #endregion

    #region Properties
    public string Name => _name;
    public int Level => _level;
    public float[] UpgradeTime => _upgradeTime;
    #endregion

    public void SetName(string name)
    {
        if (name != null)
        {
            _name = name;
        }
    }

    public void SetLevel()
    {
        _level++;
    }

    public void SetUpgradeTime(float[] upgradeTime)
    {
        if (upgradeTime != null)
        {
            _upgradeTime = upgradeTime;
        }
    }
}

[System.Serializable]
public class MegaBombData
{
    #region Fields
    [SerializeField] private float _radius;
    [SerializeField] private float _damage;
    #endregion

    #region Properties
    public float Radius => _radius;
    public float Damage => _damage;
    #endregion
}

[System.Serializable]
public class ShieldData
{
    #region Fields
    [SerializeField] private float _duration;
    #endregion

    #region Properties
    public float Duration => _duration;
    #endregion
}

[System.Serializable]
public class LaserData
{
    #region Fields
    [SerializeField] private float _duration;
    #endregion

    #region Properties
    public float Duration => _duration;
    #endregion
}

[System.Serializable]
public class SaveData
{
    #region Fields
    private int _lives;
    private int _money;
    private Dictionary<string, Medals> _achievementList = new Dictionary<string, Medals>();
    private Dictionary<string, DateTime> _statsTimer = new Dictionary<string, DateTime>();
    private List<StatsUpgradeInfo> _stats = new();
    #endregion

    #region Propertiers
    public int Lives => _lives;
    public int Money => _money;
    public Dictionary<string, Medals> AchievementList => _achievementList;
    public Dictionary<string, DateTime> StatsTimer => _statsTimer;
    public List<StatsUpgradeInfo> Stats => _stats;
    #endregion

    #region Public Methods to SaveData
    public void SetLives(int lives) => _lives = lives;

    public void SetMoney(int money) => _money = money;

    public void SetAchievementList(Dictionary<string, Medals> achievementList) => _achievementList = achievementList;

    public void SetStatsTimer(Dictionary<string, DateTime> statsTimer) => _statsTimer = statsTimer;

    public void SetStats(List<StatsUpgradeInfo> stats) => _stats = stats;
    #endregion
}
