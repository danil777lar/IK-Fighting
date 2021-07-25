using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Panel))]
public class PanelProcess : MonoBehaviour
{
    [SerializeField] private Slider _playerSlider;
    [SerializeField] private Slider _enemySlider;

    [SerializeField] private Panel _panel;

    private void Awake()
    {
        _panel.onPanelShow += HandleOnPanelShow;
    }

    private void HandleOnPanelShow() 
    {
        Debug.Log("Process panel showed");
        foreach (HealthManager health in FindObjectsOfType<HealthManager>())
            health.SetupSlider(health.gameObject.layer == LayerMask.NameToLayer("Player") ? _playerSlider : _enemySlider);
    }
}
