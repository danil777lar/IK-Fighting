using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class LimbDamageTracker : MonoBehaviour, IDamageTracker
{
    private HealthManager _healthManager;
    private Rigidbody2D _pointer;
    private PointerFiller _filler;

    private void Start()
    {
        _filler = GetComponentInParent<PointerFiller>();
        _healthManager = GetComponentInParent<HealthManager>();
        _pointer = GetComponentInParent<ProcedureAnimation>().Pointer.GetComponent<Rigidbody2D>();
    }

    private IEnumerator ReturnToKinematic(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pointer.velocity = Vector2.zero;
        _pointer.isKinematic = true;
    }

    #region IDamageTracker
    public void SendDamage(int damage, Vector2 direction) 
    {
        int halfdamage = damage / 2;
        int processedDamage = Random.Range(halfdamage / 2, halfdamage + 1);
        _healthManager.SetDamage(processedDamage, damage, direction);

        _filler.PushMotion(_pointer, PointerMotion.None);
        _pointer.isKinematic = false;
        _pointer.gravityScale = 0f;

        _pointer.position = transform.parent.GetChild(transform.parent.childCount - 1).position;
        _pointer.velocity = Vector2.zero;
        _pointer.AddForce(direction * 10f * _pointer.mass, ForceMode2D.Impulse);

        StartCoroutine(ReturnToKinematic(0.5f));
    }
    #endregion
}
