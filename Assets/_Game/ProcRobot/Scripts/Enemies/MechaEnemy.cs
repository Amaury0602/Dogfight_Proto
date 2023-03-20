using UnityEngine;
public class MechaEnemy : EnemyBase
{
    private Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected override void Die()
    {
        base.Die();
        VirtualCameraHandler.Instance.Shake(2f, 2f, 0.5f);
    }
}
