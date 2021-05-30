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
    private Vector2 _lastPoint;
    private Vector2 _forceDirection;
    private Rigidbody2D _armPointer;
    private Rigidbody2D _bodyPointer;
    private ProcedureAnimation _armRoot;
    private IWeapon _weaponObject;
    private List<Collider2D> _collisions;


    #region Lifecycle

    private void Start()
    {
        _weaponObject = GetComponent<IWeapon>();

        _armRoot = GetComponentInParent<ProcedureAnimation>();

        PointerFiller filler = GetComponentInParent<PointerFiller>();
        _armPointer = filler.GetPointer(KinematicsPointerType.FrontArm);
        _bodyPointer = filler.GetPointer(KinematicsPointerType.Body);

        _damageCollider.gameObject.layer = GetComponentInParent<FightController>().gameObject.layer;
        _damageCollider.enabled = false;

        _lastPoint = _armPointer.position;
        _forceDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        _forceDirection = ((Vector2)_armPointer.position - _lastPoint).normalized;
        _lastPoint = _armPointer.position;

        _isBlocked = false;
        _collisions = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon") 
        {
            _isBlocked = true;
            SetDamagable(false);

            GetComponentInParent<PointerFiller>().PushMotion(_armPointer, PointerMotion.None);
            _armPointer.position = _armRoot.transform.GetChild(_armRoot.transform.childCount - 1).position;
            _armPointer.isKinematic = false;
            _armPointer.gravityScale = 0f;
            _armPointer.AddForce(-_forceDirection * 10f, ForceMode2D.Impulse);

            _bodyPointer.velocity = Vector2.zero;
            _bodyPointer.AddForce(-_forceDirection * 5f, ForceMode2D.Impulse);

            StartCoroutine(SetKinematic(_armPointer, 0.5f));

            if (_blockParticles != null)
                _blockParticles.Play();
        }

        _collisions.Add(collision);
    }

    private void LateUpdate()
    {
        int targetLayer = 0;
        if (_damageCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            targetLayer = LayerMask.NameToLayer("Enemy");
        else if (_damageCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            targetLayer = LayerMask.NameToLayer("Player");

        if (!_isBlocked)
        {
            foreach (Collider2D collision in _collisions)
            {
                if (collision.gameObject.layer == targetLayer)
                {
                    int damage = _damage / (collision.CompareTag("Body") ? 1 : 2);
                    collision.gameObject.GetComponentInParent<HealthManager>().SetDamage(Random.Range(damage / 2, damage + 1), _damage);
                }

                Rigidbody2D pointer = null;
                if (collision.tag == "Body")
                    pointer = collision.GetComponent<Rigidbody2D>();
                else
                    pointer = collision.GetComponentInParent<ProcedureAnimation>()?.Pointer.GetComponent<Rigidbody2D>();
                if (pointer)
                {
                    pointer.GetComponentInParent<PointerFiller>().PushMotion(pointer, PointerMotion.None);

                    pointer.isKinematic = false;
                    pointer.gravityScale = 0f;

                    pointer.AddForce(_forceDirection * 10f, ForceMode2D.Impulse);
                    if (pointer.tag != "Body")
                        StartCoroutine(SetKinematic(pointer, 0.5f));
                }
            }
        }
    }

    #endregion

    public void SetDamagable(bool arg)
    {
        _damageCollider.enabled = arg;
        if (_trailParticles != null)
        {
            if (arg)
                _trailParticles.Play();
            else
                _trailParticles.Stop();
        }
    }

    private IEnumerator SetKinematic(Rigidbody2D pointer, float delay) 
    {
        yield return new WaitForSeconds(delay);
        pointer.velocity = Vector2.zero;
        pointer.isKinematic = true;
    }
}
