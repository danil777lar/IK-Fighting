using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private float _positionSmooth;
    [SerializeField] private float _rotationSmooth;
    [SerializeField] private Vector3 _positionOffset;

    private Transform _target;

    private void Start()
    {
        Camera.main.fieldOfView = 40f;
        LayerDefault.Default.OnPlayStart += () =>
        {
            Camera.main.DOFieldOfView(70f, 1f)
                .SetEase(Ease.InQuint);
        };
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector3 offset = LayerDefault.Default.IsPlaying ? _positionOffset : -Vector3.up * 0.5f;
            offset.z = _positionOffset.z;
            Vector3 targetPosition = new Vector3(_target.position.x - (offset.x * _target.localScale.x), _target.position.y - offset.y);
            targetPosition.z = transform.position.z;
            if (_positionSmooth != -1f) transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / _positionSmooth);
            if (_rotationSmooth != -1f) transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, Time.deltaTime / _rotationSmooth);
        }
        else _target = PlayerNetworkSpawner.Default?.Body;
    }
}
