using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private KinematicsPointer _backLegPointer;
    [SerializeField] private KinematicsPointer _frontLegPointer;

    private bool IsMotionInProcess = false;

    void Update()
    {
        if (!IsMotionInProcess)
        {
            float minX = Mathf.Min(_backLegPointer.transform.position.x, _frontLegPointer.transform.position.x);
            float maxX = Mathf.Max(_backLegPointer.transform.position.x, _frontLegPointer.transform.position.x);

            if (transform.position.x < minX || transform.position.x > maxX)
            {
                float backLegDistance = Vector2.Distance(transform.position, _backLegPointer.transform.position);
                float frontLegDistance = Vector2.Distance(transform.position, _frontLegPointer.transform.position);
                KinematicsPointer pointer = backLegDistance > frontLegDistance ? _backLegPointer : _frontLegPointer;

                float finishPositionX = transform.position.x + (pointer.transform.position.x < transform.position.x ? 2f : -2f);
                pointer.SetMotion(new StepMotion(new Vector2( finishPositionX, -3.041f)));
                pointer.MotionFinishedEvent += OnMotionFinished;
                IsMotionInProcess = true;
            }
        }
    }

    private void OnMotionFinished()
    {
        IsMotionInProcess = false;
    }
}
