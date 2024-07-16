using UnityEngine;

public class RangeAttackState : ICharacterState
{
	public void EnterState(CharacterState character)
	{
		// Logic to handle entering idle state
		Debug.Log("Entering Idle State");
	}

	public void UpdateState(CharacterState character)
	{
		// Logic to handle updating idle state
	}

	public void ExitState(CharacterState character)
	{
		// Logic to handle exiting idle state
	}
}