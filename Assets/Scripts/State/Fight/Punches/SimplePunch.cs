using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePunch : Punch
{

    public SimplePunch(int hand, Transform root) : base (hand, root) {}

    protected override void Start() 
    {
        base.Start();
        _currentHandPointer.SetMotion( new PunchStartMotion( _armRoot.transform, _controllInterface, _hand ) );
        _currentHandPointer.MotionFinishedEvent += OnPunchStart;

        KinematicsPointer otherHandPointer = _currentHandPointer == _frontArmPointer ? _backArmPointer : _frontArmPointer;
        otherHandPointer.SetMotion( new ArmCalmMotion( _bodyPointer.transform ) );
    }

    public override void Update(){}

    public void OnPunchStart() 
    {
        _currentHandPointer.MotionFinishedEvent -= OnPunchStart;
        _currentHandPointer.SetMotion(new SimplePunchMotion(_armRoot.transform));
        _currentHandPointer.MotionFinishedEvent += OnPunchEnd;
    }

    public void OnPunchEnd() 
    {
        _stateHolder.SetState( new DefaultState() );
    }
}
