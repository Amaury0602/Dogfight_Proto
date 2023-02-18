using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour, IShootable
{
    protected EnemyStateManager _stateManager;

    private void Awake()
    {
        _stateManager = GetComponent<EnemyStateManager>();
        if (_stateManager == null)
        {
            print($"WARNING NO STATE MANAGER ON {gameObject}");
        }
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();

    public virtual void OnShot(Vector3 dir, AmmunitionData data)
    {
    }
}
