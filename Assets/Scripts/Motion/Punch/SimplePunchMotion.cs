using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePunchMotion : Motion
{
    const float DURATION = 0.2f;

    private Transform _bodyPointer;
    private float _startTime = -1f;
    private Vector2 _startPosition;

    public SimplePunchMotion(Transform body)
    {
        _bodyPointer = body;
    }

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
        _startTime = Time.time;
        _startPosition = _pointer.position;
    }

    public override void UpdateMotion()
    {
        if (_startTime != -1f)
        {
            float t = (Time.time - _startTime) / DURATION;
            Vector2 targetPosition = _bodyPointer.localPosition + new Vector3(3f, 2f);
            _pointer.position = Vector2.Lerp(_startPosition, targetPosition, t);

            if (t >= 1) IsFinished = true;
        }
    }
}
