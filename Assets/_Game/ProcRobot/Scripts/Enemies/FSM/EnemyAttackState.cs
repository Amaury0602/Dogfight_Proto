using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    [SerializeField] private EnemyAim _aim;
    public override void EnterState()
    {
        _aim.SetWeapon();
    }

    public override void UpdateState()
    {
        if (_stateManager.Target != null)
        {
            _aim.AimAt(_stateManager.Target.position);
            _stateManager.Mover.RotateTowardsTarget(_stateManager.Target.position);
        }
    }


    public override void ExitState()
    {
        
    }
}
