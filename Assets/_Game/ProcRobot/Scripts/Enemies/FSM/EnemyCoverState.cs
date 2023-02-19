using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverState : EnemyBaseState
{

    [SerializeField] private int _numberOfRays;
    [SerializeField] private float _angle;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _hideMask;
    [SerializeField] private float _navHitRadius = 1f;
    private List<Vector3> _hitPoints = new List<Vector3>();

    public override void EnterState(EnemyBaseState previous)
    {
        if (previous is EnemyAttackState)
        {
            //_stateManager.Aim.AimAt(_stateManager.Target.position);
            //_stateManager.Mover.RotateTowardsTarget(_stateManager.Target.position);
        }
        GetToCover();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        
    }

    private void GetToCover()
    {

        Vector3 start = _stateManager.Target.position;

        float anglePerRay = _angle / (float)_numberOfRays;

        _hitPoints.Clear();

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _stateManager.Target.forward;
            Vector3 end = _stateManager.Target.position + _rayDistance * rotateVector;
            if (Physics.Linecast(start, end, out RaycastHit hit, layerMask: _hideMask))
            {
                //if collided
                Vector3 dir = hit.point - _stateManager.Target.position;
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

        if (_stateManager.Mover.TryFindPositionCloseToPoint(_hitPoints[closest], out Vector3 pos))
        {
            _stateManager.Mover.SetDestination(pos, OnCovered);
        }
    }

    private void OnCovered()
    {
        StartCoroutine(GoBackToAttackMode());
    }

    private IEnumerator GoBackToAttackMode() 
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        _stateManager.SetState(_stateManager.AttackState);
    }


    #region ================================== VISUAL DEBUG =============================================
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_stateManager == null) return;

        Vector3 start = _stateManager.Target.position;

        float anglePerRay = _angle / (float)_numberOfRays;

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _stateManager.Target.forward;
            Vector3 end = _stateManager.Target.position + _rayDistance * rotateVector;
            Debug.DrawLine(start, end, Color.yellow);

            if (Physics.Linecast(start, end, out RaycastHit hit, layerMask: _hideMask))
            {
                //if collided
                Vector3 dir = hit.point - _stateManager.Target.position;
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
