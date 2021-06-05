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

        Vector2 normal = ((Vector2)armRoot.position - t.position).normalized;

        Tween tween;
        Vector2 start = t.position - (Vector2)armRoot.position;
        Vector2 target = ((Vector2)armRoot.position - t.position).normalized * armRoot.GetComponent<ProcedureAnimation>().Lenght;

        if (normal.y > 0.5 || normal.y < -0.5)     
        {
            tween = DOTween.To
                (() => 0f,
                (v) =>
                {
                    float c1 = 1.70158f;
                    float c3 = c1 + 1f;

                    Vector2 pos;
                    pos.x = Mathf.Lerp(start.x, target.x, 1 + c3 * Mathf.Pow(v - 1, 3) + c1 * Mathf.Pow(v - 1, 2));
                    pos.y = Mathf.Lerp(start.y, target.y, v == 0 ? 0 : Mathf.Pow(2, 10 * v - 10));
                    t.position = (Vector2)armRoot.position + pos;
                },
                1f, 0.2f);
        }
        else
        {
            tween = DOTween.To
                (() => t.position - (Vector2)armRoot.position,
                (v) => t.position = (Vector2)armRoot.position + v,
                target, 0.2f);
        }

        tween
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.OutBack);

        return tween;
    }
}
