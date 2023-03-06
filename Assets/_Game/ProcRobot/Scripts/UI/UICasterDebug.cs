using System.Collections;
using UnityEngine;

public class UICasterDebug : MonoBehaviour
{
    private MeshRenderer _rend;

    private Coroutine _colorRoutine;

    private void Awake()
    {
        _rend = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color col)
    {
        _rend.material.color = col;

        if (_colorRoutine != null) StopCoroutine(_colorRoutine);

        _colorRoutine = StartCoroutine(GoBackToBlack());
    }

    private IEnumerator GoBackToBlack()
    {
        yield return new WaitForEndOfFrame();
        _rend.material.color = Color.black;

    }
}
