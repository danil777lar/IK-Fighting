using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using LarjeEnum;

public class PlayerNetworkSpawner : NetworkBehaviour
{
    #region Singleton
    private static PlayerNetworkSpawner _default;
    public static PlayerNetworkSpawner Default => _default;
    #endregion

    [SerializeField] private Transform _body;

    private NetworkVariableVector3 _ownerPosition;

    public Transform Body => _body;

    private void Awake()
    {
        _ownerPosition = new NetworkVariableVector3(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.OwnerOnly
        });
    }

    private void Start()
    {
        if (!IsOwner)
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else _default = this;

        GetComponent<DirectionController>().Connect();
        if (IsOwnedByServer) transform.position = LevelController.Default.HostSpawnPosition;
        else transform.position = LevelController.Default.ClientSpawnPosition;

        /*if (!IsLocalPlayer) 
        {
            GetComponent<FightController>().Start();
            GetComponent<FightController>().enabled = false;
            GetComponent<PhysicsMachine>().enabled = false;
            GetComponent<PointerFiller>().enabled = false;
            GetComponent<PhysicsMachine>().enabled = false;
            GetComponentInChildren<Weapon>().enabled = false;
            foreach (HingeJoint2D hinge in GetComponentsInChildren<HingeJoint2D>()) 
                hinge.enabled = false;
            foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
                rb.simulated = false;
            foreach (ProcedureAnimation rb in GetComponentsInChildren<ProcedureAnimation>())
                rb.enabled = false;
        }*/
    }

    private void FixedUpdate()
    {
        string x = _body.transform.position.x.ToString();
        string y = _body.transform.position.y.ToString();
        string z = _body.transform.position.z.ToString();

        if ((IsOwner && IsHost) || (!IsOwner && !IsHost))
            PanelProcess.Default.text.text = $"x : {x}\ny : {y}\nz : {z}";

        if (IsOwner) _ownerPosition.Value = _body.transform.position;
        else 
        {
            float dist = Vector3.Distance(_body.transform.position, _ownerPosition.Value);
            if (dist >= 0.5f) 
            {
                transform.position += _ownerPosition.Value - _body.transform.position;
            }
        }
    }
}
