using UnityEngine;

public interface IShootableEntity
{
    public void OnShot(Vector3 dir, AmmunitionData data);
    public void OnShot(Vector3 dir, int damage, AmmunitionData data);
}