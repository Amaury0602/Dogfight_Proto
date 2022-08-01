using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlaneController controller { get; private set; }
    public static Player I;

    private void Awake()
    {
        I = this;
    }
}
