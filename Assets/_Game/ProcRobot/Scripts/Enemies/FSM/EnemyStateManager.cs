using UnityEngine;

[RequireComponent(typeof(AIMover))]
public class EnemyStateManager : MonoBehaviour
{
    [field: SerializeField] public EnemyBaseState CurrentState { get; private set; }
    [field: SerializeField] public EnemyBaseState PreviousState { get; private set; }

    //HANDLING
    [field: SerializeField] public AIMover Mover { get; private set; }
    [field: SerializeField] public EnemyAim Aim { get; private set; }
    [field: SerializeField] public EnemyBase Enemy  { get; private set; }


    //STATES
    public EnemyRoamState RoamState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyCoverState CoverState { get; private set; }

    [field: SerializeField] public PlayerHandler Player { get; private set; }

    private void Awake()
    {
        Mover = GetComponent<AIMover>();
        Aim = GetComponent<EnemyAim>();
        Enemy = GetComponent<EnemyBase>();

        RoamState = GetComponent<EnemyRoamState>();
        AttackState = GetComponent<EnemyAttackState>();
        CoverState = GetComponent<EnemyCoverState>();


        Enemy.OnDeath += OnDeath;
    }

    private void Start()
    {
        PreviousState = RoamState;
        SetState(RoamState);
    }

    public void SetState(EnemyBaseState state)
    {
        if (CurrentState != null) 
        {
            CurrentState.ExitState();
            PreviousState = CurrentState;
        }
        Mover.OnStateChanged();


        if (state == null)
        {
            SetState(PreviousState);
            return;
        }


        CurrentState = state;
        CurrentState.EnterState(PreviousState);
    }

    private void OnDeath()
    {
        Enemy.OnDeath -= OnDeath;
        CurrentState.ExitState();
        Mover.OnDeath();
        CurrentState = null;
    }


    private void Update()
    {
        if (CurrentState != null) CurrentState.UpdateState();
    }

    public void OnShotTaken(int damage)
    {
        if (CurrentState != null) CurrentState.OnShot(damage);
    }

}
