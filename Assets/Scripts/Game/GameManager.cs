using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject armyUnitPrefab;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private GameObject ground;

	private Camera mainCamera;
	private UnitPlacer unitPlacer;
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

			if (Input.GetMouseButtonDown(0) && IsPositionValid(currentUnit.transform.position))
			{
				Debug.Log("Left mouse button clicked. Placing unit...");
				PlaceCurrentUnit();
				InstantiateNextUnit();
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
			FaceMiddleLine(currentUnit);
			lastValidPosition = unitPosition; // Update last valid position
		}
		else
		{
			if (currentUnit == null)
			{
				currentUnit = Instantiate(armyUnitPrefab, unitPosition, Quaternion.identity);
				Debug.Log("Initial unit instantiated at position: " + unitPosition);
				FaceMiddleLine(currentUnit);
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
		currentUnit = null;
	}

	private bool IsPositionValid(Vector3 position)
	{
		bool isValid = position.x < groundBounds.center.x;
		Debug.Log("Position validity check for position " + position + ": " + isValid);
		return isValid;
	}

	private void FaceMiddleLine(GameObject unit)
	{
		Vector3 middlePoint = (middleLineStart + middleLineEnd) / 2;
		Vector3 directionToMiddleLine = middlePoint - unit.transform.position;
		directionToMiddleLine.y = 0; // Keep the unit upright
		unit.transform.rotation = Quaternion.LookRotation(directionToMiddleLine);
		Debug.Log("Facing middle line with rotation: " + unit.transform.rotation.eulerAngles);
	}
}
