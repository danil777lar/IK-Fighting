using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LarjeEnum;

public class FightController : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();

    [SerializeField] private ProcedureAnimation _arm;
    [SerializeField] private Rigidbody2D _pointer;

    private PhysicsMachine _physicsMachine;
    private Weapon _curentWeapon;
    private int _curentWeaponId = 0;

    private void Start()
    {
        _physicsMachine = GetComponent<PhysicsMachine>();
        SetupWeapon();
    }

    private void SetupWeapon() 
    {
        Transform oldEnd = _arm.transform.GetChild(_arm.transform.childCount - 1);
        GameObject newWeapon = Instantiate(weapons[_curentWeaponId].gameObject);
        _curentWeapon = newWeapon.GetComponent<Weapon>();

        newWeapon.transform.SetParent(_arm.transform);
        newWeapon.transform.position = oldEnd.position;
        _curentWeapon.ChainEnd.SetParent(_arm.transform);

        DestroyImmediate(oldEnd.gameObject);
        _arm.CalculateSegments();

        _physicsMachine.offsets[_pointer].bodyOffset = _curentWeapon.CalmOffset;
    }
}
