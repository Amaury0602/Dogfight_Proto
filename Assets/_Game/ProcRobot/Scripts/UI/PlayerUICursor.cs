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
    [SerializeField] private float _midPointFollowSpeed;

    private Vector3 _aimPoint = default;

    public Vector3 CursorToWorldPosition { get; set; }

    [SerializeField, Range(1,10)] private int _targetLocksCount;
    [SerializeField] private Transform _targetLocksParent;
    [SerializeField] private TargetLockVisual _targetLockPrefab;
    

    [SerializeField] private TMP_Text _debugText;
    public TargetLockVisual[] TargetLocks { get; private set; }
    public bool AtLeastOneTargetLocked => IsAtLeastOneTargetLocked();
    public int MaxLockableTargets => _targetLocksCount;

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

        TargetLocks = new TargetLockVisual[_targetLocksCount];

        for (int i = 0; i < _targetLocksCount; i++)
        {
            TargetLockVisual target = Instantiate(_targetLockPrefab, transform.position, Quaternion.identity, _targetLocksParent);
            TargetLocks[i] = target;
            target.gameObject.name = $"TargetLock_{i}";
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


    private bool IsAtLeastOneTargetLocked()
    {
        for (int i = 0; i < TargetLocks.Length; i++)
        {
            if (TargetLocks[i].Locked) return true;
        }

        return false;
    }

    private void Move(Vector2 movement)
    {
        transform.position += (Vector3)movement;
        Ray ray = _cam.ScreenPointToRay(transform.position);

        _playerPlane.SetNormalAndPosition(Vector3.up, _playerHeightMarker.position);

        if (_playerPlane.Raycast(ray, out float rayDistance/*, out RaycastHit hit*/))
        {
            CursorToWorldPosition = ray.GetPoint(rayDistance);
            _aimPoint = ray.GetPoint(rayDistance);
        }


        RaycastHit[] hits = Physics.BoxCastAll(ray.origin, _rectWorldSize, ray.direction, _cam.transform.rotation, Mathf.Infinity, _specificAimLayer);
        //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask: _specificAimLayer))
        //{
        //    OnProjectedPoint?.Invoke(hit.point);
        //}

        if (hits.Length > 0)
        {
            Vector2 closest = _cam.WorldToScreenPoint(hits[0].collider.transform.position);
            Transform worldTarget = hits[0].collider.transform;

            for (int i = 0; i < hits.Length; i++)
            {
                Vector2 screenPoint = _cam.WorldToScreenPoint(hits[i].collider.transform.position);
                if (((Vector2)_rect.position - screenPoint).sqrMagnitude < ((Vector2)_rect.position - closest).sqrMagnitude)
                {
                    closest = screenPoint;
                    worldTarget = hits[i].collider.transform;
                }
            }

            TargetLocks[0].Focus(closest, worldTarget);

            //for (int i = 0; i < hits.Length; i++) // THIS IS IF YOU WANT TO TARGET MULTIPLE ENEMIES
            //{
            //    if (i >= _targetLocksCount) break;

            //    _targetLocks[i].Focus(_cam.WorldToScreenPoint(hits[i].collider.transform.position));
            //}
            _aimPoint = worldTarget.position;
        }

        //unfocus target locks
        int count = Mathf.Max(0, _targetLocksCount - hits.Length);

        for (int i = _targetLocksCount - 1; i >= _targetLocksCount - count; i--)
        {
            TargetLocks[i].UnFocus();
        }

        



        Vector3 midPoint = Vector3.Lerp(_playerAim.transform.position, _aimPoint, 0.75f);
        _aimMidPoint.position = Vector3.Lerp(_aimMidPoint.position, midPoint, Time.deltaTime * _midPointFollowSpeed);

        OnProjectedPoint?.Invoke(_aimPoint);


        ClampPositionOnScreen();


        _debugText.text = $"Enemies in cursor : {hits.Length}";
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
