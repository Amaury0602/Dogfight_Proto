using UnityEngine;
using System;

public class Squad : MonoBehaviour
{
    [SerializeField] private EnemyBase[] _members;
    [field: SerializeField] public EnemyBase Leader { get; private set; }

    public Action AttackAlert = default;
    public Action OnLeaderDied = default;


    private void Start()
    {
        for (int i = 0; i < _members.Length; i++)
        {
            _members[i].OnDamageTaken += MemberGotHit;
            _members[i].OnDeath += OnMemberDied;
        }
    }

    private void OnMemberDied(EnemyBase enemy)
    {
        if (enemy == Leader)
        {
            print("LEADER JUST DIED");
        }
    }

    private void MemberGotHit(int obj)
    {
        for (int i = 0; i < _members.Length; i++)
        {
            _members[i].OnDamageTaken -= MemberGotHit;
        }

        AttackAlert?.Invoke();
    }
}
