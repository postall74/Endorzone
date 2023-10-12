using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    [Header("Обновление объектов меню")]
    [SerializeField] private string _statsName;
    [SerializeField] private string _itemName;
    [SerializeField] private Text _itemNameText;
    [SerializeField] private Text _itemByuText;
    [SerializeField] private Slider _itemLevelBar;
    [SerializeField] private Button _buttonByu;
    [Header("Настройки стоимости вещей: ")]
    [SerializeField] private int[] _pricesLevel;
    private StatsUpgradeInfo _stats;
    private bool _isUpgrading;
    #endregion

    #region Properties
    #endregion

    public void ByuUpgrade()
    {
        if (StatsManager.Instance.Money >= _pricesLevel[_stats.Level])
        {
            DialogManager.Instance.ShowDialog("Do you really want to upgrade " + _statsName, () =>
            {
                //do upgrade this
                StatsManager.Instance.AddMoney(-_pricesLevel[_stats.Level]);
                StatsManager.Instance.StatsTimer.Add(_statsName, DateTime.Now.AddMinutes(StatsManager.Instance.GetUpgradeTime(_statsName)[_stats.Level]));

                //start the coroutine
                StartCoroutine(DoUpgrade());
            });
            
            //Debug.Log("Upgrading " + _statsName);
        }
        else
        {
            //show message not enough money
            //Debug.Log("Not enough money");
            DialogManager.Instance.ShowMessage("You don't have enough money to upgrade " + _statsName);
        }
    }

    public void CheckUpgradeStatus()
    {
        if (StatsManager.Instance.StatsTimer.ContainsKey(_statsName))
        {
            if (DateTime.Now < StatsManager.Instance.StatsTimer[_statsName])
            {
                StartCoroutine(DoUpgrade());
            }
            else
            {
                IncreaseStat();
            }
        }
    }

    public void UpdateItemDisplay()
    {
        _stats = StatsManager.Instance.GetStats(_statsName);
        _itemLevelBar.value = _stats.Level;

        if (_stats.Level == _pricesLevel.Length)
        {
            _itemByuText.text = "MAX";
            return;
        }

        _itemByuText.text = _pricesLevel[_stats.Level].ToString();
        CheckUpgradeStatus();
    }

    private void Start()
    {
        _stats = StatsManager.Instance.GetStats(_statsName);
        _itemNameText.text = _itemName;
        _itemByuText.text = _pricesLevel[_stats.Level].ToString();
        _itemLevelBar.value = _stats.Level;
        _buttonByu.onClick.AddListener(ByuUpgrade);
        UpdateItemDisplay();
    }

    private IEnumerator DoUpgrade()
    {
        _isUpgrading = true;

        TimeSpan timeRemaining = StatsManager.Instance.StatsTimer[_statsName] - DateTime.Now;

        while (timeRemaining.TotalSeconds > 0f)
        {
            timeRemaining = StatsManager.Instance.StatsTimer[_statsName] - DateTime.Now;
            _itemByuText.text = string.Format("{0:00}:{1:00}", timeRemaining.Minutes, timeRemaining.Seconds);
            yield return null;
        }
        
        //do upgrade this
        _isUpgrading = false;
        IncreaseStat();
    }

    private void IncreaseStat()
    {
        _stats.SetLevel();

        if (_isUpgrading)
        {
            StopAllCoroutines();
            _isUpgrading = false;
        }

        _itemByuText.text = _pricesLevel[_stats.Level].ToString();
        _itemLevelBar.value = _stats.Level;
        StatsManager.Instance.StatsTimer.Remove(_statsName);
        DialogManager.Instance.ShowMessage("Finish upgrading " + _statsName);
    }
}
