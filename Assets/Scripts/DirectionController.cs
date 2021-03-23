using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField] private DirectionController _otherController;
    [SerializeField] private Transform _body;

    public float BodyPosition { get => _body.position.x; }
    public float Direction { get => _body.localScale.x; }

    void Update()
    {
        if (_otherController != null && _body != null)
        {
            if (_otherController.BodyPosition > BodyPosition) _body.transform.localScale = new Vector2(1f, 1f);
            else if (_otherController.BodyPosition < BodyPosition) _body.transform.localScale = new Vector2(-1f, 1f);
        }
    }
}
