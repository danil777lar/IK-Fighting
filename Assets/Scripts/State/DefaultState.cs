using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/States/DefaultState")]
public class DefaultState : State
{
    protected override void Start()
    {
        _bodyPointer.SetMotion( new BodyControllMotion() );
        _frontLegPointer.SetMotion( new StepMotion(_bodyPointer.transform, _backLegPointer.transform) );
        _backLegPointer.SetMotion(new StepMotion(_bodyPointer.transform, _frontLegPointer.transform));
    }

    public override void Update() { }
}
