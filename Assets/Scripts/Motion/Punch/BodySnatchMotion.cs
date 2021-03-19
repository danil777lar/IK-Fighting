using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySnatchMotion : Motion
{
    public override void UpdateMotion(){}

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
        Rigidbody2D rb = _pointer.GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(new Vector2(5f, 0f), ForceMode2D.Impulse);
    }
}
