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


    private Rigidbody rb;
    [SerializeField] private PlaneSettingsSO settings;



    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        
    }

    private void Update()
    {
        //Vector3 rotMovement = new Vector3(transform.eulerAngles.x  + input.Direction.y, transform.eulerAngles.y + input.Direction.x, 0);

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotMovement), rotSpeed * Time.deltaTime);
        //float xRot = ClampAngle(transform.eulerAngles.x, -85f, 85f);
        //transform.eulerAngles = new Vector3(xRot, transform.eulerAngles.y, transform.eulerAngles.z);

        //RotateChild();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        MoveSecondAimPoint();

    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * settings.forwardSpeed, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings.maxVelocity);


        Vector3 torque = new Vector3(input.Vertical * settings.verticalTurnSpeed, input.Horizontal * settings.horizontalTurnSpeed, 0);

        //if (Mathf.Abs(MathUtils.WrapAngle(transform.eulerAngles.x)) >= 50f)
        //{
        //    torque.x = 0;
        //    rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, rb.angularVelocity.z);
        //}
        //rb.AddRelativeTorque(torque, ForceMode.VelocityChange);

        rb.AddRelativeTorque(torque, ForceMode.VelocityChange);

        if (input.Direction == Vector2.zero)
        {
            rb.AddTorque(-rb.angularVelocity * settings.dampenFactor, ForceMode.Acceleration);
            //StabilizePlaneUp();
        }

        //if (input.Direction.y == 0 /*&& input.Direction.x != 0*/) StabilizePlaneUp();

        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, settings.maxAngularVelocity);
    }

    private void Shoot()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            Vector3 direction = secondAimPoint.position - guns[i].position;
            Projectile newProjectile = Instantiate(laserPrefab, guns[i].position, Quaternion.LookRotation(direction));
            newProjectile.SetDirection(direction.normalized);
        }
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

        childRot.z = MathUtils.ClampAngle(childRot.z, -60f, 60f);

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

    

    private void StabilizePlaneUp()
    {
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);
        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);
        rb.AddTorque(axis.normalized * angle * settings.adjustFactor, ForceMode.Acceleration);
    }
}
