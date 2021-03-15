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
        float t = (Time.time - _startTime) / _stepDuration;
        _pointer.position = Vector2.Lerp(_startPosition, _finishPosition, t);

        if (t >= 1) IsFinished = true;
    }
}
