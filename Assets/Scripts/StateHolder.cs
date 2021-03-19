using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IControll))]

public class StateHolder : MonoBehaviour
{
    [SerializeField] private KinematicsPointer _bodyPointer;
    [SerializeField] private KinematicsPointer _frontArmPointer;
    [SerializeField] private KinematicsPointer _backArmPointer;
    [SerializeField] private KinematicsPointer _frontLegPointer;
    [SerializeField] private KinematicsPointer _backLegPointer;

    private IControll _controllInterface;

    private State _defaultState = new DefaultState();
    private State _currentState;

    /*DEBUG*/
    public string currentStateDebug;

    void Start()
    {
        _controllInterface = GetComponent<IControll>();
        SetState(_defaultState);
    }

    public void SetState(State newState, float duration = -1f)
    {
        _currentState = newState;
        _currentState.Init(this, _controllInterface, _bodyPointer, _frontArmPointer, _backArmPointer, _frontLegPointer, _backLegPointer);

        if (duration != -1f) Invoke("SetDefaultState", duration);
    }

    private void SetDefaultState()
    {
        SetState(_defaultState);
    }

    void Update()
    {
        _currentState.Update();

        currentStateDebug = ""+_currentState.GetType();/*DEBUG*/
    }
}
