using UnityEngine;

public abstract class UnitPlacer
{
	protected GameObject armyUnitPrefab;
	protected Camera mainCamera;
	protected LayerMask groundLayer;

	public UnitPlacer(GameObject prefab, Camera camera, LayerMask layer)
	{
		armyUnitPrefab = prefab;
		mainCamera = camera;
		groundLayer = layer;
	}

	public abstract Vector3 GetPlacementPosition();
}