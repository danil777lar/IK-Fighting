using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class PanelStartNetVariables : NetworkBehaviour
{
    public NetworkVariableBool host;
    public NetworkVariableBool client;
    public NetworkVariableString hostName;
    public NetworkVariableString clientName;
    public NetworkVariableBool clientReadyToDestroy;

    private void Awake()
    {
        NetworkVariableSettings sets = new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.Everyone,
        };
        host = new NetworkVariableBool(sets);
        client = new NetworkVariableBool(sets);
        hostName = new NetworkVariableString(sets);
        clientName = new NetworkVariableString(sets);
        clientReadyToDestroy = new NetworkVariableBool(sets);
    }

    private void Start()
    {
        if (IsHost) 
        {
            GetComponent<NetworkObject>().Spawn();
            if (PanelStart.Default.netVar != null)
                Destroy(PanelStart.Default.netVar.gameObject);
        }

        transform.SetParent(PanelStart.Default.transform);
        PanelStart.Default.netVar = this;

        if (IsHost) hostName.Value = LayerDefault.Default.Nickname;
        if (IsClient) clientName.Value = LayerDefault.Default.Nickname;
    }

    public void StartGame()
    {
        if (IsClient && !clientReadyToDestroy.Value) 
        {
            Debug.Log("Client start");
            UIManager.Default.CurentState = UIManager.State.Process;
            LayerDefault.Default.IsPlaying = true;
        }
        if (IsHost && clientReadyToDestroy.Value) 
        {
            UIManager.Default.CurentState = UIManager.State.Process;
            LayerDefault.Default.IsPlaying = true;
            Destroy(gameObject);
        }            
    }
}
