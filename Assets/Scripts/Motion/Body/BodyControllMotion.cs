using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControllMotion : Motion
{
    public static readonly float STAND_HEIGHT = -0.5f;
    public static readonly float SIT_HEIGHT = -1f;

    const float TIME_OFFSET = 0.2f;

    private float _speed = 10f;
    private Rigidbody2D _rb;

    private IControll _controllInterface;
    
    private KinematicsPointer _frontLeg;
    private KinematicsPointer _backLeg;

    public BodyControllMotion(IControll controll, KinematicsPointer frontLeg, KinematicsPointer backLeg) 
    {
        _controllInterface = controll;
        _frontLeg = frontLeg;
        _backLeg = backLeg;
    } 

    public override void Init(KinematicsPointer pointer)
    {
        base.Init(pointer);
        _rb = pointer.GetComponent<Rigidbody2D>();

        _rb.drag = 1f;
        _rb.gravityScale = 0f;
    }

    public override void UpdateMotion() 
    {
        float yPosition = _controllInterface.GetMoveDown() ? SIT_HEIGHT : STAND_HEIGHT;
        float t = Time.deltaTime / TIME_OFFSET;
        Vector2 targetPosition = new Vector2(_pointerTransform.position.x, yPosition);
        _pointerTransform.position = Vector2.Lerp(_pointerTransform.position, targetPosition, t);

        if (_pointerTransform.position.y >= -1.5f)
        {
            if (_controllInterface.GetMoveRight()) _rb.AddForce(new Vector2(_speed, _rb.velocity.y));
            if (_controllInterface.GetMoveLeft()) _rb.AddForce(new Vector2(-_speed, _rb.velocity.y));
            if (_controllInterface.GetJump())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
                _frontLeg.PushMotion(new JumpLegMotion(_pointer.transform));
                _backLeg.PushMotion(new JumpLegMotion(_pointerTransform));
                _pointer.PushMotion( new JumpBodyMotion() );
            }
        }
    }

}
