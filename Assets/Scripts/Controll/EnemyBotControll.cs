using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBotControll : MonoBehaviour, IControll
{
    public bool GetAttackButtonDown(int button)
    {
        return false;
    }

    public bool GetAttackButtonUp(int button)
    {
        return false;
    }

    public float GetAttackButtonAngle(int button)
    {
        return 0f;
    }

    public bool GetJump()
    {
        return false;
    }

    public bool GetMoveDown()
    {
        return false;
    }

    public bool GetMoveLeft()
    {
        return false;
    }

    public bool GetMoveRight()
    {
        return false;
    }
}
