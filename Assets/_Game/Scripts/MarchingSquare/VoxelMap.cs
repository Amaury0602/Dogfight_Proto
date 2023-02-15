using UnityEngine;

public class VoxelMap : MonoBehaviour
{
	[SerializeField] private float _size = 2f;
	[SerializeField] private int _voxelResolution = 8;
	[SerializeField] private int _chunkResolution = 8;
	[SerializeField] private VoxelGrid _gridPrefab;

	private float _chunkSize, _voxelSize, _halfSize;

	private VoxelGrid[] _grids;

	private PolygonCollider2D _collider;

	private Camera cam;

	private void Awake()
	{
		cam = Camera.main;

		_halfSize = _size * 0.5f;
		_chunkSize = _size / _chunkResolution;
		_voxelSize = _chunkSize / _voxelResolution;

		_grids = new VoxelGrid[_chunkResolution * _chunkResolution];

		for (int i = 0, y = 0; y < _chunkResolution; y++)
		{
			for (int x = 0; x < _chunkResolution; x++, i++)
			{
				CreateChunk(i, x, y);
			}
		}

		AddPolygonCollider();
	}

	private void AddPolygonCollider()
    {
		_collider = gameObject.AddComponent<PolygonCollider2D>();
		Vector2[] p = new Vector2[4];
		p[0] = new Vector2(-_halfSize, -_halfSize);
		p[1] = new Vector2(-_halfSize, _halfSize);
		p[2] = new Vector2(_halfSize, _halfSize);
		p[3] = new Vector2(_halfSize, -_halfSize);
		_collider.SetPath(0, p);
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit2D h = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(h && h.collider.gameObject == gameObject)
            {
				EditVoxels(transform.InverseTransformPoint(h.point));
			}
		}
	}

	private void EditVoxels(Vector3 point)
	{
		int centerX = (int)((point.x + _halfSize) / _voxelSize);
		int centerY = (int)((point.y + _halfSize) / _voxelSize);


		int xStart = (centerX - radiusIndex) / _voxelResolution;
		if (xStart < 0)
		{
			xStart = 0;
		}
		int xEnd = (centerX + radiusIndex) / _voxelResolution;
		if (xEnd >= _chunkResolution)
		{
			xEnd = _chunkResolution - 1;
		}
		int yStart = (centerY - radiusIndex) / _voxelResolution;
		if (yStart < 0)
		{
			yStart = 0;
		}
		int yEnd = (centerY + radiusIndex) / _voxelResolution;
		if (yEnd >= _chunkResolution)
		{
			yEnd = _chunkResolution - 1;
		}

		VoxelStencil newStencil = new VoxelStencil();
		newStencil.SetCenter(centerX, centerY);
		newStencil.Initialize(radiusIndex);



		int voxelYOffset = yStart * _voxelResolution;
		for (int y = yStart; y <= yEnd; y++)
		{
			int i = y * _chunkResolution + xStart;
			int voxelXOffset = xStart * _voxelResolution;
			for (int x = xStart; x <= xEnd; x++, i++)
			{
				newStencil.SetCenter(centerX - voxelXOffset, centerY - voxelYOffset);
				_grids[i].Apply(newStencil);
				voxelXOffset += _voxelResolution;
			}
			voxelYOffset += _voxelResolution;
		}
	}

	private void CreateChunk(int i, int x, int y)
	{
		VoxelGrid chunk = Instantiate(_gridPrefab, transform);
		chunk.Initialize(_voxelResolution, _chunkSize);
		chunk.transform.localPosition = new Vector3(x * _chunkSize - _halfSize, y * _chunkSize - _halfSize);
		_grids[i] = chunk;
	}



	private static string[] radiusNames = { "0", "1", "2", "3", "4", "5" };
	private int radiusIndex;
	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(4f, 4f, 150f, 500f));
		GUILayout.Label("Radius");
		radiusIndex = GUILayout.SelectionGrid(radiusIndex, radiusNames, 6);
		//fillTypeIndex = GUILayout.SelectionGrid(fillTypeIndex, fillTypeNames, 2);
		GUILayout.EndArea();
	}
}