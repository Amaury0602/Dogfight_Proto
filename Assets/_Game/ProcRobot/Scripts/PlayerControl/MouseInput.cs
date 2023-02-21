using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public static MouseInput Instance;

    [SerializeField] private float _speed;
    private Vector2 _lastPosition = Vector2.zero;
    private Vector2 _currentPosition = Vector2.zero;
    private Vector2 _mouseMovement = Vector2.zero;

    public System.Action<Vector2> OnMouseMove = default;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _mouseMovement = _lastPosition - (Vector2)Input.mousePosition;
        if (_mouseMovement != Vector2.zero) OnMouseMove?.Invoke(_mouseMovement);

        _lastPosition = Input.mousePosition;
    }
}
