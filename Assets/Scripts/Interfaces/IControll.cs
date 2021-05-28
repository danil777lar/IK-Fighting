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

    bool GetAttackButtonDown(int button);

    bool GetAttackButton(int button);

    Vector2 GetAttackButtonNormal(int button);
}
