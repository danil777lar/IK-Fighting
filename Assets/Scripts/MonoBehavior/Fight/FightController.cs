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
    private PointerFiller _filler;
    private IControll _controll;
    private Weapon _curentWeapon;

    private bool _isAiming = false;
    private int _curentWeaponId = 0;

    private void Start()
    {
        _physicsMachine = GetComponent<PhysicsMachine>();
        _controll = GetComponent<IControll>();
        _filler = GetComponent<PointerFiller>();
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

    private void FixedUpdate()
    {
        if (!_pointer.isKinematic) 
        {
            _curentWeapon.SetDamagable(false);
            _isAiming = false;
            return;
        }

        if (_controll.GetAttackButtonDown(0)) 
        {
            if (_curentWeapon.OnPointerDown != PointerMotion.None)
                _filler.PushMotion(_pointer, _curentWeapon.OnPointerDown, () => _isAiming = true);
            else 
                _isAiming = true;
        }

        if (_isAiming)
        {
            Vector2 offset = _curentWeapon.CalmOffset.magnitude * _controll.GetAttackButtonNormal(0) * -1f;
            offset.x *= GetComponent<DirectionController>().Direction;

            _physicsMachine.offsets[_pointer].bodyOffset = offset;
            _physicsMachine.offsets[_pointer].noiseScale = 0f;
            _physicsMachine.offsets[_pointer].transitionSpeed = 1 / Time.fixedDeltaTime;
        }
        else
        {
            _physicsMachine.offsets[_pointer].bodyOffset = _curentWeapon.CalmOffset;
            _physicsMachine.offsets[_pointer].noiseScale = 1f;
            _physicsMachine.offsets[_pointer].transitionSpeed = 5f;
        }

        if (!_controll.GetAttackButton(0) && _isAiming)
        {
            _curentWeapon.SetDamagable(true);
            _filler.PushMotion(_pointer, _curentWeapon.OnPointerUp, () => _curentWeapon.SetDamagable(false));
            _isAiming = false;
        }
    }


}
