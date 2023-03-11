using UnityEngine;
using System;

public class Squad : MonoBehaviour
{
    [SerializeField] private EnemyBase[] _members;
    [field: SerializeField] public EnemyBase Leader { get; private set; }

    public Action AttackAlert = default;


    private void Start()
    {
        for (int i = 0; i < _members.Length; i++)
        {
            _members[i].Initialize(this);
            _members[i].OnDamageTaken += MemberGotHit;
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
