using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Attacks/Simple Attack")]

public class SimpleAttack : OneHandedAttack
{
    public override Dictionary<int, Motion[]> GetMotion(AttackHolder attackHolder, Transform root, IControll controll, int hand)
    {
        Dictionary<int, Motion[]> dict = new Dictionary<int, Motion[]>();
        Motion[] motionList = new Motion[] { new SimplePunchMotion(root, attackHolder), new PunchStartMotion(root, controll, hand) };
        dict.Add( GetPointerId(hand),  motionList);
        return dict;
    }

}
