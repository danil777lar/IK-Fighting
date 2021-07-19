using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Panel))]
public class PanelEnd : MonoBehaviour
{
    [SerializeField] private Panel _panel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private SceneChangePanel _sceneChange;

    private void Start()
    {
        _restartButton.onClick.AddListener(() => _sceneChange.Show());
    }
}
