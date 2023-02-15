using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{

    [SerializeField] protected float _moveSpeed;

    public virtual void OnWeaponFire(Vector3 target)
    {
        StartCoroutine(FlyTowardsTarget(target));
    }

    protected virtual IEnumerator FlyTowardsTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        while (dir.sqrMagnitude > 1f)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;

            dir = target - transform.position;

            yield return null;
        }

        ReachedTarget();
    }

    protected virtual void ReachedTarget()
    {
        Destroy(gameObject);
    }
}
