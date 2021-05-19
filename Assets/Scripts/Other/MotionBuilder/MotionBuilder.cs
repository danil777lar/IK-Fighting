using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;
using DG.Tweening;

public static partial class MotionBuilder
{
    private delegate Tween MotionMethod(Rigidbody2D t);
    private static Dictionary<PointerMotion, MotionMethod> motionMethods = new Dictionary<PointerMotion, MotionMethod>() 
    {
        {PointerMotion.ArmCalm, ArmCalm},
        {PointerMotion.LegCalm, LegCalm}
    };

    public static Tween GetTween(Rigidbody2D pointer, PointerMotion motion) =>
        motionMethods[motion].Invoke(pointer);
}
