using System.Collections;
using UnityEngine;

public class HumanoidAttackState : EnemyAttackState
{
    private EnemyBase _leader;

    [SerializeField] private float _distanceToPlayerChase = 20f;

    private void Start()
    {
        _leader = _stateManager.Enemy.Squad.Leader;
    }


    protected override IEnumerator AttackTarget()
    {
        if (_leader == null)
        {
            yield return base.AttackTarget();
        }
        else
        {
            while (_leader != null && _leader.Alive)
            {
                _stateManager.Mover.SetDestination(_leader.transform.position);
                yield return null;
            }
        }
    }

    //private void ChasePlayer()
    //{
    //    if (CanFindPosAroundPoint(_stateManager.Player.Position, _distanceToPlayerChase, out Vector3 pos))
    //    {
    //        _stateManager.Mover.SetDestination(pos, OnReachedChasePosition);
    //    }
    //}

    //private void OnReachedChasePosition()
    //{

    //}

    protected override void HandleGainSight() {}

    protected override void HandleLoseSight(Vector3 lastKnownPos) {}
    public override void OnShot(int damage) {}
}
