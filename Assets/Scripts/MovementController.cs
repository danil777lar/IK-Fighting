using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private float _speed = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey("d")) rb.velocity = new Vector2(_speed, rb.velocity.y);
        if (Input.GetKey("a")) rb.velocity = new Vector2(-_speed, rb.velocity.y);
    }
}
