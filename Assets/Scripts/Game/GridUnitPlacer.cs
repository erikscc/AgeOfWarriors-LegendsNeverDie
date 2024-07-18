using UnityEngine;

public class GridUnitPlacer : UnitPlacer
{
	public GridUnitPlacer(GameObject prefab, Camera camera, LayerMask layer)
		: base(prefab, camera, layer)
	{
	}

	public override Vector3 GetPlacementPosition()
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
		{
			return SnapToGrid(hit.point);
		}
		else
		{
			Plane plane = new Plane(Vector3.up, Vector3.zero);
			if (plane.Raycast(ray, out float distance))
			{
				Vector3 point = ray.GetPoint(distance);
				return SnapToGrid(point);
			}
		}
		return Vector3.zero;
	}

	private Vector3 SnapToGrid(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x);
		int y = Mathf.RoundToInt(position.y);
		int z = Mathf.RoundToInt(position.z);
		return new Vector3(x, y, z);
	}
}
