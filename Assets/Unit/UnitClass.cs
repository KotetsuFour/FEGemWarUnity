using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClass
{
    public UnitType[] unitTypes;
	public string className;
	public UnitClass promotion;

	public int minMaxHP;
	public int minStrength;
	public int minMagic;
	public int minSkill;
	public int minSpeed;
	public int minLuck;
	public int minDefense;
	public int minResistance;
	public int minConstitution;
	public int minMovement;
	public int rawEXPReward;

	public int id;

	public UnitClass(string className, int minMaxHP, int minStrength, int minMagic, int minSkill,
			int minSpeed, int minLuck, int minDefense, int minResistance, int minConstitution,
			int minMovement, UnitType[] unitTypes, int rawEXPReward, int id)
    {
		this.className = className;
		this.minMaxHP = minMaxHP;
		this.minStrength = minStrength;
		this.minMagic = minMagic;
		this.minSkill = minSkill;
		this.minSpeed = minSpeed;
		this.minLuck = minLuck;
		this.minDefense = minDefense;
		this.minResistance = minResistance;
		this.minConstitution = minConstitution;
		this.minMovement = minMovement;
		this.unitTypes = unitTypes;
		this.rawEXPReward = rawEXPReward;
		this.id = id;
	}

	public bool isFlying()
    {
		foreach (UnitType type in unitTypes)
        {
			if (type == UnitType.FLYING)
            {
				return true;
            }
        }
		return false;
    }

	public enum UnitType
    {
        FLYING, HEAVY, QUARTZ, ORGANIC, NOBLE
    }
}
