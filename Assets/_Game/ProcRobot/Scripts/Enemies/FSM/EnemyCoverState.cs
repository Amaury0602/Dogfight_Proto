using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverState : EnemyBaseState
{

    [SerializeField] private int _numberOfRays;
    [SerializeField] private float _angle;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _hideMask;
    private List<Vector3> _hitPoints = new List<Vector3>();

    private Coroutine _afterCoverRoutine = null;

    private bool _isCovered = false;

    public override void EnterState(EnemyBaseState previous)
    {
        _stateManager.Aim.OnGainSight += GetOutOfCover;
        _isCovered = false;
        GetToCover();
    }

    public override void ExitState()
    {
        if (_afterCoverRoutine != null) StopCoroutine(_afterCoverRoutine);
        _stateManager.Aim.OnGainSight -= GetOutOfCover;
        _isCovered = false;
    }

    public override void UpdateState()
    {
        _stateManager.Aim.AimAt(_stateManager.Player.Position, _stateManager.Player.Direction);
        _stateManager.Mover.RotateTowardsTarget(_stateManager.Player.Position);
    }

    private void GetToCover()
    {
        print($"{gameObject.name} START GET TO COVER");

        Vector3 start = _stateManager.Player.Position;

        float anglePerRay = _angle / (float)_numberOfRays;

        _hitPoints.Clear();

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _stateManager.Player.transform.forward;
            Vector3 end = _stateManager.Player.Position + _rayDistance * rotateVector;
            if (Physics.Linecast(start, end, out RaycastHit hit, layerMask: _hideMask))
            {
                //if collided
                Vector3 dir = hit.point - _stateManager.Player.Position;
                Vector3 newStart = hit.point + dir.normalized * 15f;

                if (Physics.Linecast(newStart, hit.point, out RaycastHit hitt, layerMask: _hideMask))
                {
                    Vector3 dirToColliderCenter = (hitt.collider.bounds.center - hitt.point).normalized;
                    _hitPoints.Add(hitt.point + dir.normalized * 2f + dirToColliderCenter * 3f);
                }
            }
        }

        if (_hitPoints.Count <= 0) 
        {
            _stateManager.SetState(_stateManager.AttackState);
            return;
        }

        //get closest cover position;
        int closest = 0;
        for (int j = 0; j < _hitPoints.Count; j++)
        {
            if ((_hitPoints[j] - transform.position).sqrMagnitude < (_hitPoints[closest] - transform.position).sqrMagnitude)
            {
                closest = j;
            }
        }

        //can I get to this position ? 
        if (_stateManager.Mover.TryFindPositionCloseToPoint(_hitPoints[closest], out Vector3 pos))
        {
            print($"{gameObject.name} FOUND COVER POSITION");
            _stateManager.Mover.SetDestination(pos, OnCovered);
        }
    }

    private void OnCovered()
    {
        _isCovered = true;
        
        _afterCoverRoutine = StartCoroutine(GoBackToAttackMode());
    }


    private void GetOutOfCover()
    {
        if (!_isCovered) return;
        _stateManager.SetState(_stateManager.AttackState);
    }

    public override void OnShot(int damage)
    {
        base.OnShot(damage);
        GetOutOfCover();
    }

    private IEnumerator GoBackToAttackMode() 
    {
        print($"{gameObject.name} WAIT TO GO BACK TO ATTACK MODE");

        yield return new WaitForSeconds(Random.Range(3f, 6f));
        _stateManager.SetState(_stateManager.AttackState);
    }


    #region ================================== VISUAL DEBUG =============================================
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_stateManager == null) return;

        Vector3 start = _stateManager.Player.Position;

        float anglePerRay = _angle / (float)_numberOfRays;

        for (int i = 0; i < _numberOfRays; i++)
        {
            Vector3 rotateVector = Quaternion.AngleAxis(anglePerRay * i - _angle / 2f, Vector3.up) * _stateManager.Player.transform.forward;
            Vector3 end = _stateManager.Player.Position + _rayDistance * rotateVector;
            Debug.DrawLine(start, end, Color.yellow);

            if (Physics.Linecast(start, end, out RaycastHit hit, layerMask: _hideMask))
            {
                //if collided
                Vector3 dir = hit.point - _stateManager.Player.Position;
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
