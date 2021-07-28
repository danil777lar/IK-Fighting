using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;

public class PanelHost : MonoBehaviour
{
    [SerializeField] private Panel _panel;
    [SerializeField] private TextMeshProUGUI _ip;

    private void Awake()
    {
        _panel.onPanelShow += HandleOnPanelShow;
        _panel.onPanelHide += () => NetworkManager.Singleton.OnClientConnectedCallback -= HandleOnClientConnected;
    }

    private void HandleOnPanelShow()
    {
        _ip.text = IPManager.GetIP(ADDRESSFAM.IPv4);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.OnClientConnectedCallback += HandleOnClientConnected;
    }

    private void HandleOnClientConnected(ulong l) 
    {
        UIManager.Default.CurentState = UIManager.State.Start;
    }
}
