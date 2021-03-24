using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    //REFS
    private Text _text;
    private RectTransform _rectTransform;

    //VALUES
    [SerializeField] private Color _minDamageColor;
    [SerializeField] private Color _maxDamageColor;

    private Vector2 _startPosition;
    private bool _isInit = false;
    private int _damage;
    private int _maxDamage;
    private float _startTime;
    private float _duration = 1f;

    void Start()
    {
        _text = GetComponent<Text>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(int damage, int maxDamage)
    {
        _damage = damage;
        _maxDamage = maxDamage;
    }


    void Update()
    {
        if (_isInit)
        {
            if (Time.time - _startTime >= _duration) Destroy(gameObject);
            else 
            {
                float timePassed = Time.time - _startTime;
                float t = timePassed / _duration;
                _rectTransform.localPosition = _startPosition + new Vector2(Mathf.Sin((timePassed*3f)/_duration), timePassed/_duration);

                Color color = _text.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                _text.color = color;
            }
        }
        else 
        {
            _isInit = true;
            _startTime = Time.time;
            _startPosition = _rectTransform.localPosition;

            float scale = (float)_damage / (float)_maxDamage;
            _text.color = Color.Lerp(_minDamageColor, _maxDamageColor, scale);
            _text.text = "" + _damage;

            _duration *= scale + 1f;

            _rectTransform.localScale = new Vector3(scale+1f, scale+1f, 1f);
        }
    }
}
