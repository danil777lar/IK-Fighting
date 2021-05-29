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

    [SerializeField] private Collider2D damageCollider;

    private IWeapon _weaponObject;

    private void Start()
    {
        _weaponObject = GetComponent<IWeapon>();
        damageCollider.gameObject.layer = GetComponentInParent<FightController>().gameObject.layer;
        damageCollider.enabled = false;
    }

    public void SetDamagable(bool arg)
    {
        damageCollider.enabled = arg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int targetLayer = 0;
        if (damageCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            targetLayer = LayerMask.NameToLayer("Enemy");
        else if (damageCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            targetLayer = LayerMask.NameToLayer("Player");

        if (collision.gameObject.layer == targetLayer)
            collision.gameObject.GetComponentInParent<HealthManager>().SetDamage(5, 10);

        Rigidbody2D pointer = collision.gameObject.GetComponentInParent<ProcedureAnimation>()?.Pointer.GetComponent<Rigidbody2D>();
        if (pointer)
        {
            pointer.GetComponentInParent<PointerFiller>().PushMotion(pointer, PointerMotion.None);

            pointer.isKinematic = false;
            pointer.gravityScale = 0f;


            Vector2 forceDirection = (pointer.position - (Vector2)transform.position).normalized;
            pointer.AddForce(forceDirection * 10f, ForceMode2D.Impulse);
            StartCoroutine(SetKinematic(pointer, 1f));
        }
    }

    private IEnumerator SetKinematic(Rigidbody2D pointer, float delay) 
    {
        yield return new WaitForSeconds(delay);
        pointer.velocity = Vector2.zero;
        pointer.isKinematic = true;
    }
}
