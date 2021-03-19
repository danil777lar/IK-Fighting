using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePunch : Punch
{

    public SimplePunch(int hand) : base (hand) {}

    protected override void Start() 
    {
        base.Start();
        _currentHandPointer.SetMotion( new PunchStartMotion( _bodyPointer.transform, _controllInterface, _hand ) );
        _currentHandPointer.MotionFinishedEvent += OnPunchStart;

        KinematicsPointer otherHandPointer = _currentHandPointer == _frontArmPointer ? _backArmPointer : _frontArmPointer;
        otherHandPointer.SetMotion( new ArmCalmMotion( _bodyPointer.transform ) );
    }

    public override void Update(){}

    public void OnPunchStart() 
    {
        _currentHandPointer.SetMotion(new SimplePunchMotion(_bodyPointer.transform));
        _currentHandPointer.MotionFinishedEvent += OnPunchEnd;

        _bodyPointer.SetMotion(new BodySnatchMotion());
        _backLegPointer.SetMotion(null);
        _frontLegPointer.SetMotion(null);
    }

    public void OnPunchEnd() 
    {
        _stateHolder.SetState( new DefaultState() );
    }
}
