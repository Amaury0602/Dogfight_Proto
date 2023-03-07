using UnityEngine;
using UnityEngine.UI;

public class TargetLockVisual : MonoBehaviour
{
    [SerializeField] private Image _image = default;

    private bool _focused = false;

    public void Initialize()
    {
        _image.enabled = false;
    }

    public void Focus(Vector3 position)
    {
        if (!_focused) 
        {
            _focused = true;
            _image.enabled = true;
        } 


        transform.position = position;
    }

    public void UnFocus()
    {
        if (_focused)
        {
            _focused = false;
            _image.enabled = false;
        }
    }
}
