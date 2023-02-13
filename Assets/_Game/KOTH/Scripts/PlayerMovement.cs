using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxRadiusMove;
    [SerializeField] private float _rotSpeed;

    private Vector3 _startPosition;

    private Camera _cam;

    [SerializeField] private bool _updateCameraPlane = false;

    private void Start()
    {
        _cam = Camera.main;
        _startPosition = transform.position;

        CameraRotator.Instance.OnBigAngleChange += OnBigAngleChange;
    }

    private void OnBigAngleChange()
    {
        _updateCameraPlane = false;
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (input == Vector3.zero) 
        {
            _updateCameraPlane = true;
            return; 
        }

        
        if (_updateCameraPlane)
        {
            input = CameraRelativeMovement(new Vector2(input.x, input.z));
        }


        transform.position = Vector3.Lerp(transform.position, transform.position + input, Time.deltaTime * _moveSpeed);

        //rotation
        Quaternion lookDir = Quaternion.LookRotation(input);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * _rotSpeed);

        //ClampPosition();
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos = Vector3.ClampMagnitude(_startPosition - pos, _maxRadiusMove) ;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    } 

    private Vector3 CameraRelativeMovement(Vector2 input)
    {
        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 rightRelative = input.x * camRight;
        Vector3 forwardRelative = input.y * camForward;

        return (forwardRelative + rightRelative).normalized;
    }
}
