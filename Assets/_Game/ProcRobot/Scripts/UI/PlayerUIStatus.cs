using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PlayerUIStatus : MonoBehaviour
{

    private float _startHealth;
    private float _currentHealth;

    [SerializeField] private Image _healthFillBar;
    [SerializeField] private Image _secondHealthBar;
    [SerializeField] private Image _boostFillBar;
    [SerializeField] private Transform _healthBarParent;

    private Vector3 _healthBarStartPosition;

    private Coroutine _computeDamageRoutine;

    void Start()
    {
        _healthBarStartPosition = _healthBarParent.position;

        _startHealth = Player.Instance.Health;
        _currentHealth = _startHealth;
        Player.Instance.OnTakeDamage += OnPlayerTookDamage;
        Player.Instance.OnDeath += OnPlayerDeath;
        Player.Instance.Handler.Booster.BoostConsumed += OnBoostUpdate;
    }

    private void OnBoostUpdate(float value)
    {
        _boostFillBar.fillAmount = value;
    }

    private void OnPlayerTookDamage(float obj)
    {

        ShakeBar();


        _currentHealth -= obj;

        _currentHealth = Mathf.Max(0, _currentHealth);

        if (_computeDamageRoutine != null) StopCoroutine(_computeDamageRoutine);

        _computeDamageRoutine = StartCoroutine(ComputeTakenDamage());


        _healthFillBar.fillAmount = _currentHealth / _startHealth;
    }


    private void ShakeBar()
    {
        _healthBarParent.DOKill();
        _healthBarParent.position = _healthBarStartPosition;
        _healthBarParent.DOShakePosition(0.15f, 2);
    }

    private IEnumerator ComputeTakenDamage()
    {
        yield return new WaitForSeconds(2f);
        DOTween.To(() => _secondHealthBar.fillAmount, x => _secondHealthBar.fillAmount = x, _healthFillBar.fillAmount, 0.25f);
    }
    private void OnPlayerDeath()
    {
        if (_computeDamageRoutine != null) StopCoroutine(_computeDamageRoutine);
        DOTween.To(() => _secondHealthBar.fillAmount, x => _secondHealthBar.fillAmount = x, 0f, 0.25f);
        Player.Instance.OnTakeDamage -= OnPlayerTookDamage;
        Player.Instance.OnDeath -= OnPlayerDeath;
    }

    private void OnDisable()
    {
        Player.Instance.OnTakeDamage -= OnPlayerTookDamage;
        Player.Instance.OnDeath -= OnPlayerDeath;
    }
}
