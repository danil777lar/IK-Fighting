using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Attacks/Simple Attack")]

public class SimpleAttack : OneHandedAttack
{
    public override Dictionary<int, Motion[]> GetMotion(Transform root, IControll controll, int pointerId)
    {
        Dictionary<int, Motion[]> dict = new Dictionary<int, Motion[]>();

        dict.Add( pointerId, new Motion[]{ new SimplePunchMotion(root), new PunchStartMotion(root, controll, GetHandKey(pointerId)) } );

        return dict;
    }
}
