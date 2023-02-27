using UnityEngine;

public interface IShootable 
{
    public void OnShot(Vector3 dir, AmmunitionData data);
    public void OnShot(Vector3 dir, int damage, AmmunitionData data);
}