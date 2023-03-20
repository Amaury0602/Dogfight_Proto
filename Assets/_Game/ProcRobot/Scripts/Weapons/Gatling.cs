using UnityEngine;

public class Gatling : HitscanWeapon
{
    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("OBSTACLE"))
        {
            FXHandler.Instance.PlayDebris(hit.point + hit.normal, Quaternion.LookRotation(hit.normal));
        }

        FXHandler.Instance.BaseBulletHit(hit.point, Quaternion.LookRotation(hit.normal));
    }
}
