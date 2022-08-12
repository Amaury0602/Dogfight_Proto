using UnityEngine;

public class KinematicPlane : MonoBehaviour
{

    private PlayerInput input;

    private Camera cam;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotSpeed;

    [SerializeField] private Transform planeChild;

    private void Awake()
    {
        cam = Camera.main;
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (input.Direction == Vector2.zero) return;

        transform.position += new Vector3(input.Direction.x, input.Direction.y, 0) * moveSpeed * Time.deltaTime;


        Vector3 viewPortPos = cam.WorldToViewportPoint(transform.position);
        Vector3 rot = Vector3.up * rotSpeed * viewPortPos.x * Time.deltaTime;

        transform.Rotate(rot, Space.World);

        //ClampPositionInViewPort();
    }

    private void ClampPositionInViewPort()
    {
        Vector3 pos = cam.WorldToViewportPoint(planeChild.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        planeChild.position = cam.ViewportToWorldPoint(pos);
    }
}
