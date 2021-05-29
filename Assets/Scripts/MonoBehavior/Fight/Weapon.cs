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

        Transform pointer = GetComponentInParent<ProcedureAnimation>().Pointer;
    }
}
