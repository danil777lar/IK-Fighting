using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;

public class PanelClient : MonoBehaviour
{
    [SerializeField] private Panel _panel;
    [SerializeField] private Button _connectButton;
    [SerializeField] private TMP_InputField _input;

    private void Start()
    {
        _connectButton.onClick.AddListener(HandleOnStart);
    }

    private void HandleOnStart() 
    {
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = _input.text;
        NetworkManager.Singleton.StartClient();
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsConnectedClient) 
        {
            UIManager.Default.CurentState = UIManager.State.Start;
            return;
        }
    }


}
