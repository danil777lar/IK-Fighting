using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LarjeEnum;

public class FightController : MonoBehaviour, IProgressInformation
{
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
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
        if (!_pointer.isKinematic || _physicsMachine.CurrentState == PhysicsMachine.States.WallSlide)
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
                float force = ((Mathf.Abs(bodyRb.velocity.x) * 2f * _aimValue) + (10f * _aimValue)) * bodyRb.mass;
                bodyRb.AddForce(_forceDirection * force, ForceMode2D.Impulse);
                _filler.PushMotion(_pointer, _curentWeapon.OnPointerUp, () => _curentWeapon.SetDamagable(false, _aimValue));
                bodyRb.GetComponent<ParticleSystem>().Play();
            }
            EnableAiming(false);
        }
    }

    public void SetupWeapon(Weapon weapon = null) 
    {
        Transform oldEnd = _arm.transform.GetChild(_arm.transform.childCount - 1);
        Vector2 oldWeaponPos = oldEnd.position;
        DestroyImmediate(oldEnd.gameObject);
        if (_curentWeapon != null) 
        {
            oldWeaponPos = _curentWeapon.transform.position;
            DestroyImmediate(_curentWeapon.gameObject);
        }
        GameObject newWeapon = Instantiate(!weapon ? weapons[_curentWeaponId].gameObject : weapon.gameObject);
        _curentWeapon = newWeapon.GetComponent<Weapon>();

        newWeapon.transform.SetParent(_arm.transform);
        newWeapon.transform.position = oldWeaponPos;
        _curentWeapon.ChainEnd.SetParent(_arm.transform);

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
