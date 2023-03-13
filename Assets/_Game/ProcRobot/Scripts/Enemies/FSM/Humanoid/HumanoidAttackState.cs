using System.Collections;

public class HumanoidAttackState : EnemyAttackState
{
    private EnemyBase _leader;

    private void Start()
    {
        HumanoidEnemy enemy = _stateManager.Enemy as HumanoidEnemy;
        _leader = enemy.SquadLeader;
    }
    protected override IEnumerator AttackTarget()
    {
        while (_leader != null && _leader.Alive)
        {
            _stateManager.Mover.SetDestination(_leader.transform.position);
            yield return null;
        }
    }
    protected override void HandleGainSight() {}
    public override void OnShot(int damage) {}
}
