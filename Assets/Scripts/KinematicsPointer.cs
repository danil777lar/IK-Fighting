using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Rigidbody2D ) )]

public class KinematicsPointer : MonoBehaviour
{
    private Stack<Motion> _motionStack = new Stack<Motion>();

    public delegate void MotionFinished();
    public event MotionFinished MotionFinishedEvent;

    public string MotionName;

    private void FixedUpdate()
    {
        if (_motionStack.Count <= 0) return;

        if (_motionStack.Peek().IsFinished)
        {
            _motionStack.Pop();
            _motionStack.Peek().Init(this);
            if (MotionFinishedEvent != null) MotionFinishedEvent();
        }
        else 
        {
            _motionStack.Peek().UpdateMotion();
            MotionName = "";
            foreach (Motion m in _motionStack)
                MotionName += " " + m.GetType().ToString();
        }
    }

    public void PushMotion(Motion motion)
    {
        motion.Init(this);
        _motionStack.Push(motion);
    }
}
