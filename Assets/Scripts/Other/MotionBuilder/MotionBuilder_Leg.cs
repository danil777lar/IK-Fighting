using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LarjeEnum;

public static partial class MotionBuilder
{
    private static Tween LegCalm(Rigidbody2D t)
    {
        return t.DOMove(Vector2.zero, 1f);
    }

    private static Tween LegStep(Rigidbody2D t) 
    {
        Rigidbody2D bodyRb = t.GetComponentInParent<PointerFiller>().GetPointer(KinematicsPointerType.Body);
        float offsetX = t.position.x < bodyRb.position.x ? DataGameMain.Default.personStepLenght : -DataGameMain.Default.personStepLenght;
        return LegStepIternal(t, bodyRb, offsetX);
    }

    private static Tween LegToNormalDistance(Rigidbody2D t)
    {
        Rigidbody2D bodyRb = t.GetComponentInParent<PointerFiller>().GetPointer(KinematicsPointerType.Body);
        float offsetX = t.position.x < bodyRb.position.x ? -DataGameMain.Default.personStepLenght : DataGameMain.Default.personStepLenght;
        return LegStepIternal(t, bodyRb, offsetX);
    }

    private static Tween LegStepIternal(Rigidbody2D t, Rigidbody2D bodyRb, float offsetX) 
    {
        Vector2 targetPosition = bodyRb.position;

        float duration = 0.5f / Mathf.Abs(bodyRb.velocity.x + 1 * Mathf.Sign(bodyRb.velocity.x));
        duration = Mathf.Clamp(duration, 0f, 0.5f);
        targetPosition.x += offsetX;

        RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
        if (Mathf.Abs(hit.point.y - bodyRb.position.y) > DataGameMain.Default.personStandHeight)
            hit = Physics2D.Raycast(bodyRb.position, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
        targetPosition.y = hit.point.y;

        Tween tween = t.DOMove(targetPosition, duration)
            .SetUpdate(UpdateType.Fixed);

        return tween;
    }
}
