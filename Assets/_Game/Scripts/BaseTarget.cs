using UnityEngine;

public class BaseTarget : MonoBehaviour, IHittable
{

    [SerializeField] private ParticleSystem explosionFX;
    [SerializeField] private GameObject sphere;
    public void GetHit()
    {
        sphere.SetActive(false);
        explosionFX.Play();
        Destroy(gameObject, 5f);
    }
}
