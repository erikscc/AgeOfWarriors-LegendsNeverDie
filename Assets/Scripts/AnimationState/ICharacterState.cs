
public interface ICharacterState
{
	void EnterState(CharacterState character);
	void UpdateState(CharacterState character);
	void ExitState(CharacterState character);
}