using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUICursor : MonoBehaviour
{
    public static PlayerUICursor Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdatePosition(Vector2 position)
    {
        transform.position = position;
    }

    public void Move(Vector2 position)
    {
        transform.position = position;
    }
}
