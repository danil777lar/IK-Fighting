using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControll
{
    bool GetMoveRight();

    bool GetMoveLeft();

    bool GetJump();

    bool GetMoveDown();

    bool GetAttackButtonDown(int button);

    bool GetAttackButton(int button);

    float GetAttackButtonAngle(int button);
}
