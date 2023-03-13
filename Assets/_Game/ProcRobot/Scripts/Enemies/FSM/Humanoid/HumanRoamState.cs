using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRoamState : EnemyRoamState
{
    private EnemyBase _leader;

    private void Start()
    {
        HumanoidEnemy h = _stateManager.Enemy as HumanoidEnemy;
        _leader = h.SquadLeader;
    }

    public override void EnterState(EnemyBaseState previous)
    {
        _playerDetector.gameObject.SetActive(true);
        _playerDetector.TriggerEnter += OnPlayerTriggerEnter;
    }

    public override void UpdateState()
    {
        _stateManager.Mover.SetDestination(_leader.transform.position);
    }
}
