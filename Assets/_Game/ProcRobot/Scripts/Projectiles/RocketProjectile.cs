using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : ProjectileBase
{
    private WaitForSeconds _waitDelay = new WaitForSeconds(0.75f);

    protected override IEnumerator FlyTowardsTarget(Vector3 target)
    {
        yield return _waitDelay;
        yield return base.FlyTowardsTarget(target);
    }

    protected override void ReachedTarget()
    {
        FXHandler.Instance.RocketExplosion(transform.position);
        base.ReachedTarget();
    }
}
