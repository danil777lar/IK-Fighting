using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class UserControllInterface : NetworkBehaviour, IControll
{
    [SerializeField] private Transform _armRoot;

    private NetworkObject _netObj;
    private NetworkVariableBool _jump;
    private NetworkVariableBool _left;
    private NetworkVariableBool _right;
    private NetworkVariableBool _down;
    private NetworkVariableBool _attack;
    private NetworkVariableBool _attackDown;
    private NetworkVariableVector2 _attackNormal;

    private void Awake()
    {
        NetworkVariableSettings sets = new NetworkVariableSettings()
        {
            WritePermission = NetworkVariablePermission.OwnerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        };

        _jump = new NetworkVariableBool(sets);
        _left = new NetworkVariableBool(sets);
        _right = new NetworkVariableBool(sets);
        _down = new NetworkVariableBool(sets);
        _attack = new NetworkVariableBool(sets);
        _attackDown = new NetworkVariableBool(sets);
        _attackNormal = new NetworkVariableVector2(sets);
    }

    private void Start()
    {
        _netObj = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if (_netObj.IsOwner) 
        {
            _jump.Value = Input.GetKey("w") && LayerDefault.Default.IsPlaying;
            _left.Value = Input.GetKey("a") && LayerDefault.Default.IsPlaying;
            _right.Value = Input.GetKey("d") && LayerDefault.Default.IsPlaying;
            _down.Value = Input.GetKey("s") && LayerDefault.Default.IsPlaying;
            _attack.Value = Input.GetMouseButton(0) && LayerDefault.Default.IsPlaying;
            _attackDown.Value = Input.GetMouseButtonDown(0) && LayerDefault.Default.IsPlaying;

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 10f;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Debug.DrawLine(mousePosition, (Vector2)_armRoot.position, Color.red);
                _attackNormal.Value = ((Vector2)(mousePosition - _armRoot.position)).normalized;
            }
            else _attackNormal.Value = Vector2.zero;
        }
    }

    #region Icontroll
    public bool GetJump()
    {
        return _jump.Value;
    }

    public bool GetMoveLeft()
    {
        return _left.Value;
    }

    public bool GetMoveRight()
    {
        return _right.Value;
    }

    public bool GetMoveDown() 
    {
        return _down.Value;
    }

    public bool GetAttackButton()
    {
        return _attack.Value;
    }

    public bool GetAttackButtonDown()
    {
        return _attackDown.Value;
    }

    public Vector2 GetAttackButtonNormal()
    {
        return _attackNormal.Value;
    }

    public Transform GetArmRoot() => _armRoot;
    #endregion
}
