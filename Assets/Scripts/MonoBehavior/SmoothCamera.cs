using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _positionSmooth;
    [SerializeField] private float _rotationSmooth;

    private Vector3 _positionOffset;

    private void Start() => _positionOffset = _target.position - transform.position;

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(_target.position.x - (_positionOffset.x * _target.localScale.x), _target.position.y - _positionOffset.y);
        targetPosition.z = transform.position.z;
        if (_positionSmooth != -1f) transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime/_positionSmooth);
        if (_rotationSmooth != -1f) transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, Time.deltaTime / _rotationSmooth);
    }
}
