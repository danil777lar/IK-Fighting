using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : State
{
    public static readonly int BACK_HAND = 0;
    public static readonly int FRONT_HAND = 1;

    protected int _hand;
    protected KinematicsPointer _currentHandPointer;

    public Punch(int hand) 
    {
        _hand = hand;
    }

    protected override void Start()
    {
        if (_hand == BACK_HAND) _currentHandPointer = _backArmPointer;
        if (_hand == FRONT_HAND) _currentHandPointer = _frontArmPointer;
    }

    public override void Update()
    {
        
    }
}
