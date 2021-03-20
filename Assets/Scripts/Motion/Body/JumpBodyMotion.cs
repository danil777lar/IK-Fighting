using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBodyMotion : Motion
{
    private Rigidbody2D _rb;

    public override void Init(KinematicsPointer pointer)
    {
        base.Init(pointer);
        _rb = pointer.GetComponent<Rigidbody2D>();

        _rb.drag = 0f;
        _rb.gravityScale = 1f;
    }

    public override void UpdateMotion() 
    {
        if (_pointerTransform.position.y <= BodyControllMotion.STAND_HEIGHT && _rb.velocity.y < 0f)
            IsFinished = true;
    }
}
