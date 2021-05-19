using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBotControll : MonoBehaviour, IControll
{
    [SerializeField] private Transform _armRoot;

    private DirectionController _directionController;

    public void Awake()
    {
        _directionController = GetComponent<DirectionController>();
    }

    #region IControll
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
        return Input.GetKey("up");
    }

    public bool GetMoveLeft()
    {
        return Input.GetKey("left");
    }

    public bool GetMoveRight()
    {
        return Input.GetKey("right");
    }

    public bool GetMoveDown()
    {
        return Input.GetKey("down");
    }
    #endregion
}
