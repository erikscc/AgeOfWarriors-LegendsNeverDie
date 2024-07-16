using UnityEngine;

public class ProjectileControll : MonoBehaviour
{
	public float depth;
	public float speed = 5.0f;  // Set the speed of the projectile in the inspector
	public float rotationSpeed = 5.0f;  // Set the rotation speed of the projectile in the inspector
	private Vector3 targetPosition;
	private bool isMoving = false;
	[SerializeField] private Transform destination;
	[SerializeField] private GameObject explosionPS;
	void Update()
	{

		if (isMoving)
		{
			MoveTowardsTarget();
		}
		else
		{
			SetTarget(ScreenToWorldPosition(GetRandomScreenPosition(), depth));

		}
	}

	public void SetTarget(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
		isMoving = true;
	}

	void MoveTowardsTarget()
	{
		Vector3 direction = (targetPosition - transform.position).normalized;
		float distanceThisFrame = speed * Time.deltaTime;

		Quaternion targetRotation = Quaternion.LookRotation(direction);

		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

		if (Vector3.Distance(transform.position, targetPosition) <= distanceThisFrame)
		{
			// Reached the target position
			transform.position = targetPosition;
			isMoving = false;

		}
		else
		{
			// Move towards the target
			transform.Translate(Vector3.forward * distanceThisFrame);
		}
	}
	Vector3 GetRandomScreenPosition()
	{
		float randomX = Random.Range(0, Screen.width);
		float randomY = Random.Range(0, Screen.height);
		return new Vector3(randomX, randomY, 0);
	}

	Vector3 ScreenToWorldPosition(Vector3 screenPosition, float depth)
	{
		Vector3 screenPositionWithDepth = new Vector3(screenPosition.x, screenPosition.y, depth);
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPositionWithDepth);
		return worldPosition;
	}
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger reached " + other.name);

		Debug.Log("Child : " + other.transform.GetChild(0).name);

		other.transform.GetChild(0).gameObject.SetActive(true);

		other.transform.GetChild(0).GetComponentInChildren<PSMeshRendererUpdater>().enabled = true;
		other.transform.GetChild(0).GetComponentInChildren<PSMeshRendererUpdater>().UpdateMeshEffect();

		Instantiate(explosionPS, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}
}
