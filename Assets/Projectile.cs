using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector3 currentDirection;
    public void SetDirection(Vector3 direction)
    {
        currentDirection = direction;
    }


    private void Update()
    {
        if (currentDirection == Vector3.zero) return;

        transform.position += currentDirection * 500f * Time.deltaTime;
    }
}
