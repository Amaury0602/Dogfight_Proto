using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector3 currentDirection;
    public void SetDirection(Vector3 direction)
    {
        currentDirection = direction;
        StartCoroutine(DestroyAfterDelay(2f));
    }



    private void Update()
    {
        if (currentDirection == Vector3.zero) return;

        transform.position += currentDirection * 2500f * Time.deltaTime;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
