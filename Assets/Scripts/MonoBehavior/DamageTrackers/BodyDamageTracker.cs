using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class BodyDamageTracker : MonoBehaviour, IDamageTracker
{
    private HealthManager _healthManager;
    private Rigidbody2D _pointer;
    private PointerFiller _filler;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _filler = GetComponentInParent<PointerFiller>();
        _healthManager = GetComponentInParent<HealthManager>();
        _pointer = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    #region IDamageTracker
    public void SendDamage(DamageInfo info)
    {
        int processedDamage = Random.Range(info.damage / 2, info.damage + 1);
        _healthManager.SetDamage(processedDamage, info.damage, info.direction);

        _filler.PushMotion(_pointer, PointerMotion.None);
        _pointer.isKinematic = false;

        _pointer.velocity = info.speed;
        BloodDrawer.Draw(_sprite, info.contacts[0].point);
    }
    #endregion
}
