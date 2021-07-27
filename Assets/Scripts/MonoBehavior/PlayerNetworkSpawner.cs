using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using LarjeEnum;

public class PlayerNetworkSpawner : MonoBehaviour
{
    private NetworkObject _netObject;

    void Start()
    {
        _netObject = GetComponent<NetworkObject>();
        if (!_netObject.IsOwner)
            foreach (Transform t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer("Enemy");
        else 
            SmoothCamera.Default.Init(GetComponent<PointerFiller>().GetPointer(KinematicsPointerType.Body).transform);

        GetComponent<DirectionController>().Connect();
    }

    void Update()
    {
    }
}
