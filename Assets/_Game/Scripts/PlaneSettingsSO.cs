using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = ("SO/Settings/PlaneController"))]
public class PlaneSettingsSO : ScriptableObject
{
    public float forwardSpeed;
    public float maxVelocity;
    public float maxAngularVelocity;
    public float horizontalTurnSpeed;
    public float verticalTurnSpeed;
    public float maxTurn;
}
