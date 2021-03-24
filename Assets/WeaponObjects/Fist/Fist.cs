using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    private float _force = 5f;

    private bool _isKilled = false;
    private int _hitNumber = 1;
    private Vector3 _startPosition;

    public override void Init(Transform hand, float attackProgress)
    {
        base.Init(hand, attackProgress);
        _startPosition = transform.position;
    }

    public override void MotionFinished()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon") KillWeapon();
        if (collision.gameObject.tag == _targetTag) HitEnemy(collision.gameObject);
    }

    private void KillWeapon()
    {
        if (!_isKilled)
        {
            _isKilled = true;
            Destroy(gameObject);
        }
    }

    private void HitEnemy(GameObject hittedObject)
    {
        if (!_isKilled)
        {
            KinematicsPointer pointer = hittedObject.GetComponent<KinematicsPointer>();
            if (pointer == null)
                pointer = hittedObject.GetComponentInParent<ProcedureAnimation>().Pointer.gameObject.GetComponent<KinematicsPointer>();

            pointer.PushMotion( new AttackHitMotion( GetForce() ) );

            int damageScale = hittedObject.layer == 10 ? 1 : 3;
            hittedObject.GetComponentInParent<HealthManager>().SetDamage(GetDamage() / damageScale, (int)_force);

            _hitNumber++;
        }
    }

    private Vector2 GetForce()
    {
        float a = Vector2.SignedAngle(Vector2.right, transform.position - _startPosition);
        float dx = Mathf.Cos(a*Mathf.Deg2Rad);
        float dy = Mathf.Sin(a*Mathf.Deg2Rad);

        return (new Vector2(_force*dx, _force*dy) * _attackProgress) / _hitNumber;
    }

    private int GetDamage() => (int)(_force * _attackProgress / _hitNumber);
}
