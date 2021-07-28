using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(Panel))]
public class PanelStart : NetworkBehaviour
{
    [SerializeField] private Panel _panel;
    [Space]
    [SerializeField] private Button _weaponsButton;
    [SerializeField] private Button _skinsButton;
    [SerializeField] private PlayerCustomizeBar _weaponsPanel;
    [SerializeField] private PlayerCustomizeBar _skinsPanel;
    [Space]
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private TextMeshProUGUI _hostNick;
    [SerializeField] private TextMeshProUGUI _clientNick;
    [Space]
    [SerializeField] private PanelStartNetVariables _varPrefab;

    [HideInInspector] public PanelStartNetVariables netVar;

    private Image _hostIndicator;
    private Image _clientIndicator;

    private void Awake()
    {
        _panel.onPanelShow += HandlePanelShow;
        _panel.onPanelHide += HandlePanelHide;
    }

    private void Start()
    {
        _weaponsPanel.linkedButton = _weaponsButton.transform;
        _skinsPanel.linkedButton = _skinsButton.transform;

        _weaponsButton.onClick.AddListener(() => OpenCustomizeBar(_weaponsPanel));
        _skinsButton.onClick.AddListener(() => OpenCustomizeBar(_skinsPanel));

        _weaponsPanel.onClose += () => SwitchButtons(true);
        _skinsPanel.onClose += () => SwitchButtons(true);

        _hostIndicator = _hostButton.GetComponent<Image>();
        _clientIndicator = _clientButton.GetComponent<Image>();
    }

    private void Update() 
    {
        if (netVar != null)
        {
            Color hostColor = _hostIndicator.color;
            hostColor.a = netVar.host.Value ? 1f : 0.1f;
            _hostIndicator.color = hostColor;

            Color clientColor = _clientIndicator.color;
            clientColor.a = netVar.client.Value ? 1f : 0.1f;
            _clientIndicator.color = clientColor;

            _hostNick.text = netVar.hostName.Value;
            _clientNick.text = netVar.clientName.Value;

            if (netVar.host.Value && netVar.client.Value)
            {
                UIManager.Default.CurentState = UIManager.State.Process;
                LayerDefault.Default.IsPlaying = true;
            }
        }
    }

    private void HandlePanelShow() 
    {
        if (IsHost)
        {
            Instantiate(_varPrefab.gameObject);
            _hostButton.onClick.AddListener(() => netVar.host.Value = netVar.host.Value ? false : true);
        }
        else 
        {
            _clientButton.onClick.AddListener(() => netVar.client.Value = netVar.client.Value ? false : true);
        }

        _clientButton.interactable = IsClient;
        _hostButton.interactable = IsHost;
    }

    private void HandlePanelHide()
    {
        if (IsHost && netVar != null)
            Destroy(netVar.gameObject);
        netVar = null;
        _hostButton.onClick.RemoveAllListeners();
        _clientButton.onClick.RemoveAllListeners();
    }

    private void OpenCustomizeBar(PlayerCustomizeBar bar) 
    {
        bar.OpenPanel();
        SwitchButtons(false);
    }

    public void SwitchButtons(bool arg) 
    {
        foreach (Button b in new Button[] { _weaponsButton, _skinsButton }) 
        {
            if (arg) b.gameObject.SetActive(true);
            b.GetComponent<Image>().DOColor(arg ? Color.white : new Color(0f, 0f, 0f, 0f), 0.2f)
                .OnComplete(() => b.gameObject.SetActive(arg));

        }
    }

    private void HandleOnButtonStartClick() 
    {
        if (IsHost)
            netVar.host.Value = netVar.host.Value ? false : true;
        else
            netVar.client.Value = netVar.client.Value ? false : true;
    }
}
