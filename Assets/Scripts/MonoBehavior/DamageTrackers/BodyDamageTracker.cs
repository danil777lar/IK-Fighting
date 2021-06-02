using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class BodyDamageTracker : MonoBehaviour, IDamageTracker
{
    private HealthManager _healthManager;
    private Rigidbody2D _pointer;
    private PointerFiller _filler;

    private void Start()
    {
        _filler = GetComponentInParent<PointerFiller>();
        _healthManager = GetComponentInParent<HealthManager>();
        _pointer = GetComponent<Rigidbody2D>();
    }

    #region IDamageTracker
    public void SendDamage(int damage, Vector2 direction)
    {
        int processedDamage = Random.Range(damage / 2, damage + 1);
        _healthManager.SetDamage(processedDamage, damage, direction);

        _filler.PushMotion(_pointer, PointerMotion.None);
        _pointer.isKinematic = false;
        _pointer.gravityScale = 0f;

        _pointer.velocity = Vector2.zero;
        _pointer.AddForce(direction * 10f * _pointer.mass, ForceMode2D.Impulse);
    }
    #endregion
}
