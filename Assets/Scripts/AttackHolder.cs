using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionController))]
[RequireComponent(typeof(PointerFiller))]
[RequireComponent(typeof(IControll))]

public class AttackHolder : MonoBehaviour
{
    [SerializeField] private AttackForceIndicator _attackForceIndicator;

    [SerializeField] private Transform _armRoot;

    [SerializeField] private Transform _frontHand;
    [SerializeField] private Transform _backHand;

    public OneHandedAttack _frontHandAttack;
    public OneHandedAttack _backHandAttack;
    //public OneHandedAttack _superAttack;

    private IControll _controllInterface;
    private PointerFiller _pointerFiller;
    private DirectionController _directionController;

    // ONE HANDED ATTACK VALUES
    private Transform _currentHand;
    private GameObject _weapon;

    private bool _isAttacking = false;

    void Start()
    {
        _pointerFiller = GetComponent<PointerFiller>();
        _controllInterface = GetComponent<IControll>();
        _directionController = GetComponent<DirectionController>();
    }

    void Update()
    {
        if (!_isAttacking)
        {
            OneHandedAttack(0);
            OneHandedAttack(1);

            //if (_controllInterface.GetAttackButtonDown(2) && _superAttack != null)
            //    _pointerFiller.FillPointers(_superAttack.GetMotion(_armRoot, _controllInterface, PointerFiller.BackArm));
        }
    }

    private void OneHandedAttack(int hand)
    {
        if (_controllInterface.GetAttackButtonDown(hand))
        {
            OneHandedAttack currentAttack;
            switch (hand)
            {
                case 0:
                    _currentHand = _frontHand;
                    currentAttack = _frontHandAttack;
                    break;
                case 1:
                    _currentHand = _backHand;
                    currentAttack = _backHandAttack;
                    break;
                default: return;
            }

            if (currentAttack != null)
            {         
                _weapon = currentAttack.GetWeaponObject();
                _pointerFiller.FillPointers(currentAttack.GetMotion(this, _armRoot, _controllInterface, _directionController, hand));
                if (_attackForceIndicator != null) _attackForceIndicator._progressInfo = currentAttack.GetStartMoution();
                _isAttacking = true;
            }
        }
    }

    public void AttackStarted()
    {
        if (_weapon != null) 
        {
            _weapon = Instantiate(_weapon);
            _weapon.GetComponent<Weapon>().Init(_currentHand);
        }
        if (_attackForceIndicator != null) _attackForceIndicator._progressInfo = null;
    }

    public void AttackFinished() 
    {
        if (_weapon != null) _weapon.GetComponent<Weapon>().MotionFinished();
        _weapon = null;
        _currentHand = null;
        _isAttacking = false;
    }


}
