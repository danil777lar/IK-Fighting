using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform _pointer;

    [SerializeField]
    private float _angleLimitMin;

    [SerializeField]
    private float _angleLimitMax;

    private Transform[] _segments;
    private Dictionary<Transform, float> _segmentsLenght;


    private void Start()
    {
        _segments = GetComponentsInChildren<Transform>();

        _segmentsLenght = new Dictionary<Transform, float>();
        for (int i = 0; i < _segments.Length - 1; i++) 
        {
            float dx = _segments[i].position.x - _segments[i + 1].position.x;
            float dy = _segments[i].position.y - _segments[i + 1].position.y;
            float lenght = Mathf.Sqrt((dx * dx) + (dy * dy));
            _segmentsLenght.Add(_segments[i], lenght);
        }
    }

    private void Update()
    {
        for (int i = _segments.Length - 1; i >= 1; i--)
        {
            Transform target = GetTarget(i);
            Transform segment = _segments[i];

            if (i == _segments.Length - 1) segment.position = target.position;
            else
            {
                // SET ROTATION
                Vector2 direction = target.position - segment.position;
                float a = Vector2.SignedAngle(direction, Vector2.down) * -1f;
                segment.localRotation = Quaternion.Euler(0f, 0f, a);

                // SET POSITION
                float dx = Mathf.Cos(Mathf.Deg2Rad * (a + 270f)) * _segmentsLenght[segment];
                float dy = Mathf.Sin(Mathf.Deg2Rad * (a + 270f)) * _segmentsLenght[segment];
                segment.position = target.position - new Vector3(dx, dy);
            }
        }

        // CHECK LIMITS
        Vector3 topDir = _segments[1].position - _segments[2].position;
        Vector3 downDir = _segments[2].position - _segments[3].position;

        float angle = Vector2.SignedAngle(topDir, downDir);
        if (angle < _angleLimitMin || angle > _angleLimitMax)
        {
            Vector3 globalDir = _segments[3].position - _segments[1].position;
            _segments[0].localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(globalDir, Vector2.right));
            float yRootOffset = _segments[2].position.y - _segments[1].position.y;
            _segments[2].position = new Vector2(_segments[2].position.x, _segments[1].position.y- yRootOffset);
            _segments[0].localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        //MOVE TO ROOT
        Vector3 offset = _segments[1].localPosition;
        for (int i = 1; i < _segments.Length; i++) 
            _segments[i].position -= offset;
    }

    private Transform GetTarget(int i)
    {
        if (i == _segments.Length - 1) return _pointer;
        else return _segments[i + 1];
    }
}