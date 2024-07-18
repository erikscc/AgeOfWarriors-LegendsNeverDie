using UnityEngine;
public interface IUnitPlacer
{
	Vector3 GetPlacementPosition();
}
public interface ICharacterState
{
	void EnterState(CharacterState character);
	void UpdateState(CharacterState character);
	void ExitState(CharacterState character);
}