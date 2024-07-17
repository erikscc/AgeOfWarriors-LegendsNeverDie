using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Target Settings")]
	[Tooltip("The target that the camera will follow.")]
	public Transform target;

	[Tooltip("The offset from the target's position.")]
	public Vector3 offset = new Vector3(0, 5, -10);

	[Header("Lerp Settings")]
	[Tooltip("The speed at which the camera will follow the target.")]
	public float followSpeed = 0.125f;

	private void LateUpdate()
	{
		if (target == null)
		{
			Debug.LogWarning("Target is not assigned in the CameraFollow script.");
			return;
		}

		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
		transform.position = smoothedPosition;

		transform.LookAt(target); // Optional: If you want the camera to look at the target
	}
}
