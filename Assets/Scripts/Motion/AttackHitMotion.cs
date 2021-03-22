using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitMotion : Motion
{
    private Rigidbody2D _rb;

    private float _duration = 1f;
    private float _startTime;
    private Vector2 _force;

    public AttackHitMotion(Vector2 force) 
    {
        _force = force;
    }

    public override void Init(KinematicsPointer pointer)
    {
        base.Init(pointer);
        _startTime = Time.time;

        _rb = pointer.GetComponent<Rigidbody2D>();
        _rb.gravityScale = 1f;
        _rb.drag = 2f;
        _rb.AddForce(_force, ForceMode2D.Impulse);
    }

    public override void UpdateMotion()
    {
        if (_startTime + _duration >= Time.time) IsFinished = true;
    }
}
