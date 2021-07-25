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
    [SerializeField] private ParticleSystem _damagedParticles;

    private bool _isBlocked;
    private float _damageScale;
    private Vector2 _lastPoint;
    private Vector2 _forceDirection;
    private Rigidbody2D _armPointer;
    private Rigidbody2D _bodyRb;
    private List<DamageInfo> _damageTrackers;

    #region Lifecycle

    private void Start()
    {
        PointerFiller filler = GetComponentInParent<PointerFiller>();
        _armPointer = filler.GetPointer(KinematicsPointerType.FrontArm);
        _bodyRb = filler.GetPointer(KinematicsPointerType.Body);
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
        _damageTrackers = new List<DamageInfo>();
        StartCoroutine(ProcessCollisions());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageTracker tracker)) 
        {
            if (tracker is WeaponDamageTracker)
                new DamageInfo(_damage, _bodyRb.velocity, _forceDirection, tracker, collision.contacts).Send();
            else
                _damageTrackers.Add(new DamageInfo(_damage, _bodyRb.velocity, _forceDirection, tracker, collision.contacts));            
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
            {
                _trailParticles.Stop();
            }
        }
    }

    private IEnumerator ProcessCollisions()
    {
        yield return new WaitForFixedUpdate();

        if (!_isBlocked)
        {
            if (_damagedParticles != null && _damageTrackers.Count > 0)
            {
                GameObject parts = Instantiate(_damagedParticles.gameObject);
                parts.transform.position = _damageTrackers[0].contacts[0].point;
                float rot = Mathf.Atan2(_forceDirection.x, _forceDirection.y) * Mathf.Rad2Deg;
                parts.transform.rotation = Quaternion.Euler(0f, 0f, -(rot - 90f));
                _bodyRb.velocity = -_bodyRb.velocity / 2f;
                _bodyRb.GetComponent<ParticleSystem>().Stop();
                Destroy(parts, _damagedParticles.main.duration);
            }
            foreach (DamageInfo info in _damageTrackers)
                info.Send();
        }
    }
}