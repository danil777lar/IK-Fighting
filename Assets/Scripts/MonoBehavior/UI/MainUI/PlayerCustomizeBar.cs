using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerCustomizeBar : MonoBehaviour
{
    [HideInInspector] public Transform linkedButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private Transform _buttonsHolder;

    private float size;

    public Action onClose = () => { };

    private void Start()
    {
        _closeButton.onClick.AddListener(CLosePanel);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        transform.position = linkedButton.position;
        size = (_buttonsHolder.GetComponentsInChildren<Button>(true).Length * 60f) + 60f;

        ((RectTransform)transform).DOAnchorPos(new Vector2(0f, 150f), 0.5f)
            .SetEase(Ease.OutBack);
        DOTween.To(
            () => 100f,
            (v) => ((RectTransform)transform).sizeDelta = new Vector2(v, 100f),
            size, 0.5f)
            .SetEase(Ease.OutBack);
        DOTween.To(
            () => 0f,
            (v) => _group.alpha = v,
            1f, 0.2f);
    }

    public void CLosePanel()
    {
        transform.DOMove(linkedButton.position, 0.5f)
            .OnComplete(() => gameObject.SetActive(false));
        DOTween.To(
            () => size,
            (v) => ((RectTransform)transform).sizeDelta = new Vector2(v, 100f),
            100f, 0.5f);
        DOTween.To(
            () => 1f,
            (v) => _group.alpha = v,
            0f, 0.2f);
        onClose?.Invoke();
    }
}
