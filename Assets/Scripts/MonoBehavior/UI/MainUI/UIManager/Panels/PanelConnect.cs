using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelConnect : MonoBehaviour
{
    [SerializeField] private Panel _panel;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private TMP_InputField _nick;

    private void Awake()
    {
        _panel.onPanelShow += HandlePanelShow;
    }

    private void Start()
    {
        _hostButton.onClick.AddListener(() => UIManager.Default.CurentState = UIManager.State.Host);
        _clientButton.onClick.AddListener(() => UIManager.Default.CurentState = UIManager.State.Client);
        _nick.onValueChanged.AddListener((s) => LayerDefault.Default.Nickname = _nick.text);
        _nick.text = LayerDefault.Default.Nickname;
    }

    private void HandlePanelShow() 
    {
        _nick.text = LayerDefault.Default.Nickname;
    }
}
