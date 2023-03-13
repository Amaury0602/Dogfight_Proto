public class CannonBullet : ProjectileBase
{
    protected override void ReachedTarget()
    {
        FXHandler.Instance.BaseBulletHit(transform.position, UnityEngine.Quaternion.identity);
        base.ReachedTarget();
    }
}
