using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _chainEnd;
    public Transform ChainEnd => _chainEnd;

    [SerializeField] private Vector2 _calmOffset;
    public Vector2 CalmOffset => _calmOffset;

    [SerializeField] private PointerMotion _onPointerDown;
    public PointerMotion OnPointerDown => _onPointerDown;

    [SerializeField] private PointerMotion _onPointerUp;
    public PointerMotion OnPointerUp => _onPointerUp;

    [SerializeField] private int _damage;
    [SerializeField] private Collider2D _damageCollider;
    [SerializeField] private ParticleSystem _trailParticles;
    [SerializeField] private ParticleSystem _blockParticles;

    private bool _isBlocked;
    private float _damageScale;
    private Vector2 _lastPoint;
    private Vector2 _forceDirection;
    private Rigidbody2D _armPointer;
    private List<IDamageTracker> _damageTrackers;

    #region Lifecycle

    private void Start()
    {
        PointerFiller filler = GetComponentInParent<PointerFiller>();
        _armPointer = filler.GetPointer(KinematicsPointerType.FrontArm);
        _damageCollider.gameObject.layer = GetComponentInParent<FightController>().gameObject.layer;
        _damageCollider.enabled = false;

        _lastPoint = _armPointer.position;
        _forceDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        _forceDirection = (_armPointer.position - _lastPoint).normalized;
        _lastPoint = _armPointer.position;

        _isBlocked = false;
        _damageTrackers = new List<IDamageTracker>();
        StartCoroutine(ProcessCollisions());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageTracker tracker)) 
        {
            if (tracker is WeaponDamageTracker)
                tracker.SendDamage(_damage, _forceDirection);
            else
                _damageTrackers.Add(tracker);
        }
    }

    #endregion

    public void Block()
    {
        _isBlocked = true;
        SetDamagable(false);
        if (_blockParticles != null)
            _blockParticles.Play();
    }

    public void SetDamagable(bool arg, float damageScale = 0f)
    {
        _damageScale = damageScale;
        _damageCollider.enabled = arg;
        if (_trailParticles != null)
        {
            if (arg)
                _trailParticles.Play();
            else
                _trailParticles.Stop();
        }
    }

    private IEnumerator ProcessCollisions()
    {
        yield return new WaitForFixedUpdate();

        if (!_isBlocked)
            foreach (IDamageTracker tracker in _damageTrackers)
                tracker.SendDamage(_damage, _forceDirection);
    }
}
