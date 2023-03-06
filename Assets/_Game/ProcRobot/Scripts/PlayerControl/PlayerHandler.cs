using UnityEngine;
using System;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rbMoveSpeed;
    [SerializeField] private float _dt;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _maxVelocity;

    private Camera _cam;

    private bool _canMove = false;
    private bool _isAiming = false;
    public Action OnAimDown = default;
    public Action OnAimUp = default;

    private PlayerAim _aim;
    private Player _player;

    private Booster _booster;
    private float _boostSpeedBonus = 0;

    private Rigidbody _rb;

    public Vector3 Direction { get; private set; }
    public Vector3 Position => transform.position;

    [SerializeField] private bool _rotateWithMovement = false;

    public Action OnMovementStopped = default;
    private bool _isMoving = false;


    private Vector3 _inputDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _aim = GetComponent<PlayerAim>();
        _player = GetComponent<Player>();
        _booster = GetComponent<Booster>();
        _cam = Camera.main;
        _canMove = true;

        _player.OnDeath += OnDeath;
        _booster.OnBoost += OnBoost;

    }
    void Update()
    {
        Move();
    }


    private void FixedUpdate()
    {
        _rb.velocity = Vector3.Lerp(_rb.velocity, Direction * _rbMoveSpeed * (1 + _boostSpeedBonus), Time.deltaTime * _dt);
        //_rb.velocity = Direction * _rbMoveSpeed * (1 + _boostSpeedBonus);
    }

    private void Move()
    {

        if (_rb.velocity.sqrMagnitude <= 0.15f)
        {
            if (_isMoving)
            {
                _isMoving = false;
                OnMovementStopped?.Invoke();
                _boostSpeedBonus = 0f;
            }
        }

        if (!_canMove) return;

        _inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (_inputDirection != Vector3.zero)
        {
            if (!_isMoving) _isMoving = true;

            Vector3 forward = _cam.transform.forward * _inputDirection.z;
            Vector3 right = _cam.transform.right * _inputDirection.x;
            Vector3 mov = (forward + right).normalized;
            mov.y = 0;

            //transform.position = Vector3.Lerp(transform.position, transform.position + mov, Time.deltaTime * _moveSpeed);
            Direction = mov;
        }
        else
        {
            Direction = Vector3.zero;
        }
        
        Vector3 aimingDir = _aim.Direction;
        aimingDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(aimingDir);
        
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotSpeed);
    }


    private void OnBoost(float speedBonus)
    {
        _boostSpeedBonus = speedBonus;
    }


    private void OnDeath()
    {
        _canMove = false;
        _player.OnDeath -= OnDeath;
    }
}
