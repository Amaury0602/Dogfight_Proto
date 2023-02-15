using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        IShootable shootable = hit.collider.GetComponent<IShootable>();
        if (shootable != null)
        {
            Vector3 dir = hit.point - WeaponTransform.position;
            shootable.Shoot(dir.normalized);
        }
    }
}
