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
        return DOTween.To
            (
                () => t.position - (Vector2)armRoot.position,
                (v) => t.position = (Vector2)armRoot.position + v,
                ((Vector2)armRoot.position - t.position) * 2f, 0.2f
            )
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.InQuad);
    }
}
