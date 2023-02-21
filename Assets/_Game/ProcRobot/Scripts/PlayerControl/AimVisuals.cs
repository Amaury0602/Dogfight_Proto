using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimVisuals : MonoBehaviour
{
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 2;
    }

    public void EnableLine()
    {
        _line.positionCount = 2;
    }
    
    public void DisableLine()
    {
        _line.positionCount = 0;
    }


    public void UpdateLine(Vector3 startPos, Vector3 endPos)
    {
        _line.SetPosition(0, startPos);
        _line.SetPosition(1, endPos);
    }
}
