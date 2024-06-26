using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : SwordAndFist
{
	public Fist(string name, int proficiency, int might, int hit, int crit, int weight, int minRange, int maxRange, int uses, int id) : base(name, proficiency, might, hit, crit, weight, minRange, maxRange, uses, id)
	{
		weaponType = WeaponType.FIST;
	}
	public override Item clone()
	{
		Fist ret = new Fist(itemName, proficiency, might, hit, crit, weight, minRange, maxRange, uses, id);
		ret.magic = magic;
		ret.effectiveTypes = effectiveTypes;
		return ret;
	}
	public override string description()
	{
		return "Fist:" + (proficiency > -1 ? proficiency : "--") + " MT:" + might + " HIT:" + hit + " CRIT:" + crit
			+ " WT:" + weight + " RNG:" + minRange + "~" + maxRange + "\nUSE:" + (uses > -1 ? (usesLeft + "/" + uses) : "--/--");
	}

}
