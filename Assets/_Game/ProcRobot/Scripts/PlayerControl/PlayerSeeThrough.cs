using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeeThrough : MonoBehaviour
{
    [SerializeField] private GameObject _seeThroughObject;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private Transform _playerTarget;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void FixedUpdate()
    { 
        Ray r = new Ray(_playerTarget.position, _cam.transform.position - _playerTarget.position);
        _seeThroughObject.SetActive(Physics.Raycast(r, Mathf.Infinity, layerMask: _obstacleMask));
    }
}
