using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepMotion : Motion
{
    const float Y_POSITION = -2.5f;
    const float DURATION_MIN = 0.01f;
    const float DURATION_MAX = 0.5f;

    private Transform _bodyPointer;
    private Transform _otherLegPointer;

    private Rigidbody2D _rb;

    private float _startTime = -1f;

    private bool _mostPriority;

    private Vector2[] _animationPoints;

    public StepMotion(Transform bodyPointer, Transform otherLegPointer, bool mostPriority) 
    {
        _bodyPointer = bodyPointer;
        _otherLegPointer = otherLegPointer;
        _rb = _bodyPointer.GetComponent<Rigidbody2D>();
        _mostPriority = mostPriority;
    }

    public override void Init(KinematicsPointer pointer)
    {
        base.Init(pointer);
        Rigidbody2D rb = pointer.GetComponent<Rigidbody2D>();
        rb.drag = 1f;
        rb.gravityScale = 0f;
    }

    override public void UpdateMotion() 
    {
        if (_startTime != -1f) Animate();
        else 
        {
            if (_pointerTransform.position.y != Y_POSITION) MoveToDefaultYPosition();
            else MoveToDefaultXPosition();
            if (_otherLegPointer.position.y <= Y_POSITION) CheckBodyPosition();
        }
    }

    private void MoveToDefaultYPosition()
    {
        _pointerTransform.position = new Vector2(_pointerTransform.position.x, Y_POSITION);
    }

    private void MoveToDefaultXPosition() 
    {
        if (_bodyPointer.position.x - _pointerTransform.position.x > 3f) 
            _pointerTransform.position = new Vector2(_bodyPointer.position.x - 2f, _pointerTransform.position.y);

        if (_bodyPointer.position.x - _pointerTransform.position.x < -3f)
            _pointerTransform.position = new Vector2(_bodyPointer.position.x + 2f, _pointerTransform.position.y);
    }

    private void Animate()
    {
        float speed = _rb.velocity.x < 0f ? _rb.velocity.x * -1f : _rb.velocity.x;
        float duration = (10f - speed) / 10f;
        if (duration < 0f) duration = duration * -1f;
        if (duration < DURATION_MIN) duration = DURATION_MIN;
        if (duration > DURATION_MAX) duration = DURATION_MAX;

        float t = (Time.time - _startTime) / duration;
        _pointerTransform.position = QuadraticLerp(_animationPoints[0], _animationPoints[1], _animationPoints[2], t);
        if (t >= 1) _startTime = -1f;
    }

    private void CheckBodyPosition() 
    {
        bool leftestLeg = _mostPriority ? _pointerTransform.position.x <= _otherLegPointer.position.x : _pointerTransform.position.x < _otherLegPointer.position.x;
        bool rightestLeg = _mostPriority ? _pointerTransform.position.x > _otherLegPointer.position.x : _pointerTransform.position.x > _otherLegPointer.position.x;

        bool isBodyLeft = (_bodyPointer.position.x < _pointerTransform.position.x) && (_bodyPointer.position.x < _otherLegPointer.position.x) 
            && rightestLeg;
        bool isBodyRight = (_bodyPointer.position.x > _pointerTransform.position.x) && (_bodyPointer.position.x > _otherLegPointer.position.x)
            && leftestLeg;

        if (isBodyLeft || isBodyRight) 
        {
            Vector2 start = _pointerTransform.position;
            Vector2 finish = new Vector2();
            finish.x = isBodyLeft ? _bodyPointer.position.x - 2f : _bodyPointer.position.x + 2f;
            finish.y = Y_POSITION;
            Vector2 center = new Vector2( (start.x + finish.x) / 2, Y_POSITION + 1f );

            _animationPoints = new Vector2[]{start, center, finish};
            _startTime = Time.time;
        }
    }

    private Vector2 QuadraticLerp(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 ab = Vector2.Lerp(a, b, t);
        Vector2 bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }
}
