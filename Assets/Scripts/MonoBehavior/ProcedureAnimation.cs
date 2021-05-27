using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureAnimation : MonoBehaviour
{
    [SerializeField] private Transform _pointer;
    [SerializeField] private bool _isSpriteReverse;
    [SerializeField] private float _angleLimitMin;
    [SerializeField] private float _angleLimitMax;

    private DirectionController _directionController;
    private List<Transform> _segments;
    private Dictionary<Transform, float> _segmentsLenght;
    private SpriteRenderer[] _spriteRenderers;

    public Transform Pointer
    {
        get => _pointer;
        protected set => _pointer = value;
    }

    private void Start()
    {
        _directionController = GetComponentInParent<DirectionController>();
        CalculateSegments();
    }

    private void FixedUpdate()
    {
        _segments[0].rotation = Quaternion.Euler(0f, 0f, 0f);

        RotateAndMove();
        CheckLimits();
        MoveToRoot();
    }

    public void CalculateSegments() 
    {
        _segments = new List<Transform>();
        _segments.Add(transform);
        foreach (Transform t in transform)
            _segments.Add(t);

        _spriteRenderers = new SpriteRenderer[] { _segments[1].GetComponent<SpriteRenderer>(), _segments[2].GetComponent<SpriteRenderer>() };

        _segmentsLenght = new Dictionary<Transform, float>();
        for (int i = 0; i < _segments.Count - 1; i++)
            _segmentsLenght.Add(_segments[i], Vector2.Distance(_segments[i].position, _segments[i + 1].position));
    }

    private void RotateAndMove()
    {
        for (int i = _segments.Count - 1; i >= 1; i--)
        {
            Transform target = GetTarget(i);
            Transform segment = _segments[i];

            if (i == _segments.Count - 1) segment.position = target.position;
            else
            {
                Vector2 direction = target.position - segment.position;
                float a = Vector2.SignedAngle(direction, Vector2.down) * -1f;
                segment.rotation = Quaternion.Euler(0f, 0f, a);
                segment.position += (Vector3)direction.normalized * (direction.magnitude - _segmentsLenght[segment]);
            }
        }
    }

    private void CheckLimits()
    {
        // CHECK LIMITS
        Vector3 topDir = _segments[1].position - _segments[2].position;
        Vector3 downDir = _segments[2].position - _segments[3].position;

        float limitMin = _directionController.Direction == 1 ? _angleLimitMin : _angleLimitMax * -1f;
        float limitMax = _directionController.Direction == 1 ? _angleLimitMax : _angleLimitMin * -1f;

        bool flipSprite = false;
        float angle = Vector2.SignedAngle(topDir, downDir);
        if (angle < limitMin || angle > limitMax)
        {
            flipSprite = _isSpriteReverse;
            if (!_isSpriteReverse)
            {
                Vector3 globalDir = _segments[3].position - _segments[1].position;
                _segments[0].rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(globalDir, Vector2.right));
                float yRootOffset = _segments[2].position.y - _segments[1].position.y;
                _segments[2].position = new Vector2(_segments[2].position.x, _segments[1].position.y - yRootOffset);
                _segments[0].rotation = Quaternion.Euler(0f, 0f, 0f);
                RotateAndMove();
            }
        }

        foreach (SpriteRenderer sprite in _spriteRenderers)
            sprite.flipX = flipSprite;
    }

    private void MoveToRoot()
    {
        //MOVE TO ROOT
        Vector3 offset = _segments[1].localPosition;
        for (int i = 1; i < _segments.Count; i++)
            _segments[i].localPosition -= offset;
    }

    private Transform GetTarget(int i)
    {
        if (i == _segments.Count - 1) return _pointer;
        else return _segments[i + 1];
    }
}