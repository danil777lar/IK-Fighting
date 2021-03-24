using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AttackForceIndicator : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _finishColor;

    public IProgressInformation _progressInfo;

    private Slider _slider;
    private RectTransform _rectTransform;
    private Image _fillArea;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        _fillArea = GetComponentsInChildren<Image>()[1];
    }

    void Update()
    {
        if (_progressInfo == null) _rectTransform.localScale = new Vector2(0f, 0f);
        else 
        {
            _rectTransform.localScale = new Vector2(1f, 1f);
            _slider.value = _progressInfo.GetProgress();
            _fillArea.color = Color.Lerp(_startColor, _finishColor, _progressInfo.GetProgress());
        }
    }
}
