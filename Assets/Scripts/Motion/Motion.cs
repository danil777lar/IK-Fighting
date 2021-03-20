using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Motion
{
    protected Transform _pointerTransform;
    protected KinematicsPointer _pointer;

    public bool IsFinished { get; protected set; }


    public virtual void Init(KinematicsPointer pointer) 
    {
        _pointerTransform = pointer.transform;
        _pointer = pointer;
    }

    public abstract void UpdateMotion();
}
