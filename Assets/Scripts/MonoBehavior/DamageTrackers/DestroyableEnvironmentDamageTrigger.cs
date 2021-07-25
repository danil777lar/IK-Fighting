using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEnvironmentDamageTrigger : MonoBehaviour, IDamageTracker
{
    [SerializeField] private float _health;
    [SerializeField] private float _mass;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private ParticleSystem _particlesOnDestroy;
    [SerializeField] private SpriteRenderer _sprite;

    private float _curentHealth;

    void Start()
    {
        _curentHealth = _health;
    }

    #region IDamageTracker

    public void SendDamage(DamageInfo info) 
    {
        _curentHealth -= info.damage;
        if (_curentHealth > 0)
            _rb.AddForce(info.direction * 10f * _mass, ForceMode2D.Impulse);
        else 
        {
            _particlesOnDestroy.Play();
            Destroy(_sprite.gameObject, _particlesOnDestroy.main.duration);
            _sprite.enabled = false;
            enabled = false;
            Destroy(this);
        }
    }

    #endregion
}