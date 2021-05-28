using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllInterface : MonoBehaviour, IControll
{
    [SerializeField] private Transform _armRoot;

    #region Icontroll
    public bool GetAttackButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }

    public bool GetAttackButton(int button)
    {
        return Input.GetMouseButton(button);
    }

    public Vector2 GetAttackButtonNormal(int button)
    {
        if (Input.GetMouseButton(button))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Debug.DrawLine(mousePosition, (Vector2)_armRoot.position, Color.red);
            return ((Vector2)(mousePosition - _armRoot.position)).normalized;
        }
        else return Vector2.zero;
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

    public Transform GetArmRoot() => _armRoot;
    #endregion
}
