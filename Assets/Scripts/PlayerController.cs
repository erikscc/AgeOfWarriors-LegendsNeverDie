using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private StateController _characterState;

	// Start is called before the first frame update
	void Awake()
	{
		_characterState = GetComponent<StateController>();

	}
	private void Start()
	{
		StartCoroutine(RandomizeState(5f));
	}

	private IEnumerator RandomizeState(float delay)
	{
		_characterState.ChangeAnimationState(StateController.GetRandomState());

		yield return new WaitForSeconds(delay);

		StartCoroutine(RandomizeState(delay));
	}
}
