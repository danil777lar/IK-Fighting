using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControllMotion : Motion
{
    readonly float TIME_OFFSET = 0.2f;
    const float Y_POSITION = -0.5f;

    private float _speed = 10f;
    private Rigidbody2D _rb;

    private IControll _controllInterface;

    public BodyControllMotion(IControll controll) => _controllInterface = controll; 

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
        _rb = pointer.GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    public override void UpdateMotion() 
    {
        float yPosition = _controllInterface.GetMoveDown() ? -1f : Y_POSITION;
        float t = Time.deltaTime / TIME_OFFSET;
        Vector2 targetPosition = new Vector2(_pointer.position.x, yPosition);
        _pointer.position = Vector2.Lerp(_pointer.position, targetPosition, t);

        if (_pointer.position.y >= -1.5f)
        {
            if (_controllInterface.GetMoveRight()) _rb.AddForce(new Vector2(_speed, _rb.velocity.y));
            if (_controllInterface.GetMoveLeft()) _rb.AddForce(new Vector2(-_speed, _rb.velocity.y));
            if (_controllInterface.GetJump())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
                _rb.drag = 0f;
                _rb.gravityScale = 1f;
                IsFinished = true;
            }
        }
    }
}
