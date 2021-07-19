using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllInterface : MonoBehaviour, IControll
{
    [SerializeField] private Transform _armRoot;

    #region Icontroll
    public bool GetAttackButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button) && LayerDefault.Default.IsPlaying;
    }

    public bool GetAttackButton(int button)
    {
        return Input.GetMouseButton(button) && LayerDefault.Default.IsPlaying;
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
        return Input.GetKey("w") && LayerDefault.Default.IsPlaying;
    }

    public bool GetMoveLeft()
    {
        return Input.GetKey("a") && LayerDefault.Default.IsPlaying;
    }

    public bool GetMoveRight()
    {
        return Input.GetKey("d") && LayerDefault.Default.IsPlaying;
    }

    public bool GetMoveDown() 
    {
        return Input.GetKey("s") && LayerDefault.Default.IsPlaying;
    }

    public Transform GetArmRoot() => _armRoot;
    #endregion
}
