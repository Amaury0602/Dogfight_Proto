public class HumanRoamState : EnemyRoamState
{
    private EnemyBase _leader;

    private void Start()
    {
        _leader = _stateManager.Enemy.Squad.Leader;
    }

    public override void EnterState(EnemyBaseState previous)
    {
        _playerDetector.gameObject.SetActive(true);
        _playerDetector.TriggerEnter += OnPlayerTriggerEnter;
    }

    public override void UpdateState()
    {
        if (_leader == null) return;
        _stateManager.Mover.SetDestination(_leader.transform.position);
    }

    public override void OnShot(int damage)
    {
        _stateManager.SetState(_stateManager.AttackState);
    }
}
