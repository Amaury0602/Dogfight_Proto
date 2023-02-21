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
        _rb.isKinematic = false;

        _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        _rb.AddTorque(Random.insideUnitSphere * 150f , ForceMode.Impulse);
    }
}