using System;
using UnityEngine;

public class KinematicPlane : MonoBehaviour
{

    private PlayerInput input;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotSpeed;

    [SerializeField] private Transform planeChild;

    [SerializeField] private Projectile laserPrefab;
    [SerializeField] private Transform[] guns;

    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform secondAimPoint;
    [SerializeField] private Transform middlePoint;
    [SerializeField] private float maxMagnitude;



    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }


    private float mouseDelta = 0f;
    private Vector2 prevPos = Vector2.zero;
    private void Update()
    {
        Vector3 rotMovement = new Vector3(transform.eulerAngles.x  + input.Direction.y, transform.eulerAngles.y + input.Direction.x, 0);
        //Vector3 rotMovement = new Vector3(transform.eulerAngles.x  + input.MouseDelta.y, transform.eulerAngles.y + input.MouseDelta.x, 0);

        Quaternion rot2 = Quaternion.identity;
        if (input.Direction.y == 0)
        {
            //float angle = Vector3.Angle(transform.forward, new Vector3(transform.forward.x, 0, transform.forward.z));
            //if (transform.forward.y < 0) angle = -angle;
        }

        RotateChild();
        

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

    private void RotateChild()
    {
        Vector3 childRot;
        float speed;
        if (input.Direction == Vector2.zero)
        {
            childRot = new Vector3(0, 0, Mathf.Sin(Time.time) * 5f);
            speed = 5f;
        }
        else
        {
            childRot = new Vector3(0, 0, planeChild.localEulerAngles.z - input.Direction.x);
            speed = rotSpeed * 2f;
        }

        childRot.z = ClampAngle(childRot.z, -60f, 60f);

        planeChild.localRotation = Quaternion.Lerp(planeChild.localRotation, Quaternion.Euler(childRot), speed* Time.deltaTime);
    }

    private void MoveSecondAimPoint()
    {
        Vector3 aimPos;
        float aimSpeed;
        if (input.Direction != Vector2.zero)
        {
            aimPos = secondAimPoint.localPosition + new Vector3(input.Direction.x, -input.Direction.y, 0);
            aimSpeed = 30f;
        }
        else
        {
            aimPos = Vector3.zero;
            aimSpeed = 5f;
        }

        aimPos = Vector3.ClampMagnitude(aimPos, maxMagnitude);

        aimPos.z = aimPoint.forward.z + 10f;

        secondAimPoint.localPosition = Vector3.Lerp(secondAimPoint.localPosition, aimPos, aimSpeed * Time.deltaTime);

        middlePoint.position = Vector3.Lerp(aimPoint.position, secondAimPoint.position, 0.6f);

    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
