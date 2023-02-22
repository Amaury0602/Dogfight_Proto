using UnityEngine;
using System;
using DG.Tweening;

public class PlayerUICursor : MonoBehaviour
{
    public static PlayerUICursor Instance;


    [SerializeField] private PlayerAim _playerAim;

    private Camera _cam;

    [SerializeField] private float height = 1f;
    [SerializeField] private LayerMask _specificAimLayer;

    private Vector2 screenSize;


    public Vector3 ProjectedWorldPosition { get; private set; }

    public Action<Vector3> OnProjectedPoint = default;

    [SerializeField] private Transform _playerHeightMarker;

    private Plane _playerPlane;

    private Tween scaleTween = default;

    private void Awake()
    {
        Instance = this;
        screenSize = new Vector2(Screen.width, Screen.height);

        _playerPlane = new Plane(Vector3.up, _playerHeightMarker.position);
    }

    private void OnEnable()
    {
        MouseInput.Instance.OnMouseMove += Move;
    }

    private void Start()
    {
        _cam = Camera.main;
        _playerAim.CurrentWeapon.OnShotFired += OnShoot;
    }

    private void OnShoot()
    {
        if (scaleTween != null && scaleTween.IsPlaying()) scaleTween.Kill();
        transform.DOKill();
        transform.localScale = Vector3.one;
        scaleTween = transform.DOScale(1.5f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }

    private void Move(Vector2 movement)
    {
        transform.position += (Vector3)movement;
        Ray ray = _cam.ScreenPointToRay(transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask: _specificAimLayer))
        {
            OnProjectedPoint?.Invoke(hit.point);
        }
        else
        {
            _playerPlane.SetNormalAndPosition(Vector3.up, _playerHeightMarker.position);

            if (_playerPlane.Raycast(ray, out float rayDistance/*, out RaycastHit hit*/))
            {
                OnProjectedPoint?.Invoke(ray.GetPoint(rayDistance));
                //Vector3 worldPoint = ray.GetPoint(rayDistance);
            }
        }


        

        //clamp cursor on screen
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        transform.position = pos;
    }

    private void OnDisable()
    {
        MouseInput.Instance.OnMouseMove -= Move;
    }
}
