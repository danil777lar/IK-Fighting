using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] GameObject _damageText;
    [SerializeField] RectTransform _canvas;

    public delegate void PlayerDeath(int damage, Vector2 direction);
    public event PlayerDeath OnPlayerDeath;

    public void SetDamage(int damage, int maxDamage, Vector2 direction)
    {
        _health -= damage;

        GameObject text = Instantiate(_damageText);
        text.GetComponent<DamageText>().Init(damage, maxDamage);

        RectTransform textTransform = text.GetComponent<RectTransform>();
        textTransform.SetParent(_canvas);
        textTransform.offsetMax = new Vector2(0f, 0f);
        textTransform.offsetMin = new Vector2(0f, 0f);

        if (_health <= 0) OnPlayerDeath?.Invoke(damage, direction);
    }

    public void SetHeal(int heal) 
    {
        _health += heal;
    }
}
