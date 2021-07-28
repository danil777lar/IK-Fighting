using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Panel))]
public class PanelProcess : MonoBehaviour
{
    private static PanelProcess _default;
    public static PanelProcess Default => _default;

    [SerializeField] private Slider _playerSlider;
    [SerializeField] private Slider _enemySlider;

    [SerializeField] private Panel _panel;

    public TextMeshProUGUI text;

    private void Awake()
    {
        _default = this;
        _panel.onPanelShow += HandleOnPanelShow;
    }

    private void HandleOnPanelShow() 
    {
        Debug.Log("Process panel showed");
        foreach (HealthManager health in FindObjectsOfType<HealthManager>())
            health.SetupSlider(health.gameObject.layer == LayerMask.NameToLayer("Player") ? _playerSlider : _enemySlider);
    }
}
