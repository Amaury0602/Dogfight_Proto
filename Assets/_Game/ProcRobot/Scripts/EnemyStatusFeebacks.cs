using System;
using UnityEngine;
using DG.Tweening;

public class EnemyStatusFeebacks : MonoBehaviour
{
    private EnemyBase _base;


    [SerializeField] private float _deathExplosionForce;
    [SerializeField] private float _deathExplosionRadius;

    [Serializable]
    public struct VisualPart
    {
        public MeshRenderer Renderer;
        public Color StartColor;
        public Tween Tween;
        public Rigidbody RigidBody;
    }

    [SerializeField] private AnimationCurve _blinkCurve;
    [SerializeField] private VisualPart[] _parts;

    private void Start()
    {
        _base = GetComponent<EnemyBase>();
        _base.OnDamageTaken += OnDamageTaken;
        _base.OnDeath += OnDeath;

        InitializeRenderers();
    }

    private void OnDeath()
    {
        if (_parts.Length == 0) return;

        for (int i = 0; i < _parts.Length; i++)
        {
            _parts[i].RigidBody.isKinematic = false;
            _parts[i].RigidBody.transform.SetParent(null);
            _parts[i].RigidBody.AddExplosionForce(_deathExplosionForce, transform.position, _deathExplosionRadius);
            _parts[i].RigidBody.AddTorque(UnityEngine.Random.insideUnitSphere * 150f, ForceMode.Impulse);
        }
    }

    private void InitializeRenderers()
    {
        if (_parts.Length == 0) return;

        for (int i = 0; i < _parts.Length; i++)
        {
            _parts[i].StartColor = _parts[i].Renderer.material.color;
        }
    }

    private void OnDamageTaken(int obj)
    {
        if (_parts.Length == 0) return;

        for (int i = 0; i < _parts.Length; i++)
        {
            if (_parts[i].Tween != null && _parts[i].Tween.IsPlaying()) _parts[i].Tween.Kill();
            _parts[i].Renderer.material.color = _parts[i].StartColor;
            _parts[i].Tween = _parts[i].Renderer.material.DOColor(Color.white, 0.1f).SetEase(_blinkCurve);
        }
    }
}

