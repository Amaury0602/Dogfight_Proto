public class HomingMissile : HomingProjectileBase
{
    protected override void ReachedTarget()
    {
        FXHandler.Instance.RocketExplosion(transform.position);
        base.ReachedTarget();
    }
}
