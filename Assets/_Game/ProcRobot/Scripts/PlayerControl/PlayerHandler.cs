using UnityEngine;
using System;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rbMoveSpeed;
    [SerializeField] private float _dt;
    [SerializeField] private float _rotSpeed;

    private Camera _cam;

    private bool _canMove = false;
    private bool _isAiming = false;
    public Action OnAimDown = default;
    public Action OnAimUp = default;

    private PlayerAim _aim;
    private Player _player;

    private Rigidbody _rb;

    public Vector3 Direction { get; private set; }
    public Vector3 Position => transform.position;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _aim = GetComponent<PlayerAim>();
        _player = GetComponent<Player>();
        _cam = Camera.main;

        _player.OnDeath += OnDeath;

        _canMove = true;
    }
    void Update()
    {
        //DetectMouseInput();
        Move();
    }

    private void Move()
    {

        if (!_canMove) return;



        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (inputDirection != Vector3.zero)
        {
            Vector3 forward = _cam.transform.forward * inputDirection.z;
            Vector3 right = _cam.transform.right * inputDirection.x;
            Vector3 mov = (forward + right).normalized;
            mov.y = 0;

            transform.position = Vector3.Lerp(transform.position, transform.position + mov, Time.deltaTime * _moveSpeed);


            Direction = mov;
            //_rb.velocity = Vector3.Lerp(_rb.velocity, mov * _rbMoveSpeed, Time.deltaTime * _dt);
        }
        else
        {
            Direction = Vector3.zero;
            //_rb.velocity = Vector3.zero;
        }


        Vector3 aimingDir = _aim.Direction;
        aimingDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(aimingDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotSpeed);
    }
    private void OnDeath()
    {
        _canMove = false;
        _player.OnDeath -= OnDeath;
    }
}
