using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSView : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sens;

    private Vector2 lastMouse = default;

    private void Start()
    {
        lastMouse = default;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 forwardMovement = transform.forward * movement.z;
        Vector3 rightMovement = transform.right * movement.x;

        transform.position = Vector3.Lerp(transform.position, transform.position + forwardMovement + rightMovement, Time.deltaTime * _moveSpeed);

        Vector2 delta = (Vector2)Input.mousePosition - lastMouse;

        transform.Rotate(new Vector3(/*-delta.y*/0, delta.x , 0) * _sens * Time.deltaTime);

        lastMouse = Input.mousePosition;
    }
}
