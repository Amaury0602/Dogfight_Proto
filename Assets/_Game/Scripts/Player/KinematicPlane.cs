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



    private void Awake()
    {
        cam = Camera.main;
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Vector3 rotMovement = new Vector3(transform.eulerAngles.x  + input.Direction.y, transform.eulerAngles.y + input.Direction.x, 0);

        Quaternion rot2 = Quaternion.identity;
        if (input.Direction.y == 0)
        {
            //float angle = Vector3.Angle(transform.forward, new Vector3(transform.forward.x, 0, transform.forward.z));
            //if (transform.forward.y < 0) angle = -angle;
            rot2 = Quaternion.FromToRotation(transform.forward, new Vector3(transform.forward.x, 0, transform.forward.z));
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, rot2 * Quaternion.Euler(rotMovement), rotSpeed * Time.deltaTime);


        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < guns.Length; i++)
            {
                Projectile newProjectile = Instantiate(laserPrefab, guns[i].position, Quaternion.LookRotation(guns[i].forward));
                Vector3 direction = aimPoint.position - newProjectile.transform.position;
                newProjectile.SetDirection(direction.normalized);
            }
        }

    }
}
