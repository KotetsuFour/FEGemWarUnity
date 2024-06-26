using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LanceAndArmor : Weapon
{
    public LanceAndArmor(string name, int proficiency, int might, int hit, int crit, int weight, int minRange, int maxRange, int uses, int id) : base(name, proficiency, might, hit, crit, weight, minRange, maxRange, uses, id)
    {
    }

    public override bool isAdvantageousAgainst(Weapon w)
    {
        return w is SwordAndFist;
    }

}
