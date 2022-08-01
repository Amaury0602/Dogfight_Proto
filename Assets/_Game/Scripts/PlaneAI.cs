using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAI : MonoBehaviour
{
    private PlaneController controller;

    private void Awake()
    {
        controller = GetComponent<PlaneController>();
    }

    private void FollowTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
    }
}
