using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCalmMotion : Motion
{
    readonly Vector3 POSITION_OFFSET = new Vector3(1.5f, 0.5f);
    readonly float TIME_OFFSET = 0.4f;

    private Transform _bodyPointer;
    private float _offsetScale;
    

    public ArmCalmMotion(Transform bodyPointer) 
    {
        _bodyPointer = bodyPointer;
        _offsetScale = Random.Range(0.5f, 1.2f);
    }

    public override void UpdateMotion()
    {
        float t = Time.deltaTime / TIME_OFFSET * _offsetScale;
        Vector2 targetPosition = _bodyPointer.localPosition + POSITION_OFFSET * _offsetScale;
        _pointer.localPosition = Vector2.Lerp( _pointer.localPosition, targetPosition, t); 
    }
}
