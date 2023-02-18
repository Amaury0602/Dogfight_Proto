using System;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Action<Collider> TriggerEnter = default;
    public Action<Collider> TriggerExit = default;
    [SerializeField] private LayerMask _mask;

    private void OnTriggerEnter(Collider other)
    {
        if ((_mask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            TriggerEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_mask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            TriggerExit?.Invoke(other);
        }
    }
}
