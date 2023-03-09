using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TargetLockVisual : MonoBehaviour
{
    [SerializeField] private Image _image = default;

    public bool Locked { get; private set; } = false;
    public Transform Target { get; private set; }


    private Tween _focusTween;
    private Sequence _focusSequence;

    public void Initialize()
    {
        _focusSequence = DOTween.Sequence();
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }

    public void Focus(Vector3 position, Transform target)
    {
        if (!Locked) 
        {
            Target = target;

            Locked = true;
            _image.enabled = true;

            if (_focusSequence != null && _focusSequence.IsActive())
            {
                _focusSequence.Kill();
            }

            transform.localScale = Vector3.one * 5f;
            transform.rotation = Quaternion.Euler(35f * Vector3.forward);

            _focusSequence
                .Append(transform.DOScale(Vector3.one, 0.35f))
                .Append(transform.DORotate(Vector3.zero, 0.35f))
                .Append(_image.DOFade(1, 0.35f));
        }

        transform.position = position;
    }

    public void UnFocus()
    {
        if (Locked)
        {
            Target = null;

            Locked = false;

            if (_focusSequence != null && _focusSequence.IsActive())
            {
                _focusSequence.Kill();
            }

            _focusSequence
                .Append(transform.DOScale(Vector3.one * 3f, 0.25f))
                .Append(_image.DOFade(0f, 0.25f));

        }
    }
}
