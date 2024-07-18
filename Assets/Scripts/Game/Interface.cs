using UnityEngine;
public interface IUnitPlacer
{
	void PlaceUnit(Vector3 position);
}

public interface ICharacterState
{
	void EnterState(CharacterState character);
	void UpdateState(CharacterState character);
	void ExitState(CharacterState character);
}