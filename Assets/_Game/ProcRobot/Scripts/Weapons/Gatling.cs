using UnityEngine;

public class Gatling : HitscanWeapon
{
    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);
        FXHandler.Instance.BaseBulletHit(hit.point, Quaternion.LookRotation(hit.normal));
    }
}
