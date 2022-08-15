using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCamera : MonoBehaviour
{

    [SerializeField] private Vector3 offSet;
    [SerializeField] private Transform target;
    [SerializeField] private Transform rotTarget;
    [SerializeField] private float followSpeed;
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
        //Vector3 pos = transform.position;
        ////pos.x = Mathf.Clamp(pos.x, -minMaxPosition.x, minMaxPosition.x);
        //pos.y = Mathf.Clamp(pos.y, 0, minMaxPosition.y);
        //transform.position = pos;

        Vector3 planeForward = rotTarget.position + rotTarget.forward * 20f;

        Vector3 lookDir = planeForward - transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotateSpeed * Time.deltaTime);
    }
}
