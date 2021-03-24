using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Attacks/Simple Attack")]

public class SimpleAttack : OneHandedAttack
{
    public override Dictionary<int, Motion[]> GetMotion(AttackHolder attackHolder, Transform root, IControll controll, DirectionController directionController, int hand)
    {
        Dictionary<int, Motion[]> dict = new Dictionary<int, Motion[]>();

        _motionList = new Motion[] { new SimplePunchMotion(root, attackHolder), new PunchStartMotion(root, controll, directionController,hand) };

        dict.Add( GetPointerId(hand),  _motionList);
        return dict;
    }

    public override IProgressInformation GetStartMotion() => _motionList[1] as IProgressInformation;

}
