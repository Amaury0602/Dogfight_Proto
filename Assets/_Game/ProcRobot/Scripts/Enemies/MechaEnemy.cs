using UnityEngine;
public class MechaEnemy : EnemyBase
{
    private Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
