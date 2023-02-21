using UnityEngine;
using System;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotSpeed;

    private Camera _cam;

    private bool _isAiming = false;
    public Action OnAimDown = default;
    public Action OnAimUp = default;

    private PlayerAim _aim;

    private void Start()
    {
        _aim = GetComponent<PlayerAim>();
        _cam = Camera.main;
    }
    void Update()
    {
        DetectMouseInput();
        Move();
    }

    private void Move()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector3 forward = _cam.transform.forward * inputDirection.z;
        Vector3 right = _cam.transform.right * inputDirection.x;
        Vector3 mov = (forward + right).normalized;
        mov.y = 0;

        transform.position = Vector3.Lerp(transform.position, transform.position + mov, Time.deltaTime * _moveSpeed);

        Vector3 aimingDir = _aim.Direction;
        aimingDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(aimingDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotSpeed);
        //if (_isAiming)
        //{
            
        //}
        //else
        //{
        //    if (inputDirection == Vector3.zero) return;
        //    Quaternion rot = Quaternion.LookRotation(mov);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotSpeed);
        //}
    }

    private void DetectMouseInput()
    {
        if (Input.GetMouseButton(1))
        {
            if (!_isAiming)
            {
                _isAiming = true;
                OnAimDown?.Invoke();
            }
        } 
        else
        {
            if (_isAiming)
            {
                _isAiming = false;
                OnAimUp?.Invoke();
            }
        }
    }
}
