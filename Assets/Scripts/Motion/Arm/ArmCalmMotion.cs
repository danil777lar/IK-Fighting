using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCalmMotion : Motion
{
    private Transform _bodyPointer;

    private Vector3 _positionOffset = new Vector3(1.5f, 0.5f);
    private float _timeOffset = 0.4f;

    public override void Init(KinematicsPointer pointer)
    {
        base.Init(pointer);
        Rigidbody2D rb = pointer.GetComponent<Rigidbody2D>();
        rb.drag = 1f;
        rb.gravityScale = 0f;
    }

    public ArmCalmMotion(Transform bodyPointer) 
    {
        _bodyPointer = bodyPointer;

        _timeOffset *= Random.Range(0.5f, 1.5f);
        _positionOffset.y += Random.Range(-0.5f, 2.5f);

    }

    public override void UpdateMotion()
    {
        float t = Time.deltaTime / _timeOffset;
        Vector2 targetPosition = _bodyPointer.localPosition + _positionOffset;
        _pointerTransform.localPosition = Vector2.Lerp( _pointerTransform.localPosition, targetPosition, t); 
    }
}
