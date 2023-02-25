using System;
using UnityEngine;
using DG.Tweening;

public class EnemyStatusFeebacks : MonoBehaviour
{
    private EnemyBase _base;

    [Serializable]
    public struct VisualPart
    {
        public MeshRenderer Renderer;
        public Color StartColor;
        public Tween Tween;
    }

    [SerializeField] private AnimationCurve _blinkCurve;
    [SerializeField] private VisualPart[] _parts;

    private void Start()
    {
        _base = GetComponent<EnemyBase>();
        _base.OnDamageTaken += OnDamageTaken;

        InitializeRenderers();
    }

    private void InitializeRenderers()
    {
        for (int i = 0; i < _parts.Length; i++)
        {
            _parts[i].StartColor = _parts[i].Renderer.material.color;
        }
    }

    private void OnDamageTaken(int obj)
    {
        for (int i = 0; i < _parts.Length; i++)
        {
            if (_parts[i].Tween != null && _parts[i].Tween.IsPlaying()) _parts[i].Tween.Kill();
            _parts[i].Renderer.material.color = _parts[i].StartColor;
            _parts[i].Tween = _parts[i].Renderer.material.DOColor(Color.white, 0.1f).SetEase(_blinkCurve);
        }
    }
}

