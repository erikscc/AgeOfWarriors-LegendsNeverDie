using UnityEngine;



public class CharacterState : MonoBehaviour
{
	[SerializeField] private Animator _animator;

	private ICharacterState _currentState;

	private void Start()
	{
		TransitionToState(new IdleState(_animator));
	}

	public void TransitionToState(ICharacterState newState)
	{
		_currentState?.ExitState(this);
		_currentState = newState;
		_currentState.EnterState(this);

		Debug.LogFormat("Current state is : {0}",_currentState);
	}

	// Define methods to handle different actions and transition states
	public void Walk()
	{
		TransitionToState(new IdleState(_animator));
	}

	public void Run()
	{
		TransitionToState(new RunState());
	}

	public void Jump()
	{
		TransitionToState(new JumpState());
	}

	public void MeleeAttack()
	{
		TransitionToState(new MeleeAttackState());
	}

	public void RangeAttack()
	{
		TransitionToState(new RangeAttackState());
	}

	public void FinishingAttack()
	{
		TransitionToState(new FinishingAttackState());
	}
}