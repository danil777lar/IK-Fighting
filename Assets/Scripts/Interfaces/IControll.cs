using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControll
{
    Transform GetArmRoot();

    bool GetMoveRight();

    bool GetMoveLeft();

    bool GetJump();

    bool GetMoveDown();

    bool GetAttackButtonDown();

    bool GetAttackButton();

    Vector2 GetAttackButtonNormal();
}
