using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : Weapon
{
    public Club(string name, int proficiency, int might, int hit, int crit, int weight, int minRange, int maxRange, int uses, int id) : base(name, proficiency, might, hit, crit, weight, minRange, maxRange, uses, id)
    {
        weaponType = WeaponType.CLUB;
    }
    public override bool isAdvantageousAgainst(Weapon w)
    {
        return false;
    }
    public override Item clone()
    {
        Club ret = new Club(itemName, proficiency, might, hit, crit, weight, minRange, maxRange, uses, id);
        ret.magic = magic;
        ret.effectiveTypes = effectiveTypes;
        return ret;
    }
    public override string description()
    {
        return "Club:" + (proficiency > -1 ? proficiency : "--") + " MT:" + might + " HIT:" + hit + " CRIT:" + crit
            + " WT:" + weight + " RNG:" + minRange + "~" + maxRange + "\nUSE:" + (uses > -1 ? (usesLeft + "/" + uses) : "--/--");
    }

}
