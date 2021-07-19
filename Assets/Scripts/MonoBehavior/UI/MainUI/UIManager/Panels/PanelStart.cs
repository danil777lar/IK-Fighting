using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Panel))]
public class PanelStart : MonoBehaviour
{
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _weaponsButton;
    [SerializeField] private Button _skinsButton;
    [SerializeField] private PlayerCustomizeBar _weaponsPanel;
    [SerializeField] private PlayerCustomizeBar _skinsPanel;

    [SerializeField] private Panel _panel;


    private void Start()
    {
        _buttonStart.onClick.AddListener(HandleOnButtonStartClick);

        _weaponsPanel.linkedButton = _weaponsButton.transform;
        _skinsPanel.linkedButton = _skinsButton.transform;

        _weaponsButton.onClick.AddListener(() => OpenCustomizeBar(_weaponsPanel));
        _skinsButton.onClick.AddListener(() => OpenCustomizeBar(_skinsPanel));

        _weaponsPanel.onClose += () => SwitchButtons(true);
        _skinsPanel.onClose += () => SwitchButtons(true);
    }

    private void OpenCustomizeBar(PlayerCustomizeBar bar) 
    {
        bar.OpenPanel();
        SwitchButtons(false);
    }

    public void SwitchButtons(bool arg) 
    {
        foreach (Button b in new Button[] { _buttonStart, _weaponsButton, _skinsButton }) 
        {
            if (arg) b.gameObject.SetActive(true);
            b.GetComponent<Image>().DOColor(arg ? Color.white : new Color(0f, 0f, 0f, 0f), 0.2f)
                .OnComplete(() => b.gameObject.SetActive(arg));

        }
    }

    private void HandleOnButtonStartClick() 
    {
        UIManager.Default.CurentState = UIManager.State.Process;
        LayerDefault.Default.IsPlaying = true;
    }
}
