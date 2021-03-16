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

    private Vector2[] _animationPoints;

    public StepMotion(Transform bodyPointer, Transform otherLegPointer) 
    {
        _bodyPointer = bodyPointer;
        _otherLegPointer = otherLegPointer;
        _rb = _bodyPointer.GetComponent<Rigidbody2D>();
    }

    public override void Init(Transform pointer)
    {
        base.Init(pointer);
    }

    override public void UpdateMotion() 
    {
        if (_startTime != -1f) Animate();
        else if (_otherLegPointer.position.y <= Y_POSITION) CheckBodyPosition();
    }

    private void Animate()
    {
        float speed = _rb.velocity.x < 0f ? _rb.velocity.x * -1f : _rb.velocity.x;
        float duration = (10f - speed) / 10f;
        if (duration < 0f) duration = duration * -1f;
        if (duration < DURATION_MIN) duration = DURATION_MIN;
        if (duration > DURATION_MAX) duration = DURATION_MAX;

        float t = (Time.time - _startTime) / duration;
        _pointer.position = QuadraticLerp(_animationPoints[0], _animationPoints[1], _animationPoints[2], t);
        if (t >= 1) _startTime = -1f;
    }

    private void CheckBodyPosition() 
    {
        bool isBodyLeft = (_bodyPointer.position.x < _pointer.position.x) && (_bodyPointer.position.x < _otherLegPointer.position.x) 
            && _pointer.position.x > _otherLegPointer.position.x;
        bool isBodyRight = (_bodyPointer.position.x > _pointer.position.x) && (_bodyPointer.position.x > _otherLegPointer.position.x)
            && _pointer.position.x < _otherLegPointer.position.x;

        if (isBodyLeft || isBodyRight) 
        {
            Vector2 start = _pointer.position;
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
