using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootMover : MonoBehaviour
{
    public Vector3 CurrentPosition => transform.position;

    public bool Grounded { get; set; } = false;

    private bool _canMove = true;

    [SerializeField] private float _moveSpeed;
    private Coroutine _moveRoutine = null;


    private Vector3 _targetPos = default;

    private Vector3 _lastPosition;

    private void Start()
    {
        _lastPosition = transform.position;
        Grounded = true;
    }

    //public void UpdatePosition(Vector3 desiredPos)
    //{
    //    //transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * _moveSpeed);

    //    if (_moveRoutine == null)
    //    {
    //        Grounded = false;
    //        _moveRoutine = StartCoroutine(MoveFoot(desiredPos));
    //    }
    //}

    //private IEnumerator MoveFoot(Vector3 target)
    //{
    //    float elapsed = 0f;
    //    float duration = 0.15f;

    //    while (elapsed < duration)
    //    {
    //        elapsed += Time.deltaTime;
    //        float percent = Mathf.Clamp01(elapsed / duration);

    //        transform.position = Vector3.Lerp(_lastPosition, target, percent);

    //        yield return null;
    //    }

    //    _lastPosition = transform.position;
    //    Grounded = true;
    //    _moveRoutine = null;
    //}
}
