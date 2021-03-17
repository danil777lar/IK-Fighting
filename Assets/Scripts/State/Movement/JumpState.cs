using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/States/JumpState")]

public class JumpState : State
{
    private Rigidbody2D _bodyRb; 

    protected override void Start()
    {
        _frontLegPointer.SetMotion( new JumpLegMotion(_bodyPointer.transform) );
        _backLegPointer.SetMotion( new JumpLegMotion(_bodyPointer.transform) );

        _bodyRb = _bodyPointer.GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        if (_bodyRb.transform.position.y <= -0.4f)
        {
            _bodyRb.drag = 1f;
            _stateHolder.SetState(Resources.Load<State>("DefaultState"));
        }
    }
}
