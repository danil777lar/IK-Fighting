using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static partial class MotionBuilder
{
    private static Tween ArmCalm(Rigidbody2D t)
    {
        return t.DOMove(Vector2.zero, 1f);
    }

    private static Tween ArmPunch(Rigidbody2D t)
    {
        Transform armRoot = t.GetComponentInParent<IControll>().GetArmRoot();
        Vector2 targetPosition = (Vector2)armRoot.position + ((Vector2)armRoot.position - t.position)*2f;
        return t.DOMove(targetPosition, 0.5f)
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.InQuad);
    }
}
