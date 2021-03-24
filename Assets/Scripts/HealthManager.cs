using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    //REFS
    //[SerializeField] private HealthBar _healthBar;
    [SerializeField] GameObject _damageText;
    [SerializeField] RectTransform _canvas;

    //VALUES
    [SerializeField] private int _health;

    public void SetDamage(int damage, int maxDamage)
    {
        _health -= damage;

        GameObject text = Instantiate(_damageText);
        text.GetComponent<DamageText>().Init(damage, maxDamage);

        RectTransform textTransform = text.GetComponent<RectTransform>();
        textTransform.SetParent(_canvas);
        textTransform.offsetMax = new Vector2(0f, 0f);
        textTransform.offsetMin = new Vector2(0f, 0f);
    }
}
