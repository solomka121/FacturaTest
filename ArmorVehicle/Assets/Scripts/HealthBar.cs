using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bgFill;
    [SerializeField] private float _smoothTime = 0.2f;
    private float _velocity;

    [SerializeField] private SimpleScaler _scaler;

    private bool isActive = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_bgFill.fillAmount != _fill.fillAmount)
            _bgFill.fillAmount = Mathf.SmoothDamp(_bgFill.fillAmount, _fill.fillAmount, ref _velocity, _smoothTime);
    }

    private void Init()
    {
        _health.OnHealthChange += UpdateValue;
        UpdateValue(_health.currentHealth);
        gameObject.SetActive(false);
    }

    public void UpdateValue(float currentHealth)
    {
        _fill.fillAmount = currentHealth / _health.maxHealth;

        _scaler.PlayOnce();

        if (isActive)
            return;

        if (_fill.fillAmount < 1)
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public void UpdateValue()
    {
        UpdateValue(_health.currentHealth);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (DOTween.Kill(_canvasGroup) > 0)
        {
            Debug.LogWarning("Health bar didnt finish tween");
        }
    }
}