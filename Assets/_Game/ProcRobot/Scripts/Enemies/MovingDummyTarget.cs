using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDummyTarget : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _amplitude;


    private Vector3 _startPosition;


    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(_startPosition, _startPosition + _amplitude * Vector3.right, (Mathf.Sin(Time.time * _speed) + 1) /2f);
    }
}
