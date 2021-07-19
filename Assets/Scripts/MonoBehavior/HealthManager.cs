using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] GameObject _damageText;
    [SerializeField] RectTransform _canvas;

    private bool _dead;
    private Slider _slider;

    public delegate void PlayerDeath(int damage, Vector2 direction);
    public event PlayerDeath OnPlayerDeath;

    public void SetupSlider(Slider slider) 
    {
        _slider = slider;
        _slider.maxValue = _health;
        _slider.value = _health;
    }

    public void SetDamage(int damage, int maxDamage, Vector2 direction)
    {
        _health -= damage;

        _slider.DOValue(_health, 0.2f);

        GameObject text = Instantiate(_damageText);
        text.GetComponent<DamageText>().Init(damage, maxDamage);

        RectTransform textTransform = text.GetComponent<RectTransform>();
        textTransform.SetParent(_canvas);
        textTransform.offsetMax = new Vector2(0f, 0f);
        textTransform.offsetMin = new Vector2(0f, 0f);

        if (_health <= 0) 
        {
            if (OnPlayerDeath != null)
                OnPlayerDeath.Invoke(damage, direction);
            else
                Death(direction);
        }
    }

    public void SetHeal(int heal) 
    {
        _health += heal;
        _slider.DOValue(_health, 0.2f);
    }

    private void Death(Vector2 direction) 
    {
        UIManager.Default.CurentState = UIManager.State.End;
        LayerDefault.Default.IsPlaying = false;
        Destroy(GetComponent<PhysicsMachine>());
        foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>()) 
        {
            rb.simulated = true;
            rb.isKinematic = false;
            rb.mass = 1f;
            Collider2D collider = rb.GetComponent<Collider2D>();
            if (collider != null) collider.isTrigger = false;
        }
        foreach (ProcedureAnimation anim in GetComponentsInChildren<ProcedureAnimation>())
            Destroy(anim);

        Rigidbody2D body = GetComponentInChildren<BodyDamageTracker>().GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.velocity = direction * 10f;
    }
}
