using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CeilBorder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb && !rb.isKinematic && rb.velocity.y > 0) 
            rb.velocity = new Vector2(rb.velocity.x, 0f);
    }
}
