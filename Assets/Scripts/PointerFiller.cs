using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IControll))]

public class PointerFiller : MonoBehaviour
{

    public static readonly int BodyPointer = 0;
    public static readonly int FrontArm = 1;
    public static readonly int BackArm = 2;
    public static readonly int FrontLeg = 3;
    public static readonly int BackLeg = 4;


    [SerializeField] private KinematicsPointer[] _pointers;
   
    private IControll _controllInterface;

    void Start()
    {
        _controllInterface = GetComponent<IControll>();

        _pointers[BodyPointer].PushMotion( new BodyControllMotion(_controllInterface, _pointers[BackLeg], _pointers[FrontLeg]) );
        _pointers[FrontArm].PushMotion( new ArmCalmMotion(_pointers[BodyPointer].transform) );
        _pointers[BackArm].PushMotion( new ArmCalmMotion(_pointers[BodyPointer].transform) );
        _pointers[FrontLeg].PushMotion( new StepMotion(_pointers[BodyPointer].transform, _pointers[BackLeg].transform, true) );
        _pointers[BackLeg].PushMotion( new StepMotion(_pointers[BodyPointer].transform, _pointers[FrontLeg].transform, false) );
    }

    public void FillPointers(Dictionary<int, Motion[]> motionDictionary) 
    {
        for (int i = 0; i < _pointers.Length; i++) 
        {
            if (motionDictionary.ContainsKey(i))
                _pointers[i].PushMotion(motionDictionary[i]);
        }
    }

}
