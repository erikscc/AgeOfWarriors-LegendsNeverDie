using UnityEngine;

public class Commander : MyUnit
{
	//public MeleeUnit meleeUnitPrefab;
	//public RangedUnit rangedUnitPrefab;

	[SerializeField] private MeleeUnit meleeUnit;
	[SerializeField] private RangedUnit rangedUnit;

	void Start()
	{
		meleeUnit.SelectTarget(rangedUnit);
		rangedUnit.SelectTarget(meleeUnit);
	}
}
