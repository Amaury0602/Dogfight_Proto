using UnityEngine;
using DG.Tweening;

public class ShootDebug : MonoBehaviour, IShootable
{

    private Vector3 _startScale;
    private Vector3 _startPosition;
    [SerializeField] private float _recoil = 1f;

    private void Start()
    {
        _startScale = transform.localScale;
        _startPosition = transform.position;
    }

    public void OnShot(Vector3 dir, AmmunitionData data)
    {
        transform.DOKill();
        transform.localScale = _startScale;

        transform.position = _startPosition;


        transform.DOScale(_startScale * 1.25f, 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        transform.DOMove(_startPosition + dir * _recoil, 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }
}
