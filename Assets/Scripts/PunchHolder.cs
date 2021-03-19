using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StateHolder))]
[RequireComponent(typeof(IControll))]

public class PunchHolder : MonoBehaviour
{
    [SerializeField] private Transform _armRoot;

    private IControll _controllInterface;

    private State _frontArmState;
    private State _backArmState;
    private State _techniqueState;

    private StateHolder _stateHolder;

    void Start()
    {
        _frontArmState = new SimplePunch(Punch.FRONT_HAND, _armRoot);
        _backArmState = new SimplePunch(Punch.BACK_HAND, _armRoot);

        _stateHolder = GetComponent<StateHolder>();
        _controllInterface = GetComponent<IControll>();
    }

    void Update()
    {
        if (_controllInterface.GetAttackButtonDown(0) && _backArmState != null) _stateHolder.SetState( _backArmState );
        if (_controllInterface.GetAttackButtonDown(1) && _frontArmState != null) _stateHolder.SetState( _frontArmState );
        if (_controllInterface.GetAttackButtonDown(2) && _techniqueState != null) _stateHolder.SetState( _techniqueState );
    }
}
