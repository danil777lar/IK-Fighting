using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Slider))]
public class AttackForceIndicator : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _finishColor;

    private IProgressInformation _progressInfo;
    public IProgressInformation ProgressInfo 
    {
        get => _progressInfo;
        set
        {
            _progressInfo = value;
            if (value == null)
            {
                _rectTransform.DOScale(Vector3.zero, 0.5f)
                    .SetEase(Ease.InQuint);
            }
            else 
            {
                _rectTransform.DOScale(Vector3.one, 0.5f)
                    .SetEase(Ease.OutBack);
            }
        }
    }

    private Slider _slider;
    private RectTransform _rectTransform;
    private Image _fillArea;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        _fillArea = GetComponentsInChildren<Image>()[1];

        _rectTransform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (_progressInfo != null)
        {
            _slider.value = _progressInfo.GetProgress();
            _fillArea.color = Color.Lerp(_startColor, _finishColor, _progressInfo.GetProgress());
        }
    }
}
