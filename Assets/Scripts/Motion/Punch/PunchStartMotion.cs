using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchStartMotion : Motion
{
    const float DURATION = 1f;

    private Transform _bodyPointer;
    private IControll _controllInterface;

    private int _hand;

    private float _startTime = -1f;
    private Vector2 _startPosition;

    public PunchStartMotion(Transform body, IControll controll, int hand) 
    {
        _bodyPointer = body;
        _controllInterface = controll;
        _hand = hand;
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

            float angle = _controllInterface.GetAttackButtonAngle(_hand);
            float dy = Mathf.Sin(angle * Mathf.Deg2Rad) * -2f;
            float dx = Mathf.Sin((90f - angle) * Mathf.Deg2Rad) * 2f;

            Vector2 targetPosition = _bodyPointer.position - new Vector3(dx, dy);
            targetPosition.x -= 2f;

            _pointer.position = Vector2.Lerp(_startPosition, targetPosition, t);
        }




        IsFinished = _controllInterface.GetAttackButtonUp(_hand);
    }

}
