using UnityEngine;
using DG.Tweening;

public class LimbMovementManager : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotSpeed;


    [SerializeField] private Transform _leftFootTarget;
    [SerializeField] private Transform _leftFoot;
    [SerializeField] private bool _grounded = true;
    [SerializeField] private bool _movementNeeded = false;
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField] private float _distToMove;
    [SerializeField] private float _stepLength;


    private Vector3 _bodyMovement = default;
    private Vector3 _lastBodyPosition = default;


    private void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit h, Mathf.Infinity, _floorLayer))
        {
            _leftFootTarget.position = h.point;
        }
    }



    private void Update()
    {
        Move();

        _bodyMovement = _lastBodyPosition - transform.position;
        _lastBodyPosition = transform.position;

        if (_bodyMovement == Vector3.zero)
        {
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit h, Mathf.Infinity, _floorLayer))
            {
                if ((h.point - _leftFootTarget.position).sqrMagnitude > _distToMove)
                {
                    _movementNeeded = true;
                    Vector3 dir = h.point - _leftFootTarget.position;
                    _leftFootTarget.position += dir.normalized * _stepLength;
                    // move foot
                }
                else
                    _movementNeeded = false;
            }
        }


        

        if (_movementNeeded && _grounded)
        {
            _grounded = false;
            _leftFoot.DOJump(_leftFootTarget.position, 2, 1, 0.25f).OnComplete(() => { _grounded = true; });
        }
    }

    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (movement != Vector3.zero)
        {
            transform.position += movement * _moveSpeed * Time.deltaTime;


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * _rotSpeed);
        }

    }
}
