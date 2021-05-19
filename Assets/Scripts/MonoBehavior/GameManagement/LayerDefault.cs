using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDefault : MonoBehaviour
{
    #region Singleton

    private static LayerDefault _default = null;
    public static LayerDefault Default { get => _default; }

    #endregion

    #region Events

    private void Awake()
    {
        Initialization();
    }

    #endregion

    #region Iternal

    private void Initialization() 
    {
        _default = this;

        Resources.Load<DataGameMain>("DataGameMain").Init();
    }

    #endregion
}
