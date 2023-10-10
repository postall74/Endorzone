using UnityEngine;

public class DebugButton : MonoBehaviour
{
    #region Constants
    #endregion

    #region Components
    #endregion

    #region Fields
    #endregion

    #region Properties
    #endregion

    public void Save()
    {
        StatsManager.Instance.SaveProgress();
    }

    public void Load()
    {
        StatsManager.Instance.LoadProgress();
    }
}
