using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _default;
    public static Player Default => _default;

    private FightController _fightController;

    public FightController FightController => _fightController;

    private void Awake() 
    {
        _default = this;
    }

    private void Start()
    {
        _fightController = GetComponent<FightController>();
    }
}
