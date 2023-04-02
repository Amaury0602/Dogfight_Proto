using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessSeeThrough : MonoBehaviour
{
    private RenderTexture _buffer;

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _buffer = RenderTexture.GetTemporary(source.width, source.height, 24);
        Graphics.SetRenderTarget(_buffer.colorBuffer, _buffer.depthBuffer);

        Graphics.Blit(_buffer, destination);

        _cam.targetTexture = _buffer;
    }

    private void OnPostRender()
    {
        RenderTexture.ReleaseTemporary(_buffer);
    }
}
