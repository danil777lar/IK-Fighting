using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LarjeEnum;

public class FightController : MonoBehaviour, IProgressInformation
{
    public List<Weapon> weapons = new List<Weapon>();

    [SerializeField] private ProcedureAnimation _arm;
    [SerializeField] private Rigidbody2D _pointer;
    [SerializeField] private AttackForceIndicator _forceIndicator;

    private PhysicsMachine _physicsMachine;
    private PointerFiller _filler;
    private Weapon _curentWeapon;
    private IControll _controll;

    private bool _isAiming = false;
    private int _curentWeaponId = 0;
    private float _aimValue = 0f;
    private Vector2 _forceDirection;
    private Vector2 _aimOffset;
    private Vector2 _startPosition;
    private Tween _aimValueTween;

    public float AimValue => _aimValue;
    public Vector2 StartPosition => _startPosition;
    public Vector2 AimOffset => _aimOffset; 

    private void Start()
    {
        _physicsMachine = GetComponent<PhysicsMachine>();
        _controll = GetComponent<IControll>();
        _filler = GetComponent<PointerFiller>();
        SetupWeapon();
    }

    private void FixedUpdate()
    {
        if (!_pointer.isKinematic)
        {
            _curentWeapon.SetDamagable(false, _aimValue);
            EnableAiming(false);
            return;
        }

        if (_controll.GetAttackButton(0) && !_isAiming)
        {
            if (_curentWeapon.OnPointerDown != PointerMotion.None)
                _filler.PushMotion(_pointer, _curentWeapon.OnPointerDown, () => EnableAiming(true));
            else
            {
                _pointer.position = _arm.transform.GetChild(_arm.transform.childCount - 1).position;
                EnableAiming(true);
            }
        }

        if (_controll.GetAttackButton(0) && _isAiming)
        {
            _forceDirection = _controll.GetAttackButtonNormal(0);
            Vector2 offset = _curentWeapon.CalmOffset.magnitude * -_forceDirection;
            offset.x *= GetComponent<DirectionController>().Direction;

            _aimOffset = offset;
        }

        if (!_controll.GetAttackButton(0) && _isAiming)
        {
            if (AimValue > 0.5f)
            {
                _curentWeapon.SetDamagable(true, _aimValue);
                Rigidbody2D bodyRb = _filler.GetPointer(KinematicsPointerType.Body);
                bodyRb.AddForce(_forceDirection * 10f * bodyRb.mass, ForceMode2D.Impulse);
                _filler.PushMotion(_pointer, _curentWeapon.OnPointerUp, () => _curentWeapon.SetDamagable(false, _aimValue));
            }
            EnableAiming(false);
        }
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

    private void EnableAiming(bool arg) 
    {
        _isAiming = arg;
        _startPosition = _pointer.position;
        _forceIndicator.ProgressInfo = arg ? this : null;
        if (arg)
        {
            _aimValue = 0f;
            _aimValueTween = DOTween.To
                (
                    () => _aimValue,
                    (v) => _aimValue = v,
                    1f, 1f
                )
                .SetUpdate(UpdateType.Fixed);
        }
        else
        {
            _aimValueTween?.Kill();
        }
    }

    #region IProgressInformation

    public float GetProgress() => AimValue;

    #endregion
}
