using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject armyUnitPrefab;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private GameObject ground;

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
		}
		else
		{
			if (currentUnit == null)
			{
				currentUnit = Instantiate(armyUnitPrefab, unitPosition, Quaternion.identity);
				Debug.Log("Initial unit instantiated at position: " + unitPosition);
				FaceTowardsCamera(currentUnit);
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
			}
			else
			{
				currentUnit.transform.position = lastValidPosition;
				Debug.Log("No valid position found. Using last valid position: " + lastValidPosition);
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

		currentUnit = null;
	}

	private bool IsPositionValid(Vector3 position)
	{
		bool isValid = position.x < groundBounds.center.x;
		Debug.Log("Position validity check for position " + position + ": " + isValid);
		return isValid;
	}

	private void FaceTowardsCamera(GameObject unit)
	{
		Vector3 directionToCamera = mainCamera.transform.forward;
		directionToCamera.y = 0; // Keep the direction horizontal
		unit.transform.rotation = Quaternion.LookRotation(directionToCamera);
		Debug.Log("Facing camera with rotation: " + unit.transform.rotation.eulerAngles);
	}
}