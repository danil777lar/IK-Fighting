using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IControll))]

public class StateHolder : MonoBehaviour
{
    [SerializeField] private KinematicsPointer _bodyPointer;
    [SerializeField] private KinematicsPointer _frontArmPointer;
    [SerializeField] private KinematicsPointer _backArmPointer;
    [SerializeField] private KinematicsPointer _frontLegPointer;
    [SerializeField] private KinematicsPointer _backLegPointer;

    private IControll _controllInterface;

    void Start()
    {
        _controllInterface = GetComponent<IControll>();

        _bodyPointer.PushMotion( new BodyControllMotion(_controllInterface, _backLegPointer, _frontLegPointer) );
        _frontArmPointer.PushMotion( new ArmCalmMotion(_bodyPointer.transform) );
        _backArmPointer.PushMotion( new ArmCalmMotion(_bodyPointer.transform) );
        _frontLegPointer.PushMotion( new StepMotion(_bodyPointer.transform, _backLegPointer.transform, true) );
        _backLegPointer.PushMotion( new StepMotion(_bodyPointer.transform, _frontLegPointer.transform, true) );
    }

}
