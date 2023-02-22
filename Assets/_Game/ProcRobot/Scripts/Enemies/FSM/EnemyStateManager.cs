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
        PreviousState = null;
        SetState(RoamState);
    }

    public void SetState(EnemyBaseState state)
    {
        if (state == null)
        {
#if UNITY_EDITOR
            print($"this character has no {state}");
#endif
            return;
        }
        if (CurrentState != null) 
        {
            CurrentState.ExitState();
            PreviousState = CurrentState;
        }

        Mover.OnStateChanged();

        CurrentState = state;
        CurrentState.EnterState(PreviousState);
    }

    private void OnDeath()
    {
        Enemy.OnDeath -= OnDeath;
        CurrentState.ExitState();
        CurrentState = null;
        Mover.OnDeath();
    }


    private void Update()
    {
        if (CurrentState != null) CurrentState.UpdateState();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(CoverState);
        }
#endif
    }

    public void OnShotTaken(int damage)
    {
        if (CurrentState != null) CurrentState.OnShot(damage);
    }

}
