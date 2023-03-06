using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{

    [SerializeField] private InputActionReference _action;

    public static PlayerInputs Instance;

    [SerializeField] private float _sensitivity;
    private Vector2 _lastPosition = Vector2.zero;
    private Vector2 _mouseMovement = Vector2.zero;

    public System.Action<Vector2> OnMouseMove = default;
    public System.Action OnRightMouseDown = default;
    public System.Action OnRightMouseHold = default;
    public System.Action OnRightMouseUp = default;
    public System.Action OnSpaceDown = default;

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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpaceDown?.Invoke();
        }
    }

    private void LateUpdate()
    {
        OnMouseMove?.Invoke(Mouse.current.delta.ReadValue()/*_mouseMovement*/ * _sensitivity);
    }
}
