using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected KinematicsPointer _bodyPointer;
    protected KinematicsPointer _frontArmPointer;
    protected KinematicsPointer _backArmPointer;
    protected KinematicsPointer _frontLegPointer;
    protected KinematicsPointer _backLegPointer;

    protected StateHolder _stateHolder;
    protected IControll _controllInterface;

    public virtual void Init(StateHolder stateHolder, IControll controll, KinematicsPointer body, KinematicsPointer frontArm,
        KinematicsPointer backArm, KinematicsPointer frontLeg, KinematicsPointer backLeg)
    {
        _bodyPointer = body;
        _frontArmPointer = frontArm;
        _backArmPointer = backArm;
        _frontLegPointer = frontLeg;
        _backLegPointer = backLeg;

        _stateHolder = stateHolder;
        _controllInterface = controll;

        Start();
    }

    protected abstract void Start();

    public abstract void Update();
}
