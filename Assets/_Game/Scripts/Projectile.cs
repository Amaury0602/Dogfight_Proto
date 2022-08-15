using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 currentDirection;
    [SerializeField] private LayerMask notPlayerLayer;
    [SerializeField] private ParticleSystem smokeFX;
    private Vector3 prevPos;

    private Coroutine destroyRoutine;

    private bool isHit = false;



    public void SetDirection(Vector3 direction)
    {
        currentDirection = direction;
        prevPos = transform.position;
        destroyRoutine = StartCoroutine(DestroyAfterDelay(2f));
    }



    private void Update()
    {
        if (currentDirection == Vector3.zero || isHit) return;

        transform.position += currentDirection * 2500f * Time.deltaTime;

        if (Physics.Linecast(prevPos, transform.position, notPlayerLayer))
        {
            Hit();
        }

        prevPos = transform.position;
    }

    private void Hit()
    {
        isHit = true;
        StopCoroutine(destroyRoutine);
        smokeFX.Play();
        Destroy(gameObject, 5f);
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
