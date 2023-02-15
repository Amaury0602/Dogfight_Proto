using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _cam;

    [SerializeField] private float _minRotSpeed;
    [SerializeField] private float _maxRotSpeed;
    [SerializeField] private float _minAngleToRotate;
    [SerializeField] private float _maxAngleToFullRotation;

    public System.Action OnBigAngleChange = default;

    public static CameraRotator Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        Vector3 playerDir = (_player.position - transform.position).normalized;
        playerDir.y = 0;
        Vector3 camDir = (_cam.transform.position - transform.position).normalized;
        camDir.y = 0;
        Debug.DrawLine(transform.position, _player.position, Color.red);
        Debug.DrawLine(transform.position, _cam.transform.position, Color.green);

        Vector2 v1 = new Vector2(playerDir.x, playerDir.z);
        Vector2 v2 = new Vector2(camDir.x, camDir.z);

        float targetAngle = Vector2.Angle(v1, v2);


        if (targetAngle > 100)
        {
            OnBigAngleChange?.Invoke();
        }


        float angleRatio = Mathf.InverseLerp(_minAngleToRotate, _maxAngleToFullRotation, targetAngle);


        float speed = Mathf.Lerp(_minRotSpeed, _maxRotSpeed, angleRatio);



        Quaternion lookDir = Quaternion.LookRotation(-playerDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, speed * Time.deltaTime);
    }
}
