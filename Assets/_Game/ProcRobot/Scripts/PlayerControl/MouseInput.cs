using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInput : MonoBehaviour
{

    [SerializeField] private InputActionReference _action;

    public static MouseInput Instance;

    [SerializeField] private float _sensitivity;
    private Vector2 _lastPosition = Vector2.zero;
    private Vector2 _mouseMovement = Vector2.zero;

    public System.Action<Vector2> OnMouseMove = default;
    public System.Action OnRightMouseDown = default;
    public System.Action OnRightMouseHold = default;
    public System.Action OnRightMouseUp = default;

    private bool _rightMouseDown = false;
    

    private void Awake()
    {
        Instance = this;
        _rightMouseDown = false;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //_mouseMovement = ((Vector2)Input.mousePosition - _lastPosition);
        //_lastPosition = Input.mousePosition;

        OnMouseMove?.Invoke(Mouse.current.delta.ReadValue()/*_mouseMovement*/ * _sensitivity);

        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseDown?.Invoke();
        }
        
        if (Input.GetMouseButton(1))
        {
            OnRightMouseHold?.Invoke();
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            OnRightMouseUp?.Invoke();
        }

    }
}
