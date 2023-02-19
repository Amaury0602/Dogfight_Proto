using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _navigationRotSpeed;
    private NavMeshAgent _agent;

    [SerializeField] private Transform _lookTransform;
    public Vector3 NextPoint => _agent.steeringTarget;
    [SerializeField] private float _navHitRadius = 1f;

    private WaitForSeconds _pathCompletedCheckRate = new WaitForSeconds(0.5f);
    private Coroutine _checkDestinationReachedRoutine = default;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    public void SetDestination(Vector3 target, Action onEnd = null)
    {
        _agent.SetDestination(target);

        if (_checkDestinationReachedRoutine != null) StopCoroutine(_checkDestinationReachedRoutine);

        _checkDestinationReachedRoutine = StartCoroutine(CheckDestinationReached(target, onEnd));
    }

    private IEnumerator CheckDestinationReached(Vector3 dest, Action onReachedDest = null)
    {
        if (_agent.pathPending) yield return null;

        while(true)
        {
            if (_agent.remainingDistance > 0.2f)
            {
                yield return _pathCompletedCheckRate;
            }
            else
            {
                onReachedDest?.Invoke();
                yield break;
            }
        }
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

    public bool TryFindPositionCloseToPoint(Vector3 samplePosition, out Vector3 pos)
    {
        pos = Vector3.zero;
        if (NavMesh.SamplePosition(samplePosition, out NavMeshHit hit, _navHitRadius, NavMesh.AllAreas))
        {
            pos = hit.position;
            return true;
        }

        return false;
    }

    public void OnDeath()
    {
        _agent.enabled = false;
        if (_checkDestinationReachedRoutine != null) StopCoroutine(_checkDestinationReachedRoutine);
    }
}


