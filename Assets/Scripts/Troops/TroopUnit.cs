using UnityEngine;

[CreateAssetMenu(fileName = "TroopUnit", menuName = "ScriptableObjects/TroopUnit", order = 1)]
public class TroopUnit : ScriptableObject
{
	public GameObject deployableTroops;
	public int troopCosts;
	public Sprite buttonImage;

	[HideInInspector]
	public GameObject button;
}
