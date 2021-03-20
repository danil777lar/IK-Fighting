using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OneHandedAttack : ScriptableObject
{
    public abstract Dictionary<int, Motion[]> GetMotion(Transform root, IControll controll, int pointerId);

    protected int GetHandKey(int pointerId) 
    {
        if (pointerId == PointerFiller.BackArm) return 1;
        if (pointerId == PointerFiller.FrontArm) return 0;
        return -1;
    }
}
