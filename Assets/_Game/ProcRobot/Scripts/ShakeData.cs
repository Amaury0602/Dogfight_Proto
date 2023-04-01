using UnityEngine;


[CreateAssetMenu(menuName = "SO/ShakeData", fileName = "New Shake Data")]
public class ShakeData : ScriptableObject
{
    public float Amplitude;
    public float Frequency;
    public float Duration;
}
