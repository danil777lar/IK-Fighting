using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Motion
{
    protected Transform _pointer;

    public bool IsFinished { get; protected set; }


    public virtual void Init(Transform pointer) 
    {
        _pointer = pointer;
    }

    public abstract void UpdateMotion();
}
