using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePunchMotion : Motion
{
    const float DURATION = 0.2f;

    private Transform _armRoot;

    private float _startTime = -1f;
    private Vector3 _startPosition;

    public SimplePunchMotion(Transform root)
    {
        _armRoot = root;
    }

    public override void Init(KinematicsPointer pointer)
    {
        base.Init(pointer);
        _startTime = Time.time;
        _startPosition = _pointerTransform.position;
    }

    public override void UpdateMotion()
    {
        if (_startTime != -1f)
        {
            float t = (Time.time - _startTime) / DURATION;

            Vector3 deltaPosition = _armRoot.position - _startPosition;

            Vector2 targetPosition = _armRoot.position + deltaPosition;
            _pointerTransform.position = Vector2.Lerp(_startPosition, targetPosition, t);

            if (t >= 1) IsFinished = true;
        }
    }
}
