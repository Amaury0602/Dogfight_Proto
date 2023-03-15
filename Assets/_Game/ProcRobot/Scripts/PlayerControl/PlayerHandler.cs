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

    public Booster Booster { get; private set; }
    private float _boostSpeedBonus = 0;

    private Rigidbody _rb;

    public Vector3 Direction { get; private set; }
    public Vector3 Position => transform.position;

    [SerializeField] private bool _rotateWithMovement = false;

    public Action OnMovementStopped = default;
    private bool _isMoving = false;

    private bool _inputDown = false;


    private Vector3 _inputDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _aim = GetComponent<PlayerAim>();
        _player = GetComponent<Player>();
        Booster = GetComponent<Booster>();
        _cam = Camera.main;
        _canMove = true;

        _player.OnDeath += OnDeath;
        Booster.OnBoost += OnBoost;
        Booster.EnergyFullyConsumed += OnBoostStopped;

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

        if (_rb.velocity.sqrMagnitude <= _rbMoveSpeed * 1.75f)
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

            if (!_inputDown) _inputDown = true;

            Vector3 forward = _cam.transform.forward * _inputDirection.z;
            Vector3 right = _cam.transform.right * _inputDirection.x;
            Vector3 mov = (forward + right).normalized;
            mov.y = 0;

            //transform.position = Vector3.Lerp(transform.position, transform.position + mov, Time.deltaTime * _moveSpeed);
            Direction = mov;
        }
        else
        {
            if (_inputDown) 
            {
                _inputDown = false;
                _rb.velocity = Vector3.zero;
            } 
            Direction = Vector3.zero;
        }
        
        Vector3 aimingDir = PlayerUICursor.Instance.CursorToWorldPosition - transform.position;
        aimingDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(aimingDir);
        
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotSpeed);
    }


    private void OnBoost(float impulse, float speedBonus)
    {
        if (Direction == Vector3.zero)
        {
            OnMovementStopped?.Invoke();
            return;
        }
        _rb.AddForce(Direction * impulse, ForceMode.Impulse);
        _boostSpeedBonus = speedBonus;
    }

    private void OnBoostStopped()
    {
        _boostSpeedBonus = 0f;
    }


    private void OnDeath()
    {
        _canMove = false;
        _player.OnDeath -= OnDeath;
    }
}
