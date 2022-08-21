using UnityEngine;

public class AimMenu : MonoBehaviour
{

    [SerializeField] private Transform worldTarget;
    [SerializeField] private RectTransform crossHair;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(worldTarget.position);
        crossHair.position = screenPos;
    }
}
