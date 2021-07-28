using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using LarjeEnum;

public class PlayerNetworkSpawner : NetworkBehaviour
{
    [SerializeField] private List<GameObject> objectsToDelete = new List<GameObject>();

    void Start()
    {
        if (IsLocalPlayer)
            SmoothCamera.Default.Init(GetComponent<PointerFiller>().GetPointer(KinematicsPointerType.Body).transform);
        else
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        GetComponent<DirectionController>().Connect();



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

        }
    }
}
