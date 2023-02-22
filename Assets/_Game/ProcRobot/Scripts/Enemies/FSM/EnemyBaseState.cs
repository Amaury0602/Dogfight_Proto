using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour, IShootable
{
    protected EnemyStateManager _stateManager;

    private void Awake()
    {
        _stateManager = GetComponent<EnemyStateManager>();
        if (_stateManager == null)
        {
#if UNITY_EDITOR
            print($"WARNING NO STATE MANAGER ON {gameObject}");
#endif
        }
    }

    public abstract void EnterState(EnemyBaseState previousState = null);
    public abstract void ExitState();
    public abstract void UpdateState();

    public virtual void OnShot(Vector3 dir, AmmunitionData data)
    {
#if UNITY_EDITOR
        print($"{gameObject.name}, and this : {this}");
#endif


        _stateManager.Enemy.OnShot(dir, data);
    }
}
