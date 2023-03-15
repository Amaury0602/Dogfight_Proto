using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    private bool _targetInSight = false;

    protected Coroutine _attackRoutine = default;

    [SerializeField] private float _refreshDamageCD = 2f;
    [SerializeField] private float _damageThreshold = 30f;
    private float _computedDamage;
    private Coroutine _damageComputeRoutine = null;

    [SerializeField, Range(-0.1f, 1.1f)] private float aggressiveness;

    private Vector3 lastKnownPosition = default;
    public override void EnterState(EnemyBaseState previous)
    {
        _stateManager.Aim.OnGainSight += HandleGainSight;
        _stateManager.Aim.OnLostSight += HandleLoseSight;

        _attackRoutine = StartCoroutine(AttackTarget());
    }
    protected virtual void HandleGainSight()
    {
        _targetInSight = true;
        _attackRoutine = StartCoroutine(AttackTarget());
    }

    protected virtual void HandleLoseSight(Vector3 lastKnownPos)
    {
        lastKnownPosition = lastKnownPos;
        if (_attackRoutine != null) StopCoroutine(_attackRoutine);

        _targetInSight = false;

        if (Random.Range(0f, 1f) < aggressiveness) // harass player 
        {
            _stateManager.Mover.SetDestination(lastKnownPosition);
        }
        else
        {
            Vector3 closePoint = Vector3.Lerp(transform.position, lastKnownPosition, 0.35f);
            if (CanFindPosAroundPoint(closePoint, 5f, out Vector3 pos)) 
            {
                _stateManager.Mover.SetDestination(pos);
            }
        }

    }

    protected bool CanFindPosAroundPoint(Vector3 point, float radius, out Vector3 pos)
    {
        pos = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            Vector2 rand = Random.insideUnitCircle.normalized * radius;
            Vector3 aroundPoint = point + new Vector3(rand.x, 0, rand.y);

            if (_stateManager.Mover.TryFindPositionCloseToPoint(aroundPoint, out pos))
            {
                return true;
            }
        }

        return false;
    }

    public override void UpdateState()
    {
        if (_stateManager.Player != null)
        {
            _stateManager.Aim.AimAt(_stateManager.Player.Position, _stateManager.Player.Direction);
            _stateManager.Mover.RotateTowardsTarget(_stateManager.Player.Position);
        }
    }

    protected virtual IEnumerator AttackTarget()
    {
        while(_stateManager.Enemy.Alive)
        {
            float distToPlayer = 50f;
            if (Random.Range(0f, 1f) < aggressiveness) distToPlayer = 25f;

            if (CanFindPosAroundPoint(_stateManager.Player.Position, distToPlayer, out Vector3 pos))
            {
                _stateManager.Mover.SetDestination(pos);
            }
            yield return new WaitForSeconds(Random.Range(3f, 6f));  
        }
    }

    public override void ExitState()
    {
        _targetInSight = false;
        _stateManager.Aim.OnGainSight -= HandleGainSight;
        _stateManager.Aim.OnLostSight -= HandleLoseSight;
        if (_attackRoutine != null) StopCoroutine(_attackRoutine);
        if (_damageComputeRoutine != null) StopCoroutine(_damageComputeRoutine);
    }

    public override void OnShot(int damage)
    {
        if (_damageComputeRoutine != null)
        {
            StopCoroutine(_damageComputeRoutine);
        }

        StartCoroutine(ComputeDamageTaken(damage));

        if (_computedDamage >= _damageThreshold)
        {
            OnTooMuchDamageReceived();
        }
    }

    private void OnTooMuchDamageReceived()
    {
        if (_attackRoutine != null) StopCoroutine(_attackRoutine);
        _stateManager.SetState(_stateManager.CoverState);
    }

    private IEnumerator ComputeDamageTaken(int dmg)
    {
        _computedDamage += dmg;
        yield return new WaitForSeconds(_refreshDamageCD);
        _computedDamage = 0;
    }
}
