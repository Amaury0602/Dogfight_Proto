using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFPlaneController : MonoBehaviour
{
    [SerializeField] private PlaneSettingsSO settings;
    private PlayerInput input;
    private Rigidbody rb;


    [SerializeField] private Transform debugCube;

    public Vector3 AimCenter { get => transform.position + transform.forward * 50f;  }

    [SerializeField] private Transform center;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        debugCube.position = AimCenter;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * settings.forwardSpeed, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings.maxVelocity);
        

        RotateTowardsAim();
        //if (input) RotatePlane();
    }


    private void Update()
    {
        Vector3 nextPos = debugCube.position;
        float speed = 5f;
        center.position = AimCenter;
        if (input.Direction != Vector2.zero)
        {
            Vector3 inputDir = new Vector3(input.Direction.x, input.Direction.y, 0);


            nextPos = center.position + center.TransformDirection(inputDir * settings.horizontalTurnSpeed);
            Vector3 v = nextPos - AimCenter;
            v = Vector3.ClampMagnitude(v, 20f);
            nextPos = AimCenter + v;
            speed = 5f;


        }
        else 
        {
            Vector3 targetPos = new Vector3(center.position.x, transform.position.y, center.position.z);
            Vector3 dir = targetPos - debugCube.position;
            nextPos = debugCube.position + dir;
            speed = settings.verticalTurnSpeed;
        }
        debugCube.position = Vector3.Lerp(debugCube.position, nextPos, speed * Time.deltaTime);
    }

    public void RotatePlane()
    {
        if (input.Direction != Vector2.zero)
        {
            //Vector3 torque = new Vector3(input.Vertical * settings.verticalTurnSpeed, input.Horizontal * settings.horizontalTurnSpeed, 0);

            //if (Mathf.Abs(WrapAngle(transform.eulerAngles.x)) >= 50f)
            //{
            //    torque.x = 0;
            //    rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, rb.angularVelocity.z);
            //}
            //rb.AddRelativeTorque(torque, ForceMode.VelocityChange);
        }
        else
        {
            //rb.AddTorque(-rb.angularVelocity * settings.dampenFactor, ForceMode.Acceleration);
            //StabilizePlaneUp();
        }

        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, settings.maxAngularVelocity);
    }

    private void RotateTowardsAim()
    {
        Quaternion rotation = Quaternion.LookRotation(debugCube.position - rb.position);

        rb.rotation = Quaternion.Slerp(rb.rotation, rotation, 5 * Time.deltaTime);

    }

    private void StabilizePlaneForward()
    {
        Vector3 direction = debugCube.position - rb.position;
        print(direction);
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.forward, direction.normalized);
        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);
        rb.AddTorque(axis.normalized * angle * settings.adjustFactor, ForceMode.Acceleration);
    }

    private void StabilizePlaneUp()
    {
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);
        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);
        rb.AddTorque(axis.normalized * angle * settings.adjustFactor, ForceMode.Acceleration);
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
