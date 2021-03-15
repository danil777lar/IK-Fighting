using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicsPointer : MonoBehaviour
{
    private Motion _motion;

    public delegate void MotionFinished();
    public event MotionFinished MotionFinishedEvent;

    private void Update()
    {
        if (_motion != null) 
        {
            if (_motion.IsFinished)
            {
                _motion = null;
                MotionFinishedEvent();
            }
            else _motion.UpdateMotion();
        }
    }

    public void SetMotion(Motion motion)
    {
        motion.Init(transform);
        _motion = motion;
    }
}
