using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpitMovement : MonoBehaviour
{
	[SerializeField] private GameObject explosionPS;
	[SerializeField] private Vector3 targetPosition;
	private int speed = 50;
	private int rotationSpeed = 5;
	private bool isMoving;

	public SpitMovement(Vector3 target)
	{
		targetPosition = target;
		isMoving = true;
	}
	private void Start()
	{
		InitSpit(new Vector3(-0.26f, 1.68f, 1.18f));
	}
	// Update is called once per frame
	void Update()
	{
		MoveTowardsTarget();
	}

	public void InitSpit(Vector3 target)
	{
		targetPosition = target;
		Debug.Log("Target : " + target);
		isMoving = true;
	}

	void MoveTowardsTarget()
	{
		
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1f);
		
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
