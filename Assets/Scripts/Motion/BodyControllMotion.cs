using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControllMotion : Motion
{
    readonly float TIME_OFFSET = 0.2f;
    const float Y_POSITION = -0.5f;

    private float _speed = 10f;
    private Rigidbody2D _rb;

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
        _rb = pointer.GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    public override void UpdateMotion() 
    {
        if (Input.GetKey("d")) _rb.AddForce(new Vector2(_speed, _rb.velocity.y));
        if (Input.GetKey("a")) _rb.AddForce(new Vector2(-_speed, _rb.velocity.y));

        float yPosition = Input.GetKey("s") ? -1f : Y_POSITION;
        float t = Time.deltaTime / TIME_OFFSET;
        Vector2 targetPosition = new Vector2(_pointer.position.x, yPosition);
        _pointer.position = Vector2.Lerp(_pointer.position, targetPosition, t);

        float a = _rb.velocity.x * -4f;
        _pointer.rotation = Quaternion.Euler(0f, 0f, a);

        if (Input.GetKey("w")) 
        {
            _rb.AddForce( new Vector2(0f, 15f), ForceMode2D.Impulse );
            _rb.gravityScale = 1f;
            IsFinished = true;
        }
    }
}
