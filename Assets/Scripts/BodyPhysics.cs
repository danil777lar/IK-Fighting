using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPhysics : MonoBehaviour
{
    private Rigidbody2D _rb;
    private KinematicsPointer _pointer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pointer = GetComponent<KinematicsPointer>();
    }

    private void Update()
    {
        float a = _rb.velocity.x * -4f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, a), Time.deltaTime/0.1f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && _pointer != null)
        {
            collision.gameObject.GetComponent<KinematicsPointer>().PushMotion(new AttackHitMotion(_rb.velocity * 1f));
            _pointer.PushMotion( new AttackHitMotion( _rb.velocity * -1f ) );
        }
    }
}
