using UnityEngine;

public class GridUnitPlacer : IUnitPlacer
{
	private GameObject unitPrefab;
	private Camera mainCamera;
	private LayerMask groundLayer;

	public GridUnitPlacer(GameObject unitPrefab, Camera mainCamera, LayerMask groundLayer)
	{
		this.unitPrefab = unitPrefab;
		this.mainCamera = mainCamera;
		this.groundLayer = groundLayer;
	}

	public Vector3 GetPlacementPosition()
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
		{
			return hit.point;
		}
		return Vector3.zero;
	}
}
