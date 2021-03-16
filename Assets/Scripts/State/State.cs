using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    protected KinematicsPointer _bodyPointer;
    protected KinematicsPointer _frontArmPointer;
    protected KinematicsPointer _backArmPointer;
    protected KinematicsPointer _frontLegPointer;
    protected KinematicsPointer _backLegPointer;

    public virtual void Init(KinematicsPointer body, KinematicsPointer frontArm, KinematicsPointer backArm, KinematicsPointer frontLeg, KinematicsPointer backLeg)
    {
        _bodyPointer = body;
        _frontArmPointer = frontArm;
        _backArmPointer = backArm;
        _frontLegPointer = frontLeg;
        _backLegPointer = backLeg;
        Start();
    }

    protected abstract void Start();

    public abstract void Update();
}
