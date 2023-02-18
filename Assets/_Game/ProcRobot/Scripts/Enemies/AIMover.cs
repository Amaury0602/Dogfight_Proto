using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _navigationRotSpeed;
    [SerializeField] private LayerMask _mouseDetectLayer;
    private NavMeshAgent _agent;

    [SerializeField] private Transform _lookTransform;

    private Coroutine _moveRoutine = null;

    public Vector3 NextPoint => _agent.steeringTarget;

    [SerializeField] private int _numberOfRays;
    [SerializeField] private float _angle;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _hideMask;
    [SerializeField] private float _navHitRadius = 1f;

    private List<Vector3> _hitPoints = new List<Vector3>();

    [SerializeField] private bool _hideFromPlayerSight = true;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    public void SetDestination(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    public void ToggleNavigation(bool stop)
    {
        _agent.isStopped = stop;
    }

    public void RotateTowardsTarget(Vector3 targetPos) 
    {
        if (_agent.angularSpeed != 0) _agent.angularSpeed = 0;

        targetPos.y = transform.position.y;
        Quaternion lookDir = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, _rotationSpeed * Time.deltaTime);
    }
    
    public void RotateAlongNavigation() 
    {
        _agent.angularSpeed = _navigationRotSpeed;
    }

    
    private void GetToCover()
    {

        Vector3 start = _lookTransform.position;

        float anglePerRay = _angle / (float)_numberOfRays;

        _hitPoints.Clear();

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _lookTransform.forward;
            Vector3 end = _lookTransform.position + _rayDistance * rotateVector;
            if (Physics.Linecast(start, end, out RaycastHit hit, layerMask: _hideMask))
            {
                //if collided
                Vector3 dir = hit.point - _lookTransform.position;
                Vector3 newStart = hit.point + dir.normalized * 15f;

                if (Physics.Linecast(newStart, hit.point, out RaycastHit hitt, layerMask: _hideMask))
                {
                    Vector3 dirToColliderCenter = (hitt.collider.bounds.center - hitt.point).normalized;                    
                    _hitPoints.Add(hitt.point + dir.normalized * 2f + dirToColliderCenter * 3f);
                }
            }

        }

        if (_hitPoints.Count <= 0) return;

        int closest = 0;
        for (int j = 0; j < _hitPoints.Count; j++)
        {
            if ((_hitPoints[j] - transform.position).sqrMagnitude < (_hitPoints[closest] - transform.position).sqrMagnitude)
            {
                closest = j;
            }
        }

        if (NavMesh.SamplePosition(_hitPoints[closest], out NavMeshHit navHit, _navHitRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(navHit.position);
        }

    }

    #region ================================== VISUAL DEBUG =============================================
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 start = _lookTransform.position;

        float anglePerRay = _angle / (float)_numberOfRays;

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _lookTransform.forward;
            Vector3 end = _lookTransform.position + _rayDistance * rotateVector;
            Debug.DrawLine(start, end, Color.yellow);

            if (Physics.Linecast(start, end, out RaycastHit hit, layerMask: _hideMask))
            {
                //if collided
                Vector3 dir = hit.point - _lookTransform.position;
                Vector3 newStart = hit.point + dir.normalized * 15f;

                Debug.DrawLine(newStart, hit.point, Color.red);

                if (Physics.Linecast(newStart, hit.point, out RaycastHit hitt, layerMask: _hideMask))
                {
                    Vector3 dirToColliderCenter = (hitt.collider.bounds.center - hitt.point).normalized;
                    Gizmos.DrawCube(hitt.point + dir.normalized * 2f + dirToColliderCenter * 3f, Vector3.one * 2f);
                }
            }
            else
            {
                Gizmos.DrawSphere(end, 0.5f);
            }
        }
    }
#endif
    #endregion
}


