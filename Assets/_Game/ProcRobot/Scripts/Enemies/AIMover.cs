using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMover : MonoBehaviour
{
    
    private Camera _cam;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private LayerMask _mouseDetectLayer;
    private NavMeshAgent _agent;

    private NavMeshPath _path;
    [SerializeField] private NavMeshQueryFilter areaFilter;

    [SerializeField] private Transform _lookTransform;

    private Coroutine _moveRoutine = null;

    public Vector3 NextPoint => _agent.steeringTarget;

    [SerializeField] private int _numberOfRays;
    [SerializeField] private float _angle;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _hideMask;
    [SerializeField] private float _navHitRadius = 1f;
    private RaycastHit[] _rayHits;

    private List<Vector3> _hitPoints = new List<Vector3>();

    private void Start()
    {
        _rayHits = new RaycastHit[_numberOfRays];
        _agent = GetComponent<NavMeshAgent>();
        _cam = Camera.main;
        _path = new NavMeshPath();
    }

    private IEnumerator GoToTarget(Vector3 target)
    {
        while((target - transform.position).sqrMagnitude > 1f)
        {
            Vector3 dir = target - transform.position;
            dir.y = 0;
            transform.position = Vector3.Lerp(transform.position, transform.position + dir, Time.deltaTime * _moveSpeed);
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        Vector3 lookDir = _lookTransform.position - transform.position;
        lookDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookDir);
        transform.rotation = rot;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray r = _cam.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(r, out RaycastHit h, Mathf.Infinity, layerMask: _mouseDetectLayer))
        //    {
        //        Vector3 rand = Random.insideUnitSphere * 10f;
        //        Vector3 p = h.point + new Vector3(rand.x, 0, rand.z);
        //        _agent.SetDestination(p);
        //        NavMesh.CalculatePath(transform.position, h.point, NavMesh.AllAreas, _path);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.H))
        {
            GetToCover();
        }
    }
#endif

    
    private void GetToCover()
    {

        Vector3 start = _lookTransform.position;

        float anglePerRay = _angle / (float)_numberOfRays;

        _hitPoints.Clear();

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _lookTransform.forward;
            Vector3 end = _lookTransform.position + _rayDistance * rotateVector;
            if (Physics.Linecast(start, end, out RaycastHit hit))
            {
                //if collided
                Vector3 dir = hit.point - _lookTransform.position;
                Vector3 newStart = hit.point + dir.normalized * 15f;

                if (Physics.Linecast(newStart, hit.point, out RaycastHit hitt))
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

            if (Physics.Linecast(start, end, out RaycastHit hit))
            {
                //if collided
                Vector3 dir = hit.point - _lookTransform.position;
                Vector3 newStart = hit.point + dir.normalized * 15f;


                Debug.DrawLine(newStart, hit.point, Color.red);

                //Gizmos.DrawWireCube(newStart, Vector3.one * 0.25f);
                //Gizmos.DrawWireCube(hit.point, Vector3.one * 1f);

                if (Physics.Linecast(newStart, hit.point, out RaycastHit hitt))
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
