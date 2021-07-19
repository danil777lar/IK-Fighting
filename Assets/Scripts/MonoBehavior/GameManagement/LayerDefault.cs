using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDefault : MonoBehaviour
{
    #region Singleton

    private static LayerDefault _default = null;
    public static LayerDefault Default { get => _default; }

    #endregion

    private bool _isPlaying;
    public bool IsPlaying 
    {
        get => _isPlaying;
        set 
        {
            _isPlaying = value;
            if (value)
                OnPlayStart.Invoke();
            else
                OnPlayStop.Invoke();
        }
    }

    public Action OnPlayStart = () => { };
    public Action OnPlayStop = () => { };

    #region Events
    private void Awake()
    {
        Initialization();
    }
    #endregion

    public void Restart() 
    {

    }

    #region Iternal
    private void Initialization() 
    {
        _default = this;

        Resources.Load<DataGameMain>("DataGameMain").Init();
    }
    #endregion
}
