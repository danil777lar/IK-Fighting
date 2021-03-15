using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Motion
{
    protected Transform _pointer;

    protected Vector2 _startPosition;
    protected float _startTime;

    public bool IsFinished { get; protected set; }


    public virtual void Init(Transform pointer) 
    {
        _pointer = pointer;
        _startPosition = pointer.position;
        _startTime = Time.time;
    }

    public abstract void UpdateMotion();
}
