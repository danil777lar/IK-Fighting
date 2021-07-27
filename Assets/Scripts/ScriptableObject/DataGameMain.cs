using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DataGameMain")]
public class DataGameMain : ScriptableObject
{
    #region Singleton

    private static DataGameMain _default = null;
    public static DataGameMain Default { get => _default; }

    #endregion

    public float personStandHeight = 2f;
    public float personStepLenght = 2f;
    public float bloodMaxRad = 5;
    public float bloodMinPaintForce = 0.5f;

    public void Init() 
    {
        _default = this;
    }
}
