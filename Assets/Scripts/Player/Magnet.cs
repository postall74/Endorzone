using UnityEngine;

public class Magnet : MonoBehaviour
{
    #region Fields
    [Header("Основные параметры")]
    [Range(0f, 5f)]
    [SerializeField] private float _magnetPower;
    [SerializeField] private float _magnetRange = 2f;
    #endregion

    #region Properties
    public float MagnetPower => _magnetPower;
    public float MagnetRange => _magnetRange;
    #endregion
}
