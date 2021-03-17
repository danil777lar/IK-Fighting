using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHolder : MonoBehaviour
{
    [SerializeField] private KinematicsPointer _bodyPointer;
    [SerializeField] private KinematicsPointer _frontArmPointer;
    [SerializeField] private KinematicsPointer _backArmPointer;
    [SerializeField] private KinematicsPointer _frontLegPointer;
    [SerializeField] private KinematicsPointer _backLegPointer;

    [SerializeField] private State _defaultState;

    private State _currentState;

    void Start()
    {
        SetState(_defaultState);
    }

    public void SetState(State newState, float duration = -1f)
    {
        _currentState = Instantiate(newState);
        _currentState.Init(this, _bodyPointer, _frontArmPointer, _backArmPointer, _frontLegPointer, _backLegPointer);

        if (duration != -1f) Invoke("SetDefaultState", duration);
    }

    private void SetDefaultState()
    {
        SetState(_defaultState);
    }

    void Update()
    {
        _currentState.Update();
    }
}
