using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChangePanel : MonoBehaviour
{
    [SerializeField] private Image _back;
    [SerializeField] private Image _stripeBack;
    [SerializeField] private Image _stripeBounds;
    [SerializeField] private Text _stripeText;
    [SerializeField] private CanvasGroup _group;


    void Start()
    {
        Close();
    }

    public void Show() 
    {
        transform.localScale = Vector2.one;
        Color backColor = _back.color;
        backColor.a = 1f;

        DOTween.Sequence()
            .Append(_group.DOFade(1f, 0.2f))
            .Append(transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack))
            .Append(_back.DOColor(backColor, 0.3f)
                .OnComplete(() => Application.LoadLevel("Scenes/SampleScene")));
    }

    public void Close() 
    {
        transform.localScale = Vector2.one;
        Color backColor = _back.color;
        backColor.a = 0f;

        DOTween.Sequence()
            .Append(_back.DOColor(backColor, 0.3f))
            .Append(transform.DOScale(2f, 0.5f).SetEase(Ease.InBack))
            .Append(_group.DOFade(0f, 0.2f));
    }
}
