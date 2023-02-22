using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public static MouseInput Instance;

    [SerializeField] private float _sensitivity;
    private Vector2 _lastPosition = Vector2.zero;
    private Vector2 _mouseMovement = Vector2.zero;

    public System.Action<Vector2> OnMouseMove = default;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _mouseMovement = ((Vector2)Input.mousePosition - _lastPosition);

        _lastPosition = Input.mousePosition;
        OnMouseMove?.Invoke(_mouseMovement * _sensitivity);
    }
}
