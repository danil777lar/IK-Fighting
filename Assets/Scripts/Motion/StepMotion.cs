using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepMotion : Motion
{
    private Vector2 _finishPosition;

    private float _stepDuration = 0.3f;

    public StepMotion(Vector2 finishPosition) 
    {
        _finishPosition = finishPosition;
    }

    override public void UpdateMotion() 
    {
        Vector2 center = (_startPosition + _finishPosition) / 2;
        center.y += 2f;
        float t = (Time.time - _startTime) / _stepDuration;
        _pointer.position = QuadraticLerp(_startPosition, center, _finishPosition, t);
        if (t >= 1) IsFinished = true;
    }

    private Vector2 QuadraticLerp(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 ab = Vector2.Lerp(a, b, t);
        Vector2 bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }
}
