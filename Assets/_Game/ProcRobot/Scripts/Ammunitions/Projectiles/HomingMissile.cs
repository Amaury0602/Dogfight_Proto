using System.Collections;
using UnityEngine;

public class HomingMissile : HomingProjectileBase
{
    protected override void ReachedTarget()
    {
        FXHandler.Instance.RocketExplosion(transform.position);
        base.ReachedTarget();
    }

    protected override IEnumerator HomeTowardsTarget(Transform target)
    {
        //transform.rotation = Quaternion.Euler(0, Random.Range(-15f, 15f), 0);
        return base.HomeTowardsTarget(target);
    }
}
