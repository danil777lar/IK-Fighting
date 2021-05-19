using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static partial class MotionBuilder
{
    private static Tween LegCalm(Rigidbody2D t)
    {
        return t.DOMove(Vector2.zero, 1f);
    }
}
