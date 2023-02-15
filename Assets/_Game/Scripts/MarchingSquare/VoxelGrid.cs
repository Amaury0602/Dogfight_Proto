using System;
using UnityEngine;

[SelectionBase]
public class VoxelGrid : MonoBehaviour
{
	[SerializeField] private int _resolution;
	[SerializeField] private GameObject _voxelPrefab;
	private float _voxelSize;
	private bool[] _voxels;

	private MeshRenderer[] _renderers;

	


	public void Initialize(int resolution, float size)
	{
		_resolution = resolution;
		_voxelSize = size / resolution;
		_voxels = new bool[resolution * resolution];

		_renderers = new MeshRenderer[_voxels.Length];

		for (int i = 0, y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++, i++)
			{
				CreateVoxel(i, x, y);
			}
		}

		
	}

	private void CreateVoxel(int i, int x, int y)
	{
		GameObject o = Instantiate(_voxelPrefab);
		o.transform.parent = transform;
		o.transform.localPosition = new Vector3((x + 0.5f) * _voxelSize, (y + 0.5f) * _voxelSize);
		o.transform.localScale = Vector3.one * _voxelSize;

		_renderers[i] = o.GetComponent<MeshRenderer>();
	}

	public void Apply(VoxelStencil stencil)
    {
		int xStart = stencil.XStart;
		if (xStart < 0)
		{
			xStart = 0;
		}
		int xEnd = stencil.XEnd;
		if (xEnd >= _resolution)
		{
			xEnd = _resolution - 1;
		}
		int yStart = stencil.YStart;
		if (yStart < 0)
		{
			yStart = 0;
		}
		int yEnd = stencil.YEnd;
		if (yEnd >= _resolution)
		{
			yEnd = _resolution - 1;
		}

		for (int y = yStart; y <= yEnd; y++)
		{
			int i = y * _resolution + xStart;
			for (int x = xStart; x <= xEnd; x++, i++)
			{
				_voxels[i] = stencil.Apply(x, y);
			}
		}
		SetVoxelColors();
	}

    private void SetVoxelColors()
    {
		for (int i = 0; i < _voxels.Length; i++)
		{
			if (_renderers[i] == _voxels[i]) _renderers[i].enabled = false;
		}
	}
}