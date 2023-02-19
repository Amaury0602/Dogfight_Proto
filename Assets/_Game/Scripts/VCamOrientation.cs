using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamOrientation : MonoBehaviour
{
    [SerializeField] private float lookAtSpeed;
    private Quaternion startRotation;

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        //Vector3 dir = (Player.I.transform.position - transform.position).normalized;
        //Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), lookAtSpeed * Time.deltaTime);
        //transform.rotation = rot;
    }
}
