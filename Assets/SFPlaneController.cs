using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFPlaneController : MonoBehaviour
{
    [SerializeField] private PlaneSettingsSO settings;
    private PlayerInput input;
    private Rigidbody rb;


    [SerializeField] private Transform debugCube;

    public Vector3 AimCenter { get => transform.position + transform.forward * 20f;  }

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
        float speed = 0f;
        if (input.Direction != Vector2.zero)
        {
            center.position = AimCenter;
            //debugCube.position = center.position + center.TransformDirection(Vector3.right * 15f);
            Vector3 inputDir = new Vector3(input.Direction.x, input.Direction.y, 0);


            nextPos = center.position + center.TransformDirection(inputDir * 5f);
            Vector3 v = nextPos - AimCenter;
            v = Vector3.ClampMagnitude(v, 20f);

            nextPos = AimCenter + v;

            //if (Vector3.Distance(nextPos, AimCenter) >= 15f) nextPos = debugCube.position;

            //speed = 100f;
            speed = 5f;


        }
        else 
        {
            Vector3 targetPos = new Vector3(debugCube.position.x, transform.position.y, debugCube.position.z);
            Vector3 dir = targetPos - debugCube.position;
            nextPos = debugCube.position + dir;
            speed = 10f;
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
        rb.rotation = rotation;
    }

    //private void StabilizePlaneForward()
    //{
    //    Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.forward, Vector3.forward);
    //    Vector3 axis;
    //    float angle;
    //    deltaQuat.ToAngleAxis(out angle, out axis);
    //    rb.AddTorque(axis.normalized * angle * settings.adjustFactor, ForceMode.Acceleration);
    //}

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
