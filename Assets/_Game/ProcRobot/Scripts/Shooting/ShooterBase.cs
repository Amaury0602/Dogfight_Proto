using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ShooterBase : MonoBehaviour
{
    public abstract Action<RaycastHit> OnShoot { get; set; }
    public abstract Action<Transform> OnShotHoming { get; set; }

    public virtual Action<Vector3> OnShootInDirection { get; set; }

    [field: SerializeField] public LayerMask DetectionLayer { get; private set; }
}
