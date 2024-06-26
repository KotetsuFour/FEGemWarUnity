using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : LanceAndArmor
{
	public int protection;
	public Armor(string name, int proficiency, int might, int hit, int crit, int weight, int minRange, int maxRange, int uses, int protection, int id) : base(name, proficiency, might, hit, crit, weight, minRange, maxRange, uses, id)
	{
		this.protection = protection;
        weaponType = WeaponType.ARMOR;
	}

    public override Item clone()
    {
        Armor ret = new Armor(itemName, proficiency, might, hit, crit, weight, minRange, maxRange, uses, protection, id);
        ret.magic = magic;
        ret.effectiveTypes = effectiveTypes;
        return ret;
    }
    public override string description()
    {
        return "Armor:" + (proficiency > -1 ? proficiency : "--") + " MT:" + might + " HIT:" + hit + " CRIT:" + crit
            + " WT:" + weight + " RNG:" + minRange + "~" + maxRange + "\nDEF:" + protection + " USE:" + (uses > -1 ? (usesLeft + "/" + uses) : "--/--");
    }
}
