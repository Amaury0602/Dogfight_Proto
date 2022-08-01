using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public float Horizontal { get => Input.GetAxisRaw("Horizontal"); }
    public float Vertical { get => Input.GetAxisRaw("Vertical"); }
    public Vector2 Direction { get => new Vector2(Horizontal, Vertical); }

    public bool NoInput { get => new Vector2(Horizontal, Vertical) == Vector2.zero; }
}
