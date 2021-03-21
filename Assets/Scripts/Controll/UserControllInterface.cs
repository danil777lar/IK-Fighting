using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllInterface : MonoBehaviour, IControll
{
    [SerializeField] private Transform _armRoot;

    public bool GetAttackButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }

    public bool GetAttackButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    public float GetAttackButtonAngle(int button)
    {
        if (Input.GetMouseButton(button))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            return Vector2.SignedAngle(Camera.main.ScreenToWorldPoint(mousePosition) - _armRoot.position, Vector2.right);
        }
        else return -1f;
    }

    public bool GetJump()
    {
        return Input.GetKey("w");
    }

    public bool GetMoveLeft()
    {
        return Input.GetKey("a");
    }

    public bool GetMoveRight()
    {
        return Input.GetKey("d");
    }

    public bool GetMoveDown() 
    {
        return Input.GetKey("s");
    }
}
