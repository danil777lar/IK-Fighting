using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    [SerializeField] private DirectionController _otherController;
    [SerializeField] private Transform _body;

    public float BodyPosition { get => _body.position.x; }
    public float Direction { get => _body.localScale.x; }

    private void Update()
    {
        if (_otherController != null && _body != null)
        {
            if (_otherController.BodyPosition > BodyPosition) _body.transform.localScale = new Vector2(1f, 1f);
            else if (_otherController.BodyPosition < BodyPosition) _body.transform.localScale = new Vector2(-1f, 1f);
        }
    }

    public void Connect()
    {
        if (_otherController == null)
        {
            List<DirectionController> all = new List<DirectionController>();
            all.AddRange(FindObjectsOfType<DirectionController>());
            DirectionController other = all.Find((c) => c != this);
            if (other != null)
            {
                _otherController = other;
                other.Connect();
            }
        }
    }
}
