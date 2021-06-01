using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;


public class WeaponDamageTracker : MonoBehaviour, IDamageTracker
{
    [SerializeField] private Weapon weapon;

    private Rigidbody2D _bodyPointer;
    private Rigidbody2D _armPointer;
    private PointerFiller _filler;
    private ProcedureAnimation _armRoot;

    void Start()
    {
        _filler = GetComponentInParent<PointerFiller>();
        _bodyPointer = _filler.GetPointer(KinematicsPointerType.Body);
        _armRoot = GetComponentInParent<ProcedureAnimation>();
        _armPointer = _armRoot.Pointer.GetComponent<Rigidbody2D>();
    }

    private IEnumerator ReturnToKinematic(float delay)
    {
        yield return new WaitForSeconds(delay);
        _armPointer.velocity = Vector2.zero;
        _armPointer.isKinematic = true;
    }

    #region IDamageTracker
    public void SendDamage(int damage, Vector2 direction)
    {
        GetComponentInParent<PointerFiller>().PushMotion(_armPointer, PointerMotion.None);
        _armPointer.position = _armRoot.transform.GetChild(_armRoot.transform.childCount - 1).position;
        _armPointer.isKinematic = false;
        _armPointer.gravityScale = 0f;
        _armPointer.AddForce(direction * 10f, ForceMode2D.Impulse);

        _bodyPointer.velocity = Vector2.zero;
        _bodyPointer.AddForce(direction * 5f, ForceMode2D.Impulse);

        weapon.Block();
        StartCoroutine(ReturnToKinematic(0.5f));
    }
    #endregion
}
