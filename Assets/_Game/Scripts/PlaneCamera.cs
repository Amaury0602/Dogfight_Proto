using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCamera : MonoBehaviour
{

    [SerializeField] private Vector3 offSet;
    [SerializeField] private Transform target;
    [SerializeField] private Transform rotTarget;
    [SerializeField] private float followSpeed;
    [SerializeField] private float xOffsetSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float smoothTime;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Vector2 minMaxPosition;
    // Update is called once per frame
    void Update()
    {
        //if (!target) return;
        //Vector3 nextPosition = target.position + target.TransformDirection(offSet);
        //transform.position = Vector3.Lerp(transform.position, nextPosition, followSpeed * Time.deltaTime);

        //Vector3 targetRot = new Vector3(transform.eulerAngles.x, target.eulerAngles.y, transform.eulerAngles.z);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), rotateSpeed * Time.deltaTime);

        Vector3 targetPos = target.position + target.TransformDirection(offSet);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        //transform.rotation = target.rotation;

    }

    private void LateUpdate()
    {
        Vector3 planeForward = rotTarget.position + rotTarget.forward * 20f;

        Vector3 lookDir = planeForward - transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotateSpeed * Time.deltaTime);
        
        
        //offset on X axis when player turns right or left
        LateralOffsetFollow();
    }

    private void LateralOffsetFollow()
    {
        Vector3 input = PlayerInput.i.Direction;
        float movement = 0f;
        float offSpeed = xOffsetSpeed;
        if (input.x == 0)
        {
            movement = -offSet.x;
            offSpeed = xOffsetSpeed;
        }
        else
        {
            movement = input.x;
            offSpeed = xOffsetSpeed * 3f;
        }

        offSet += Vector3.right * movement * offSpeed * Time.deltaTime;

        offSet.x = Mathf.Clamp(offSet.x, -15, 15);
    }
}
