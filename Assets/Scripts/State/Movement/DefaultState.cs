using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : State
{
    private Rigidbody2D _bodyRb;

    protected override void Start()
    {
        _bodyPointer.SetMotion( new BodyControllMotion(_controllInterface) );
        _frontLegPointer.SetMotion( new StepMotion(_bodyPointer.transform, _backLegPointer.transform, true) );
        _backLegPointer.SetMotion( new StepMotion(_bodyPointer.transform, _frontLegPointer.transform, false) );
        _frontArmPointer.SetMotion( new ArmCalmMotion(_bodyPointer.transform) );
        _backArmPointer.SetMotion(new ArmCalmMotion(_bodyPointer.transform));

        _bodyRb = _bodyPointer.GetComponent<Rigidbody2D>();
    }

    public override void Update() 
    {
        if (_bodyRb.transform.position.y > -0.4f)
            _stateHolder.SetState(new JumpState());
    }
}
