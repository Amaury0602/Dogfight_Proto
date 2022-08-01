using UnityEngine;

public class PlaneController : MonoBehaviour
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

        if (input) RotatePlane(input.Direction);
    }

    public void RotatePlane(Vector2 input)
    {
        rb.angularDrag = input == Vector2.zero ? 1f : 0.1f;
        rb.AddRelativeTorque(new Vector3(input.y * settings.verticalTurnSpeed, 0, -input.x * settings.horizontalTurnSpeed), ForceMode.VelocityChange);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, settings.maxAngularVelocity);
    }
}
