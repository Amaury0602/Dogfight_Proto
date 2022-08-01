using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFPlaneController : MonoBehaviour
{
    [SerializeField] private PlaneSettingsSO settings;
    private PlayerInput input;
    private Rigidbody rb;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * settings.forwardSpeed, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings.maxVelocity);

        if (input) RotatePlane();
    }

    public void RotatePlane()
    {
        //rb.AddRelativeTorque(new Vector3(input.y * settings.verticalTurnSpeed, 0, -input.x * settings.horizontalTurnSpeed), ForceMode.VelocityChange);

        if (input.Direction != Vector2.zero)
        {
            // rotate towards direction
            rb.AddRelativeTorque(new Vector3(input.Vertical, input.Horizontal, 0) * settings.globalTurnSpeed, ForceMode.VelocityChange);
        
        }
        else
        {
            Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.forward, Vector3.forward);

            Vector3 axis;
            float angle;
            deltaQuat.ToAngleAxis(out angle, out axis);

            rb.AddTorque(-rb.angularVelocity * settings.dampenFactor, ForceMode.Acceleration);

            rb.AddTorque(axis.normalized * angle * settings.adjustFactor, ForceMode.Acceleration);
        }

        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, settings.maxAngularVelocity);
    }
}
