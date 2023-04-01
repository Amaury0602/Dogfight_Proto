using System.Collections;
using UnityEngine;

public class FXHandler : MonoBehaviour
{
    public static FXHandler Instance;

    [SerializeField] private ParticleSystem _baseBulletFX;
    [SerializeField] private ParticleSystem _bigBulletFX;
    [SerializeField] private ParticleSystem _explosionFX;
    [SerializeField] private ParticleSystem _debrisFX;

    [Header("Weapon FX")]
    [SerializeField] private TrailRenderer _bulletTrail;
    [SerializeField] private float _bulletTrailTime = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PoolParty.SetPoolSize(_baseBulletFX, 20);
        PoolParty.SetPoolSize(_bigBulletFX, 20);
        PoolParty.SetPoolSize(_explosionFX, 10);
        PoolParty.SetPoolSize(_debrisFX, 10);


        PoolParty.SetPoolSize(_bulletTrail, 20);
    }

    public void BaseBulletHit(/*bool big, */Vector3 position, Quaternion rotation)
    {
        //ParticleSystem p = big ? _bigBulletFX : _baseBulletFX;
        ParticleSystem fx = PoolParty.Instantiate(_baseBulletFX, position, rotation);
        PoolParty.Destroy(fx, 2f);
    }
    public void PlayDebris(Vector3 position, Quaternion rotation, int damage = 3)
    {
        //ParticleSystem p = big ? _bigBulletFX : _baseBulletFX;
        ParticleSystem fx = PoolParty.Instantiate(_debrisFX, position, rotation);
        damage = Mathf.Min(damage, 15);
        fx.Emit(damage);
        PoolParty.Destroy(fx, 10f);
    }
    
    public void RocketExplosion(Vector3 position)
    {
        ParticleSystem fx = PoolParty.Instantiate(_explosionFX, position, Quaternion.identity);
        PoolParty.Destroy(fx, 3f);
    }


    public void PlayBulletTrail(Vector3 start, Vector3 endPoint)
    {
        StartCoroutine(SpawnTrail(start, endPoint));
    }

    private IEnumerator SpawnTrail(Vector3 start, Vector3 endPoint)
    {
        TrailRenderer t = PoolParty.Instantiate(_bulletTrail, start, Quaternion.identity);


        _bulletTrailTime = 0;
        while(_bulletTrailTime < 1f)
        {
            t.transform.position = Vector3.Lerp(start, endPoint, _bulletTrailTime);
            _bulletTrailTime += Time.deltaTime / t.time;
            yield return null;
        }

        t.transform.position = endPoint;

        PoolParty.Destroy(t, t.time);
    }
}
