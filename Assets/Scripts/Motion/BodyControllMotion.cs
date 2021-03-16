using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControllMotion : Motion
{
    readonly float TIME_OFFSET = 0.2f;
    const float Y_POSITION = -0.5f;

    private float _speed = 10f;
    private Rigidbody2D rb;

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
        rb = pointer.GetComponent<Rigidbody2D>();
    }

    public override void UpdateMotion() 
    {
        if (Input.GetKey("d")) rb.AddForce(new Vector2(_speed, rb.velocity.y));
        if (Input.GetKey("a")) rb.AddForce(new Vector2(-_speed, rb.velocity.y));

        float yPosition = Input.GetKey("s") ? -1f : Y_POSITION;
        float t = Time.deltaTime / TIME_OFFSET;
        Vector2 targetPosition = new Vector2(_pointer.position.x, yPosition);
        _pointer.position = Vector2.Lerp(_pointer.position, targetPosition, t);

        float a = rb.velocity.x * -4f;
        _pointer.rotation = Quaternion.Euler(0f, 0f, a);
    }
}
