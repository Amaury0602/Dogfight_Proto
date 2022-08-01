using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFPlaneController : MonoBehaviour
{
    [SerializeField] private PlaneSettingsSO settings;
    private PlayerInput input;
    private Rigidbody rb;


    [SerializeField] private Transform debugCube;

    public Vector3 ForwardPosition { get => transform.position + transform.forward * 10f;  }

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        debugCube.position = ForwardPosition;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * settings.forwardSpeed, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings.maxVelocity);

        if (input) RotatePlane();
    }

    private void Update()
    {
        Vector3 nextPos = ForwardPosition;
        float speed;
        if (input.Direction != Vector2.zero)
        {
            nextPos = debugCube.position + new Vector3(input.Direction.x, input.Direction.y, 0);

            if (Vector3.Distance(nextPos, ForwardPosition) >= 15f) nextPos = debugCube.position;

            speed = 20f;
        }
        else
        {
            Vector3 dir = (ForwardPosition - debugCube.position).normalized;
            nextPos = debugCube.position + dir;
            speed = 15f;
        }

        //nextPos = Vector3.ClampMagnitude(nextPos, 5f);
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
