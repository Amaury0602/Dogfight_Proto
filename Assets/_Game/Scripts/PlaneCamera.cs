using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCamera : MonoBehaviour
{

    [SerializeField] private Vector3 offSet;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotateSpeed;
    // Update is called once per frame
    void LateUpdate()
    {
        if (!target) return;
        Vector3 nextPosition = target.position + target.TransformDirection(offSet);
        transform.position = Vector3.Lerp(transform.position, nextPosition, followSpeed * Time.deltaTime);

        Vector3 targetRot = new Vector3(transform.eulerAngles.x, target.eulerAngles.y, transform.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), rotateSpeed * Time.deltaTime);
    }
}
