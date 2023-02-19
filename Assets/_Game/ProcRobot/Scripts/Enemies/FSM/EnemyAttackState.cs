using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    private bool _targetInSight = false;

    private Coroutine _attackRoutine = default;

    public override void EnterState(EnemyBaseState previous)
    {
        _stateManager.Aim.SetWeapon();
        _stateManager.Aim.OnGainSight += HandleGainSight;
        _stateManager.Aim.OnLostSight += HandleLoseSight;
        _stateManager.Aim.SetWeapon();
    }
    private void HandleGainSight()
    {
        _targetInSight = true;

        _attackRoutine = StartCoroutine(ChaseTarget());
    }

    private void HandleLoseSight(Vector3 lastPos)
    {
        if (_attackRoutine != null) StopCoroutine(_attackRoutine);

        _targetInSight = false;
        _stateManager.Mover.SetDestination(lastPos);
    }
    public override void UpdateState()
    {
        if (_stateManager.Target != null)
        {
            _stateManager.Aim.AimAt(_stateManager.Target.position);
            _stateManager.Mover.RotateTowardsTarget(_stateManager.Target.position);
        }
    }

    private IEnumerator ChaseTarget()
    {
        while(_targetInSight)
        {
            
            yield return null;
        }
    }

    private void FindSpotToAttackTarget()
    {

    }

    public override void ExitState()
    {
        _stateManager.Aim.OnGainSight -= HandleGainSight;
        _stateManager.Aim.OnLostSight -= HandleLoseSight;
    }
}
