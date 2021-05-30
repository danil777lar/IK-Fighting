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


    private Vector2 _lastPoint;
    private Vector2 _forceDirection;
    private Rigidbody2D _pointer;
    private IWeapon _weaponObject;


    #region Lifecycle

    private void Start()
    {
        _weaponObject = GetComponent<IWeapon>();
        _pointer = GetComponentInParent<ProcedureAnimation>().Pointer.GetComponent<Rigidbody2D>();

        _damageCollider.gameObject.layer = GetComponentInParent<FightController>().gameObject.layer;
        _damageCollider.enabled = false;

        _lastPoint = _pointer.position;
        _forceDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        _forceDirection = ((Vector2)_pointer.position - _lastPoint).normalized;
        _lastPoint = _pointer.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int targetLayer = 0;
        if (_damageCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            targetLayer = LayerMask.NameToLayer("Enemy");
        else if (_damageCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            targetLayer = LayerMask.NameToLayer("Player");


        if (collision.tag == "Weapon") 
        {
            SetDamagable(false);
            if (_blockParticles != null)
                _blockParticles.Play();
        }
        else if (collision.gameObject.layer == targetLayer)
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

    #endregion

    public void SetDamagable(bool arg)
    {
        _damageCollider.enabled = arg;
        //Debug.Break();
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
