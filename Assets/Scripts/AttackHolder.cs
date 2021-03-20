using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PointerFiller))]
[RequireComponent(typeof(IControll))]

public class AttackHolder : MonoBehaviour
{
    [SerializeField] private Transform _armRoot; 

    public OneHandedAttack _frontHandAttack;
    public OneHandedAttack _backHandAttack;
    //public Attack _superAttack;

    private IControll _controllInterface;
    private PointerFiller _pointerFiller;

    void Start()
    {
        _pointerFiller = GetComponent<PointerFiller>();
        _controllInterface = GetComponent<IControll>();
    }

    void Update()
    {
        if (_controllInterface.GetAttackButtonDown(0) && _frontHandAttack != null) 
            _pointerFiller.FillPointers(_frontHandAttack.GetMotion(_armRoot, _controllInterface, PointerFiller.FrontArm));

        if (_controllInterface.GetAttackButtonDown(1) && _backHandAttack != null) 
            _pointerFiller.FillPointers(_backHandAttack.GetMotion(_armRoot, _controllInterface, PointerFiller.BackArm));

        //if (_controllInterface.GetAttackButtonDown(2) && _superAttack != null) _pointerFiller.FillPointers();
    }
}
