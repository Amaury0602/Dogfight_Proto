using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ShooterBase : MonoBehaviour
{
    public abstract Action<RaycastHit> OnShoot { get; set; }

    [field: SerializeField] public LayerMask DetectionLayer { get; private set; }
}
