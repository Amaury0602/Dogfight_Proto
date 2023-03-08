using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TargetLockVisual : MonoBehaviour
{
    [SerializeField] private Image _image = default;

    private bool _focused = false;

    private Tween _focusTween;
    private Sequence _focusSequence;

    public void Initialize()
    {
        _focusSequence = DOTween.Sequence();
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }

    public void Focus(Vector3 position)
    {
        if (!_focused) 
        {
            _focused = true;
            _image.enabled = true;

            if (_focusSequence != null && _focusSequence.IsActive())
            {
                _focusSequence.Kill();
            }

            transform.localScale = Vector3.one * 5f;

            _focusSequence
                .Append(transform.DOScale(Vector3.one, 0.35f))
                .Append(_image.DOFade(1, 0.35f));
        }

        transform.position = position;
    }

    public void UnFocus()
    {
        if (_focused)
        {
            _focused = false;
            _image.enabled = false;

            if (_focusSequence != null && _focusSequence.IsActive())
            {
                _focusSequence.Kill();
            }

            _focusSequence
                .Append(transform.DOScale(Vector3.one * 5f, 0.25f))
                .Append(_image.DOFade(0f, 0.25f));

        }
    }
}
