using UnityEngine;

public interface IShootable 
{
    public void OnShot(Vector3 dir, AmmunitionData data);
}