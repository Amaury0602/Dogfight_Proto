using UnityEngine;

[RequireComponent(typeof(AIMover))]
public class EnemyStateManager : MonoBehaviour
{
    [field: SerializeField] public EnemyBaseState CurrentState { get; private set; }
    [field: SerializeField] public EnemyBaseState PreviousState { get; private set; }

    [field: SerializeField] public AIMover Mover { get; private set; }

    public EnemyRoamState RoamState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    [field: SerializeField] public Transform Target { get; private set; }

    private void Awake()
    {
        Mover = GetComponent<AIMover>();

        RoamState = GetComponent<EnemyRoamState>();
        AttackState = GetComponent<EnemyAttackState>();
    }

    private void Start()
    {
        PreviousState = null;
        SetState(RoamState);
    }

    public void SetState(EnemyBaseState state)
    {
        if (state == null)
        {
            print($"this character has no {state}");
            return;
        }
        if (CurrentState != null) 
        {
            CurrentState.ExitState();
            PreviousState = CurrentState;
        }

        CurrentState = state;
        CurrentState.EnterState();
    }

    private void Update()
    {
        if (CurrentState != null) CurrentState.UpdateState();
    }

}
