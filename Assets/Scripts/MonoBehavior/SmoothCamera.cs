using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _positionSmooth;
    [SerializeField] private float _rotationSmooth;

    private Vector3 _positionOffset;

    private void Start()
    {
        _positionOffset = _target.position - transform.position;
        Camera.main.fieldOfView = 40f;
        LayerDefault.Default.OnPlayStart += () =>
        {
            Camera.main.DOFieldOfView(70f, 1f)
                .SetEase(Ease.InQuint);
        };
    }

    void FixedUpdate()
    {
        Vector3 offset = LayerDefault.Default.IsPlaying ? _positionOffset : -Vector3.up * 0.5f;
        offset.z = _positionOffset.z;
        Vector3 targetPosition = new Vector3(_target.position.x - (offset.x * _target.localScale.x), _target.position.y - offset.y);
        targetPosition.z = transform.position.z;
        if (_positionSmooth != -1f) transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime/_positionSmooth);
        if (_rotationSmooth != -1f) transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, Time.deltaTime / _rotationSmooth);
    }
}
