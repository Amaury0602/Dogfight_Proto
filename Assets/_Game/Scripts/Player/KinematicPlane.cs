using System;
using UnityEngine;

public class KinematicPlane : MonoBehaviour
{

    private PlayerInput input;

    private Camera cam;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotSpeed;

    [SerializeField] private Transform planeChild;

    [SerializeField] private Projectile laserPrefab;
    [SerializeField] private Transform[] guns;

    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform secondAimPoint;
    [SerializeField] private float maxMagnitude;



    private void Awake()
    {
        cam = Camera.main;
        input = GetComponent<PlayerInput>();
    }


    private float mouseDelta = 0f;
    private Vector2 prevPos = Vector2.zero;
    private void Update()
    {
        //Vector3 rotMovement = new Vector3(transform.eulerAngles.x  + input.Direction.y, transform.eulerAngles.y + input.Direction.x, 0);
        Vector3 rotMovement = new Vector3(transform.eulerAngles.x  + input.MouseDelta.y, transform.eulerAngles.y + input.MouseDelta.x, 0);

        Quaternion rot2 = Quaternion.identity;
        if (input.Direction.y == 0)
        {
            //float angle = Vector3.Angle(transform.forward, new Vector3(transform.forward.x, 0, transform.forward.z));
            //if (transform.forward.y < 0) angle = -angle;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, rot2 * Quaternion.Euler(rotMovement), rotSpeed * Time.deltaTime);


        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < guns.Length; i++)
            {
                Vector3 direction = secondAimPoint.position - guns[i].position;
                Projectile newProjectile = Instantiate(laserPrefab, guns[i].position, Quaternion.LookRotation(direction));
                newProjectile.SetDirection(direction.normalized);
            }
        }


        MoveSecondAimPoint();
    }

    private void MoveSecondAimPoint()
    {
        Vector3 aimPos;
        if (input.Direction != Vector2.zero)
        {
            aimPos = secondAimPoint.localPosition + new Vector3(input.Direction.x, -input.Direction.y, 0);
        }
        else
        {
            aimPos = Vector3.zero;
        }

        aimPos = Vector3.ClampMagnitude(aimPos, maxMagnitude);

        aimPos.z = aimPoint.forward.z + 10f;

        secondAimPoint.localPosition = Vector3.Lerp(secondAimPoint.localPosition, aimPos, 15f * Time.deltaTime);
    }
}
