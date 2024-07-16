using UnityEngine;

public enum State
{
	Idle = 0,
	Walk = 1,
	Run = 2,
	Attack = 3

}

public class StateController : MonoBehaviour
{
	[SerializeField] private Animator animator;

	public void ChangeAnimationState(State animState)
	{
		animator.SetInteger("State", (int)animState);
	}

	public static State GetRandomState()
	{
		State[] states = (State[])System.Enum.GetValues(typeof(State));

		int randomIndex = Random.Range(0, states.Length);

		Debug.LogFormat("State Choosed: {0}", states[randomIndex]);

		return states[randomIndex];
	}

}
