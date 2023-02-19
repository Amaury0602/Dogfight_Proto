using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MechaInfoCanvas : MonoBehaviour
{
    private Camera _cam;

    [SerializeField] private EnemyBase _enemy;
    [SerializeField] private Image _fillBar;

    private int _startHealth;
    private int _currentHealth;

    private void Awake()
    {
        _cam = Camera.main;
        _startHealth = _enemy.Health;
        _currentHealth = _enemy.Health;

        _enemy.OnDamageTaken += OnDamageTaken;
        _enemy.OnDeath += OnDeath;
    }
    private void OnDamageTaken(int damage)
    {

        transform.DOKill();

        transform.DOShakePosition(0.15f, Vector3.right * 2f);

        _currentHealth -= damage;

        _fillBar.fillAmount = (float)_currentHealth / (float)_startHealth;
    }
    private void OnDeath()
    {
        transform.DOKill();
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        BillBoardEffect();
    }

    private void BillBoardEffect()
    {
        //Quaternion rot = Quaternion.LookRotation(transform.position - _cam.transform.forward);
        transform.rotation = _cam.transform.rotation;
    }

    private void OnDisable()
    {
        _enemy.OnDamageTaken -= OnDamageTaken;
        _enemy.OnDeath -= OnDeath;
    }
}
