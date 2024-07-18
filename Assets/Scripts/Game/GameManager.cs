using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject armyUnitPrefab;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private GameObject ground;
	[SerializeField] private float placementRadius = 0.5f; // Radius of the sphere collider

	private Camera mainCamera;
	private IUnitPlacer unitPlacer;
	private List<GameObject> placedUnits = new List<GameObject>(); // List to keep track of placed units
	private GameObject currentUnit;
	private bool isPlacing = true;

	private Bounds groundBounds;
	private Vector3 middleLineStart;
	private Vector3 middleLineEnd;

	private Vector3 lastValidPosition; // Track the last valid position

	private void Start()
	{
		mainCamera = Camera.main;
		unitPlacer = new GridUnitPlacer(armyUnitPrefab, mainCamera, groundLayer);
		CalculateGroundBounds();
		InstantiateNextUnit();
	}

	private void Update()
	{
		if (isPlacing)
		{
			UpdateUnitPosition();

			// Check if mouse click hits the ground and place unit accordingly
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 hitPoint = unitPlacer.GetPlacementPosition();
				if (IsPositionValid(hitPoint))
				{
					Debug.Log("Left mouse button clicked. Placing unit...");
					PlaceCurrentUnit();
					InstantiateNextUnit();
				}
				else
				{
					Debug.Log("No valid hit point found.");
				}
			}
		}
	}

	private void CalculateGroundBounds()
	{
		groundBounds = ground.GetComponent<Renderer>().bounds;

		// Define the middle line based on the ground bounds
		middleLineStart = new Vector3(groundBounds.center.x, 0, groundBounds.min.z);
		middleLineEnd = new Vector3(groundBounds.center.x, 0, groundBounds.max.z);

		Debug.Log("Ground bounds calculated:");
		Debug.Log("Center: " + groundBounds.center);
		Debug.Log("Size: " + groundBounds.size);
	}

	private void InstantiateNextUnit()
	{
		Vector3 unitPosition = unitPlacer.GetPlacementPosition();
		Debug.Log("Attempting to instantiate unit at position: " + unitPosition);

		if (IsPositionValid(unitPosition))
		{
			currentUnit = Instantiate(armyUnitPrefab, unitPosition, Quaternion.identity);
			Debug.Log("Unit instantiated at position: " + unitPosition);
			FaceTowardsCamera(currentUnit);
			lastValidPosition = unitPosition; // Update last valid position

			// Add the placed unit to the list
			placedUnits.Add(currentUnit);

			// Disable colliders during placement
			SetCollidersActive(currentUnit, false);

			// Change renderer colors based on validity
			ChangeRendererColors(currentUnit, true);
		}
		else
		{
			if (currentUnit == null)
			{
				currentUnit = Instantiate(armyUnitPrefab, unitPosition, Quaternion.identity);
				Debug.Log("Initial unit instantiated at position: " + unitPosition);
				FaceTowardsCamera(currentUnit);

				// Disable colliders during placement
				SetCollidersActive(currentUnit, false);

				// Change renderer colors based on validity
				ChangeRendererColors(currentUnit, false);
			}
			else
			{
				currentUnit.transform.position = lastValidPosition; // Keep last valid position
				Debug.Log("No valid position found. Using last valid position: " + lastValidPosition);
			}
		}
	}

	private void UpdateUnitPosition()
	{
		if (currentUnit != null)
		{
			Vector3 unitPosition = unitPlacer.GetPlacementPosition();
			Debug.Log("Updating unit position to: " + unitPosition);

			if (IsPositionValid(unitPosition))
			{
				currentUnit.transform.position = unitPosition;
				Debug.Log("Unit position updated to valid position: " + unitPosition);
				lastValidPosition = unitPosition; // Update last valid position

				// Change renderer colors based on validity
				ChangeRendererColors(currentUnit, true);
			}
			else
			{
				currentUnit.transform.position = lastValidPosition;
				Debug.Log("No valid position found. Using last valid position: " + lastValidPosition);

				// Change renderer colors based on validity
				ChangeRendererColors(currentUnit, false);
			}
		}
	}

	private void PlaceCurrentUnit()
	{
		Debug.Log("Placing current unit.");
		// Perform any additional logic before placing the current unit
		// For example, saving data or triggering events

		// Add the current unit to the list of placed units
		placedUnits.Add(currentUnit);

		// Enable colliders after placement
		SetCollidersActive(currentUnit, true);

		// Turn off emission color
		SetEmissionColor(currentUnit, Color.black);

		currentUnit = null;
	}

	private bool IsPositionValid(Vector3 position)
	{
		// Check if the position is within the valid bounds
		if (position.x >= groundBounds.center.x)
		{
			Debug.Log("Position is invalid: Out of bounds.");
			return false;
		}

		// Offset position for OverlapSphere
		Vector3 offsetPosition = position + Vector3.up * 0.5f;

		// Use OverlapSphere to check for collisions
		Collider[] hitColliders = Physics.OverlapSphere(offsetPosition, placementRadius);
		foreach (Collider collider in hitColliders)
		{
			if (collider.gameObject != currentUnit && !groundLayer.Contains(collider.gameObject.layer))
			{
				Debug.Log("Position is invalid: Colliding with object.");
				return false;
			}
		}

		Debug.Log("Position is valid: No collisions detected.");
		return true;
	}

	private void FaceTowardsCamera(GameObject unit)
	{
		Vector3 directionToCamera = mainCamera.transform.forward;
		directionToCamera.y = 0; // Keep the direction horizontal
		unit.transform.rotation = Quaternion.LookRotation(directionToCamera);
		Debug.Log("Facing camera with rotation: " + unit.transform.rotation.eulerAngles);
	}

	private void ChangeRendererColors(GameObject unit, bool isValid)
	{
		if (isValid)
		{
			SetEmissionColor(unit, Color.green);
		}
		else
		{
			SetEmissionColor(unit, Color.red);
		}
	}

	private void SetEmissionColor(GameObject unit, Color color)
	{
		Renderer[] renderers = unit.GetComponentsInChildren<Renderer>();

		foreach (Renderer renderer in renderers)
		{
			foreach (Material mat in renderer.materials)
			{
				mat.SetColor("_EmissionColor", color);

				if (color == Color.black)
				{
					mat.DisableKeyword("_EMISSION");
				}
				else
				{
					mat.EnableKeyword("_EMISSION");
				}
			}
		}
	}

	private void SetCollidersActive(GameObject unit, bool active)
	{
		Collider[] colliders = unit.GetComponentsInChildren<Collider>();
		foreach (Collider collider in colliders)
		{
			collider.enabled = active;
		}
	}

	private void OnDrawGizmos()
	{
		if (isPlacing && currentUnit != null)
		{
			Gizmos.color = Color.yellow;
			Vector3 offsetPosition = currentUnit.transform.position + Vector3.up * 0.5f;
			Gizmos.DrawWireSphere(offsetPosition, placementRadius);
		}
	}
}

public static class LayerMaskExtensions
{
	public static bool Contains(this LayerMask mask, int layer)
	{
		return mask == (mask | (1 << layer));
	}
}
