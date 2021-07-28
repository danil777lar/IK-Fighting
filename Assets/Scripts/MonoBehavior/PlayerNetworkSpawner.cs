using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using LarjeEnum;

public class PlayerNetworkSpawner : NetworkBehaviour
{
    //[SerializeField] private List<GameObject> objectsToDelete = new List<GameObject>();
    private static PlayerNetworkSpawner _default;
    public static PlayerNetworkSpawner Default => _default;

    [SerializeField] private Transform _body;

    public Transform Body => _body;

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
/*

        if (!IsLocalPlayer) 
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
            foreach (GameObject go in objectsToDelete)
                Destroy(go);
        }

        if (!IsHost) 
        {

        }*/
    }
}
