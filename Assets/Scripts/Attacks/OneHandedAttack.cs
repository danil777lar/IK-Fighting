using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OneHandedAttack : ScriptableObject
{
    [SerializeField] protected GameObject _weapon;

    public GameObject GetWeaponObject() => _weapon;

    protected int GetPointerId(int hand)
    {
        if (hand == 1) return PointerFiller.BackArm;
        if (hand == 0) return PointerFiller.FrontArm;
        return -1;
    }

    // abstract & virtual
    public abstract Dictionary<int, Motion[]> GetMotion(AttackHolder attackHolder, Transform root, IControll controll, DirectionController directionController, int hand);
}
