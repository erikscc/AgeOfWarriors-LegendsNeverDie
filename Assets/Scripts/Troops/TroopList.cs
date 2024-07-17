using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TroopList", menuName = "ScriptableObjects/TroopList", order = 2)]
public class TroopList : ScriptableObject
{
	public List<TroopUnit> troopUnits;
}
