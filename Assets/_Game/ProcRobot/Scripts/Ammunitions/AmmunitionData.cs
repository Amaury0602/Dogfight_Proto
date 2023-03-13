using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "SO/AmmunitionData")]
public class AmmunitionData : ScriptableObject
{
    public AmmunitionType Type;
    public int Damage;
    public bool Homing;
    public AmmunitionEffect Effect;
}


public enum AmmunitionType
{
    Hitscan = 0,
    Projectile = 1
}

public enum AmmunitionEffect
{
    None = 0,
    Explosive = 1
}
