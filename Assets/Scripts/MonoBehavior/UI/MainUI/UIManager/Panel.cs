using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class Panel : MonoBehaviour
{
    [SerializeField] private bool _hideOnStart;
    [SerializeField] private float _animDuration = 0.25f;
    [SerializeField] private CanvasGroup _group;

    public Action onPanelShow = () => { };
    public Action onPanelHide = () => { };

    public void Start()
    {
        _group = GetComponent<CanvasGroup>();
        gameObject.SetActive(!_hideOnStart);
    }

    public void ShowPanel() 
    {
        gameObject.SetActive(true);
        onPanelShow.Invoke();
        transform.localScale = Vector3.one * 1.2f;
        transform.DOScale(1f, _animDuration);
        DOTween.To(
            () => 0f,
            (v) => _group.alpha = v,
            1f, _animDuration);
    }

    public void HidePanel() 
    {
        onPanelHide.Invoke();
        transform.localScale = Vector3.one;
        transform.DOScale(1.2f, _animDuration);
        DOTween.To(
            () => 1f,
            (v) => _group.alpha = v,
            0f, _animDuration)
                .OnComplete(() => gameObject.SetActive(false));
    }
}
