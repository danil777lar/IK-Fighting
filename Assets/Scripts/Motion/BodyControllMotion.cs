using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControllMotion : Motion
{
    private Rigidbody2D rb;

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
        rb = pointer.GetComponent<Rigidbody2D>();
    }

    public override void UpdateMotion() 
    {
        if (Input.GetKey("d")) rb.velocity = new Vector2(5f, rb.velocity.y);
        if (Input.GetKey("a")) rb.velocity = new Vector2(-5f, rb.velocity.y);
    }
}
