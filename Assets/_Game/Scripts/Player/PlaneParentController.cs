using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneParentController : MonoBehaviour
{
    private PlayerInput input;

    [SerializeField] private Transform planeChild;

    [SerializeField] private Projectile laserPrefab;
    [SerializeField] private Transform[] guns;

    [SerializeField] private float rotSpeed;
    [SerializeField] private float parentRotSpeed;
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Vector3 rotMovement = new Vector3(planeChild.localEulerAngles.x + input.Direction.y, planeChild.localEulerAngles.y + input.Direction.x, 0);
        Vector3 parentRot = new Vector3(transform.eulerAngles.x + input.Direction.y, transform.eulerAngles.y + input.Direction.x, 0);

        Quaternion rot2 = Quaternion.identity;
        if (input.Direction.y == 0)
        {
            //float angle = Vector3.Angle(transform.forward, new Vector3(transform.forward.x, 0, transform.forward.z));
            //if (transform.forward.y < 0) angle = -angle;
        }

        planeChild.localRotation = Quaternion.Lerp(planeChild.localRotation, rot2 * Quaternion.Euler(rotMovement), rotSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, planeChild.localRotation, parentRotSpeed * Time.deltaTime);


        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        //{
        //    for (int i = 0; i < guns.Length; i++)
        //    {
        //        Vector3 direction = secondAimPoint.position - guns[i].position;
        //        Projectile newProjectile = Instantiate(laserPrefab, guns[i].position, Quaternion.LookRotation(direction));
        //        newProjectile.SetDirection(direction.normalized);
        //    }
        //}


        //MoveSecondAimPoint();
    }
}
