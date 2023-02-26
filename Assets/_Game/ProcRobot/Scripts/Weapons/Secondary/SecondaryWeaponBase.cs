using UnityEngine;

public abstract class SecondaryWeaponBase : WeaponBase
{
    public bool IsActive { get; private set; } = false;
    public override void OnEquipped(ShooterBase shooter)
    {
        base.OnEquipped(shooter);
        IsActive = true;
        OnStart();
    }

    public override void OnUnequipped(ShooterBase shooter)
    {
        base.OnUnequipped(shooter);
        IsActive = false;
        OnExit();
    }

    public abstract void OnStart();
    public abstract void OnExit();
}
