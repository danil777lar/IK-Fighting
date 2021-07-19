﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum State {Start, Process, End}

    private static UIManager _default;
    public static UIManager Default => _default;

    [SerializeField] private Panel _startPanel;
    [SerializeField] private Panel _processPanel;
    [SerializeField] private Panel _endPanel;

    private Dictionary<State, Panel> _stateToPanel;
    private State _curentState;

    public State CurentState 
    {
        get => _curentState;
        set
        {
            if (_curentState != value)
            {
                _stateToPanel[value].ShowPanel();
                _stateToPanel[_curentState].HidePanel();
                _curentState = value;
            }
        }
    }

    private void Awake()
    {
        _default = this;

        _stateToPanel = new Dictionary<State, Panel>();
        _stateToPanel.Add(State.Start, _startPanel);
        _stateToPanel.Add(State.Process, _processPanel);
        _stateToPanel.Add(State.End, _endPanel);
    }
}
