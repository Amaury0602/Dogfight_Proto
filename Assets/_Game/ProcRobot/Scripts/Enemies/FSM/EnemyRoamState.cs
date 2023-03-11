using UnityEngine;

public class EnemyRoamState : EnemyBaseState
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    protected Transform _currentTarget;

    [SerializeField] protected Detector _playerDetector;

    public override void EnterState(EnemyBaseState previous)
    {
        _currentTarget = Random.Range(0f, 1f) > 0.5f ? _pointA : _pointB;
        _stateManager.Mover.SetDestination(_currentTarget.position);

        _playerDetector.gameObject.SetActive(true);
        _playerDetector.TriggerEnter += OnPlayerTriggerEnter;
    }

    protected virtual void OnPlayerTriggerEnter(Collider obj)
    {
        _playerDetector.TriggerEnter -= OnPlayerTriggerEnter;
        _stateManager.SetState(_stateManager.AttackState);
    }

    public override void ExitState()
    {
        _playerDetector.gameObject.SetActive(false);
    }

    public override void UpdateState()
    {
        _stateManager.Mover.RotateTowardsTarget(_stateManager.Player.Position);
        if ((_currentTarget.position - transform.position).sqrMagnitude < 2f)
        {
            if (_currentTarget == _pointA)
            {
                _currentTarget = _pointB;
            }
            else
            {
                _currentTarget = _pointA;
            }

            _stateManager.Mover.SetDestination(_currentTarget.position);
        }
    }

    public override void OnShot(int damage)
    {
        base.OnShot(damage);
        _stateManager.SetState(_stateManager.CoverState);
    }
}
