using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    private float _force = 10f;
    private int _hitNumber = 1;
    private Vector3 _startPosition;

    public override void Init(Transform hand)
    {
        base.Init(hand);
        _startPosition = transform.position;
    }

    public override void MotionFinished()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _targetTag) HitEnemy(collision.gameObject);
    }

    private void HitEnemy(GameObject hittedObject)
    {
        KinematicsPointer pointer = hittedObject.GetComponent<KinematicsPointer>();
        if (pointer == null) 
            pointer = hittedObject.GetComponentInParent<ProcedureAnimation>().Pointer.gameObject.GetComponent<KinematicsPointer>();

        pointer.PushMotion( new AttackHitMotion(GetForce()/_hitNumber));

        _hitNumber++;
        //Destroy(gameObject);
    }

    private Vector2 GetForce()
    {
        float a = Vector2.SignedAngle(Vector2.right, transform.position - _startPosition);
        float dx = Mathf.Cos(a*Mathf.Deg2Rad);
        float dy = Mathf.Sin(a*Mathf.Deg2Rad);

        return new Vector2(_force*dx, _force*dy);
    }
}
