using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

public class PlayerUICursor : MonoBehaviour
{
    public static PlayerUICursor Instance;
    [SerializeField] private PlayerAim _playerAim;
    private Camera _cam;
    [SerializeField] private LayerMask _specificAimLayer;

    [SerializeField] private Image _cursorImage;
    [SerializeField] private Sprite _baseCursor;
    [SerializeField] private Sprite _secondaryCursor;

    public Action<Vector3> OnProjectedPoint = default;

    [SerializeField] private Transform _playerHeightMarker;
    [SerializeField] private Transform _worldPlayerOrientationTarget;
    [SerializeField] private float _orientationTargetMaxDistance;

    private Plane _playerPlane;

    private Tween scaleTween = default;

    //Detect objects behind cursor
    private RectTransform _rect;
    private Vector3 _rectSize = default;
    private Vector3 _rectWorldSize = default;

    [SerializeField] private Transform _aimMidPoint;

    private Vector3 _aimPoint = default;

    public Vector3 CursorPosition { get; set; }

    [SerializeField, Range(1,10)] private int _targetLocksCount;
    [SerializeField] private Transform _targetLocksParent;
    [SerializeField] private TargetLockVisual _targetLockPrefab;
    private TargetLockVisual[] _targetLocks;

    [SerializeField] private TMP_Text _debugText;

    private void Awake()
    {
        Instance = this;
        _rect = GetComponent<RectTransform>();
        _playerPlane = new Plane(Vector3.up, _playerHeightMarker.position);
    }

    private void OnEnable()
    {
        PlayerInputs.Instance.OnMouseMove += Move;
        //PlayerInputs.Instance.OnRightMouseDown += SwitchToSecondaryCursor;
        //PlayerInputs.Instance.OnRightMouseUp += SwitchToBaseCursor;
    }

    private void SwitchToBaseCursor()
    {
        _cursorImage.sprite = _baseCursor;
    }

    private void SwitchToSecondaryCursor()
    {
        _cursorImage.sprite = _secondaryCursor;
    }

    private void Start()
    {
        _cam = Camera.main;

        _rectSize = new Vector3(_rect.rect.width * _rect.lossyScale.x, _rect.rect.height * _rect.lossyScale.y, 0);
        _rectWorldSize = new Vector3(_rectSize.x / Screen.width * _cam.orthographicSize * _cam.aspect, _rectSize.y / Screen.height * _cam.orthographicSize, 0f);

        _playerAim.CurrentWeapon.OnShotFired += BumpCursor;

        _targetLocks = new TargetLockVisual[_targetLocksCount];

        for (int i = 0; i < _targetLocksCount; i++)
        {
            TargetLockVisual target = Instantiate(_targetLockPrefab, transform.position, Quaternion.identity, _targetLocksParent);
            _targetLocks[i] = target;
            target.Initialize();
        }
    }

    private void BumpCursor() // scale tweeen
    {
        if (scaleTween != null && scaleTween.IsPlaying()) scaleTween.Kill();
        transform.DOKill();
        transform.localScale = Vector3.one;
        scaleTween = transform.DOScale(1.15f, 0.05f).SetLoops(2, LoopType.Yoyo);
    } 

    private void Move(Vector2 movement)
    {
        transform.position += (Vector3)movement;
        Ray ray = _cam.ScreenPointToRay(transform.position);


        RaycastHit[] hits = Physics.BoxCastAll(ray.origin, _rectWorldSize, ray.direction, _cam.transform.rotation, Mathf.Infinity, _specificAimLayer);
        //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask: _specificAimLayer))
        //{
        //    OnProjectedPoint?.Invoke(hit.point);
        //}

        if (hits.Length > 0)
        {
            Vector2 mousePos = Input.mousePosition;
            RaycastHit closest = hits[0];
            for (int i = 0; i < hits.Length; i++)
            {
                if (i >= _targetLocksCount) break;

                _targetLocks[i].Focus(_cam.WorldToScreenPoint(hits[i].collider.transform.position));

                //Vector2 worldToScreenPoint = _cam.WorldToScreenPoint(hits[i].point);
                //if ((mousePos - worldToScreenPoint).sqrMagnitude < (mousePos - (Vector2)_cam.WorldToScreenPoint(closest.point)).sqrMagnitude)
                //{
                //    closest = hits[i];
                //}
            }
            _aimPoint = closest.collider.transform.position;
        }



        //unfocus target locks
        int count = Mathf.Max(0, _targetLocksCount - hits.Length);

        print($"targets to unfocus {_targetLocksCount - count}");

        if (hits.Length < _targetLocksCount)
        {
            for (int i = _targetLocksCount - 1; i > count; i--)
            {
                _targetLocks[i].UnFocus();
            }
        }


        _playerPlane.SetNormalAndPosition(Vector3.up, _playerHeightMarker.position);

        if (_playerPlane.Raycast(ray, out float rayDistance/*, out RaycastHit hit*/))
        {
            CursorPosition = ray.GetPoint(rayDistance);
            _aimPoint = ray.GetPoint(rayDistance);
        }


        _aimMidPoint.position = Vector3.Lerp(_playerAim.transform.position, _aimPoint, 0.75f);

        OnProjectedPoint?.Invoke(_aimPoint);


        ClampPositionOnScreen();


        _debugText.text = $"Enemies in cursor : {hits.Length} Targets to unfocus {_targetLocksCount - count}";
    }


    private void ClampPositionOnScreen()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        transform.position = pos;
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.OnMouseMove -= Move;
        PlayerInputs.Instance.OnRightMouseDown -= SwitchToSecondaryCursor;
        PlayerInputs.Instance.OnRightMouseUp -= SwitchToBaseCursor;
    }
}
