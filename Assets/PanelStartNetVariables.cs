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

        host.Value = false;
        client.Value = false;
    }

    private void Start()
    {
        if (IsHost) GetComponent<NetworkObject>().Spawn();
        PanelStart panel = UIManager.Default.GetComponentInChildren<PanelStart>(true);
        transform.SetParent(panel.transform);
        panel.netVar = this;

        if (IsHost) hostName.Value = LayerDefault.Default.Nickname;
        if (IsClient) clientName.Value = LayerDefault.Default.Nickname;
    }
}
