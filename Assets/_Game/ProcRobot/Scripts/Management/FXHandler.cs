using UnityEngine;

public class FXHandler : MonoBehaviour
{
    public static FXHandler Instance;

    [SerializeField] private ParticleSystem _baseBulletFX;
    [SerializeField] private ParticleSystem _bigBulletFX;
    [SerializeField] private ParticleSystem _explosionFX;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PoolParty.SetPoolSize(_baseBulletFX, 20);
        PoolParty.SetPoolSize(_bigBulletFX, 20);
        PoolParty.SetPoolSize(_explosionFX, 10);
    }

    public void BaseBulletHit(/*bool big, */Vector3 position, Quaternion rotation)
    {
        //ParticleSystem p = big ? _bigBulletFX : _baseBulletFX;
        ParticleSystem fx = PoolParty.Instantiate(_baseBulletFX, position, rotation);
        PoolParty.Destroy(fx, 2f);
    }
    
    public void RocketExplosion(Vector3 position)
    {
        ParticleSystem fx = PoolParty.Instantiate(_explosionFX, position, Quaternion.identity);
        PoolParty.Destroy(fx, 3f);
    }
}
