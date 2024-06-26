using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CampaignSaveData
{
    public int iron;
    public int steel;
    public int silver;
	public int bonusEXP;

	public string[] unitName;
	public int[] unitClass;
	public string[] description;
	public int[] maxHP;
	public int[] currentHP;
	public int[] strength;
	public int[] magic;
	public int[] skill;
	public int[] speed;
	public int[] luck;
	public int[] defense;
	public int[] resistance;
	public int[] constitution;
	public int[] movement;

	public int[] hpGrowth;
	public int[] strengthGrowth;
	public int[] magicGrowth;
	public int[] skillGrowth;
	public int[] speedGrowth;
	public int[] luckGrowth;
	public int[] defenseGrowth;
	public int[] resistanceGrowth;
	public int[] level;
	public int[] experience;

	public int[] personalItemId;
	public int[] personalItemUsesLeft;
	public int[] heldWeaponId;
	public int[] heldWeaponUsesLeft;
	public int[] heldItemId;
	public int[] heldItemUsesLeft;

	public int[] weaponType;
	public int[] proficiency;

	public bool[] isEssential;
	public bool[] isLeader;
	public int[] equipped; //0 = personal, 1 = held, 2 = none
	public bool[] isExhausted;

	public string[] deathQuote;

	public int[] supportId1;
	public int[] supportId2;
	public int[] fusionSkill1;
	public int[] fusionSkill2;
	public int[] fusionSkillBonus;
	public int[] affinity;

	public string[] spriteName;
	public string[] talkIconName;

	public string[] punitName;
	public int[] punitClass;
	public string[] pdescription;
	public int[] pmaxHP;
	public int[] pcurrentHP;
	public int[] pstrength;
	public int[] pmagic;
	public int[] pskill;
	public int[] pspeed;
	public int[] pluck;
	public int[] pdefense;
	public int[] presistance;
	public int[] pconstitution;
	public int[] pmovement;

	public int[] phpGrowth;
	public int[] pstrengthGrowth;
	public int[] pmagicGrowth;
	public int[] pskillGrowth;
	public int[] pspeedGrowth;
	public int[] pluckGrowth;
	public int[] pdefenseGrowth;
	public int[] presistanceGrowth;
	public int[] plevel;
	public int[] pexperience;

	public int[] ppersonalItemId;
	public int[] ppersonalItemUsesLeft;
	public int[] pheldWeaponId;
	public int[] pheldWeaponUsesLeft;
	public int[] pheldItemId;
	public int[] pheldItemUsesLeft;

	public int[] pweaponType;
	public int[] pproficiency;

	public bool[] pisEssential;
	public bool[] pisLeader;
	public int[] pequipped; //0 = personal, 1 = held, 2 = none
	public bool[] pisExhausted;

	public string[] pdeathQuote;

	public int[] psupportId1;
	public int[] psupportId2;
	public int[] pfusionSkill1;
	public int[] pfusionSkill2;
	public int[] pfusionSkillBonus;
	public int[] pAffinity;

	public string[] pspriteName;
	public string[] ptalkIconName;

	public string[][] supportLog;
	public int[] supportLevels;
	public int[][] supportRequirements;

	public int scene;

	public int[][] convoyIds;
	public int[][] convoyDurabilities;

	public int savefile;

	public int chapterPrep;
	public int[] positions;

	public CampaignSaveData()
    {
        iron = CampaignData.iron;
        steel = CampaignData.steel;
        silver = CampaignData.silver;
		bonusEXP = CampaignData.bonusEXP;

		unitName = new string[CampaignData.members.Count];
		unitClass = new int[CampaignData.members.Count];
		description = new string[CampaignData.members.Count];
		maxHP = new int[CampaignData.members.Count];
		currentHP = new int[CampaignData.members.Count];
		strength = new int[CampaignData.members.Count];
		magic = new int[CampaignData.members.Count];
		skill = new int[CampaignData.members.Count];
		speed = new int[CampaignData.members.Count];
		luck = new int[CampaignData.members.Count];
		defense = new int[CampaignData.members.Count];
		resistance = new int[CampaignData.members.Count];
		constitution = new int[CampaignData.members.Count];
		movement = new int[CampaignData.members.Count];

		hpGrowth = new int[CampaignData.members.Count];
		strengthGrowth = new int[CampaignData.members.Count];
		magicGrowth = new int[CampaignData.members.Count];
		skillGrowth = new int[CampaignData.members.Count];
		speedGrowth = new int[CampaignData.members.Count];
		luckGrowth = new int[CampaignData.members.Count];
		defenseGrowth = new int[CampaignData.members.Count];
		resistanceGrowth = new int[CampaignData.members.Count];
		level = new int[CampaignData.members.Count];
		experience = new int[CampaignData.members.Count];

		personalItemId = new int[CampaignData.members.Count];
		personalItemUsesLeft = new int[CampaignData.members.Count];
		heldWeaponId = new int[CampaignData.members.Count];
		heldWeaponUsesLeft = new int[CampaignData.members.Count];
		heldItemId = new int[CampaignData.members.Count];
		heldItemUsesLeft = new int[CampaignData.members.Count];

		weaponType = new int[CampaignData.members.Count];;
		proficiency = new int[CampaignData.members.Count];;

		isEssential = new bool[CampaignData.members.Count];
		isLeader = new bool[CampaignData.members.Count];
		equipped = new int[CampaignData.members.Count]; //0 = personal, 1 = held, 2 = none
		isExhausted = new bool[CampaignData.members.Count];

		deathQuote = new string[CampaignData.members.Count];

		supportId1 = new int[CampaignData.members.Count];
		supportId2 = new int[CampaignData.members.Count];
		fusionSkill1 = new int[CampaignData.members.Count];
		fusionSkill2 = new int[CampaignData.members.Count];
		fusionSkillBonus = new int[CampaignData.members.Count];
		affinity = new int[CampaignData.members.Count];

		spriteName = new string[CampaignData.members.Count];
		talkIconName = new string[CampaignData.members.Count];

		for (int q = 0; q < CampaignData.members.Count; q++)
        {
			Unit m = CampaignData.members[q];

			unitName[q] = m.unitName;
			unitClass[q] = m.unitClass.id;
			description[q] = m.description;
			maxHP[q] = m.maxHP;
			currentHP[q] = m.currentHP;
			strength[q] = m.strength;
			magic[q] = m.magic;
			skill[q] = m.skill;
			speed[q] = m.speed;
			luck[q] = m.luck;
			defense[q] = m.defense;
			resistance[q] = m.resistance;
			constitution[q] = m.constitution;
			movement[q] = m.movement;

			hpGrowth[q] = m.hpGrowth;
			strengthGrowth[q] = m.strengthGrowth;
			magicGrowth[q] = m.magicGrowth;
			skillGrowth[q] = m.skillGrowth;
			speedGrowth[q] = m.speedGrowth;
			luckGrowth[q] = m.luckGrowth;
			defenseGrowth[q] = m.defenseGrowth;
			resistanceGrowth[q] = m.resistanceGrowth;
			level[q] = m.level;
			experience[q] = m.experience;

			personalItemId[q] = m.personalItem == null ? -1 : m.personalItem.id;
			personalItemUsesLeft[q] = m.personalItem == null ? -1 : m.personalItem.usesLeft;
			heldWeaponId[q] = m.heldWeapon == null ? -1 : m.heldWeapon.id;
			heldWeaponUsesLeft[q] = m.heldWeapon == null ? -1 : m.heldWeapon.usesLeft;
			heldItemId[q] = m.heldItem == null ? -1 : m.heldItem.id;
			heldItemUsesLeft[q] = m.heldItem == null ? -1 : m.heldItem.usesLeft;

			weaponType[q] = (int)m.weaponType;
			proficiency[q] = m.proficiency;

			isEssential[q] = m.isEssential;
			isLeader[q] = m.isLeader;
			equipped[q] = m.equipped; //0 = personal, 1 = held, 2 = none
			isExhausted[q] = m.isExhausted;

			deathQuote[q] = m.deathQuote == null ? "" : m.deathQuote.filename;

			supportId1[q] = m.supportId1;
			supportId2[q] = m.supportId2;
			fusionSkill1[q] = (int)m.fusionSkill1;
			fusionSkill2[q] = (int)m.fusionSkill2;
			fusionSkillBonus[q] = (int)m.fusionSkillBonus;
			affinity[q] = (int)m.affinity;

			spriteName[q] = m.spriteName;
			talkIconName[q] = m.talkIconName;
		}

		punitName = new string[CampaignData.prisoners.Count];
		punitClass = new int[CampaignData.prisoners.Count];
		pdescription = new string[CampaignData.prisoners.Count];
		pmaxHP = new int[CampaignData.prisoners.Count];
		pcurrentHP = new int[CampaignData.prisoners.Count];
		pstrength = new int[CampaignData.prisoners.Count];
		pmagic = new int[CampaignData.prisoners.Count];
		pskill = new int[CampaignData.prisoners.Count];
		pspeed = new int[CampaignData.prisoners.Count];
		pluck = new int[CampaignData.prisoners.Count];
		pdefense = new int[CampaignData.prisoners.Count];
		presistance = new int[CampaignData.prisoners.Count];
		pconstitution = new int[CampaignData.prisoners.Count];
		pmovement = new int[CampaignData.prisoners.Count];

		phpGrowth = new int[CampaignData.prisoners.Count];
		pstrengthGrowth = new int[CampaignData.prisoners.Count];
		pmagicGrowth = new int[CampaignData.prisoners.Count];
		pskillGrowth = new int[CampaignData.prisoners.Count];
		pspeedGrowth = new int[CampaignData.prisoners.Count];
		pluckGrowth = new int[CampaignData.prisoners.Count];
		pdefenseGrowth = new int[CampaignData.prisoners.Count];
		presistanceGrowth = new int[CampaignData.prisoners.Count];
		plevel = new int[CampaignData.prisoners.Count];
		pexperience = new int[CampaignData.prisoners.Count];

		ppersonalItemId = new int[CampaignData.prisoners.Count];
		ppersonalItemUsesLeft = new int[CampaignData.prisoners.Count];
		pheldWeaponId = new int[CampaignData.prisoners.Count];
		pheldWeaponUsesLeft = new int[CampaignData.prisoners.Count];
		pheldItemId = new int[CampaignData.prisoners.Count];
		pheldItemUsesLeft = new int[CampaignData.prisoners.Count];

		pweaponType = new int[CampaignData.prisoners.Count]; ;
		pproficiency = new int[CampaignData.prisoners.Count]; ;

		pisEssential = new bool[CampaignData.prisoners.Count];
		pisLeader = new bool[CampaignData.prisoners.Count];
		pequipped = new int[CampaignData.prisoners.Count]; //0 = personal, 1 = held, 2 = none
		pisExhausted = new bool[CampaignData.prisoners.Count];

		pdeathQuote = new string[CampaignData.prisoners.Count];

		psupportId1 = new int[CampaignData.prisoners.Count];
		psupportId2 = new int[CampaignData.prisoners.Count];
		pfusionSkill1 = new int[CampaignData.prisoners.Count];
		pfusionSkill2 = new int[CampaignData.prisoners.Count];
		pfusionSkillBonus = new int[CampaignData.prisoners.Count];
		pAffinity = new int[CampaignData.prisoners.Count];

		pspriteName = new string[CampaignData.prisoners.Count];
		ptalkIconName = new string[CampaignData.prisoners.Count];

		for (int q = 0; q < CampaignData.prisoners.Count; q++)
		{
			Unit m = CampaignData.prisoners[q].unit;

			punitName[q] = m.unitName;
			punitClass[q] = m.unitClass.id;
			pdescription[q] = m.description;
			pmaxHP[q] = m.maxHP;
			pcurrentHP[q] = m.currentHP;
			pstrength[q] = m.strength;
			pmagic[q] = m.magic;
			pskill[q] = m.skill;
			pspeed[q] = m.speed;
			pluck[q] = m.luck;
			pdefense[q] = m.defense;
			presistance[q] = m.resistance;
			pconstitution[q] = m.constitution;
			pmovement[q] = m.movement;

			phpGrowth[q] = m.hpGrowth;
			pstrengthGrowth[q] = m.strengthGrowth;
			pmagicGrowth[q] = m.magicGrowth;
			pskillGrowth[q] = m.skillGrowth;
			pspeedGrowth[q] = m.speedGrowth;
			pluckGrowth[q] = m.luckGrowth;
			pdefenseGrowth[q] = m.defenseGrowth;
			presistanceGrowth[q] = m.resistanceGrowth;
			plevel[q] = m.level;
			pexperience[q] = m.experience;

			ppersonalItemId[q] = m.personalItem == null ? -1 : m.personalItem.id;
			ppersonalItemUsesLeft[q] = m.personalItem == null ? -1 : m.personalItem.usesLeft;
			pheldWeaponId[q] = m.heldWeapon == null ? -1 : m.heldWeapon.id;
			pheldWeaponUsesLeft[q] = m.heldWeapon == null ? -1 : m.heldWeapon.usesLeft;
			pheldItemId[q] = m.heldItem == null ? -1 : m.heldItem.id;
			pheldItemUsesLeft[q] = m.heldItem == null ? -1 : m.heldItem.usesLeft;

			pweaponType[q] = (int)m.weaponType;
			pproficiency[q] = m.proficiency;

			pisEssential[q] = m.isEssential;
			pisLeader[q] = m.isLeader;
			pequipped[q] = m.equipped; //0 = personal, 1 = held, 2 = none
			pisExhausted[q] = m.isExhausted;

			pdeathQuote[q] = m.deathQuote == null ? "" : m.deathQuote.filename;

			psupportId1[q] = m.supportId1;
			psupportId2[q] = m.supportId2;
			pfusionSkill1[q] = (int)m.fusionSkill1;
			pfusionSkill2[q] = (int)m.fusionSkill2;
			pfusionSkillBonus[q] = (int)m.fusionSkillBonus;
			pAffinity[q] = (int)m.affinity;

			pspriteName[q] = m.spriteName;
			ptalkIconName[q] = m.talkIconName;
		}

		supportLog = CampaignData.getSupportLog();
		supportLevels = CampaignData.getSupportLevels();
		supportRequirements = CampaignData.getSupportRequirements();
		scene = CampaignData.scene;
		convoyIds = new int[CampaignData.getConvoyIds().Length][];
		convoyDurabilities = new int[CampaignData.getConvoyDurabilities().Length][];
		for (int q = 0; q < convoyIds.Length; q++)
        {
			convoyIds[q] = CampaignData.getConvoyIds()[q].ToArray();
			convoyDurabilities[q] = CampaignData.getConvoyDurabilities()[q].ToArray();
        }
		savefile = CampaignData.savefile;
		chapterPrep = CampaignData.chapterPrep;
		positions = CampaignData.positions;
	}

	public void unload(Unit unitToInstantiate)
    {
		CampaignData.iron = iron;
		CampaignData.steel = steel;
		CampaignData.silver = silver;
		CampaignData.bonusEXP = bonusEXP;

		CampaignData.members.Clear();
		for (int q = 0; q < unitName.Length; q++)
        {
			Unit mem = MonoBehaviour.Instantiate(unitToInstantiate);
			mem.constructor(unitName[q], CampaignData.getUnitClasses()[unitClass[q]], description[q], maxHP[q], strength[q], magic[q],
			skill[q], speed[q], luck[q], defense[q], resistance[q], constitution[q], movement[q],
			hpGrowth[q], strengthGrowth[q], magicGrowth[q], skillGrowth[q], speedGrowth[q], luckGrowth[q],
			defenseGrowth[q], resistanceGrowth[q], personalItemId[q] == -1 ? null : CampaignData.getItems()[personalItemId[q]].clone(), (Weapon.WeaponType)weaponType[q], proficiency[q],
			Unit.UnitTeam.PLAYER, supportId1[q], supportId2[q], (Unit.Affinity)affinity[q], spriteName[q]);
			mem.currentHP = currentHP[q];
			mem.level = level[q];
			mem.experience = experience[q];
			if (mem.personalItem != null)
            {
				mem.personalItem.usesLeft = personalItemUsesLeft[q];
            }
			if (heldWeaponId[q] != -1)
            {
				mem.heldWeapon = (Weapon)CampaignData.items[heldWeaponId[q]].clone();
				mem.heldWeapon.usesLeft = heldWeaponUsesLeft[q];
            }
			if (heldItemId[q] != -1)
            {
				mem.heldItem = CampaignData.items[heldItemId[q]].clone();
				mem.heldItem.usesLeft = heldItemUsesLeft[q];
            }

			mem.isEssential = isEssential[q];
			mem.isLeader = isLeader[q];
			mem.equipped = equipped[q]; //0 = personal, 1 = held, 2 = none
			mem.isExhausted = isExhausted[q];

			mem.deathQuote = deathQuote[q].Equals("") ? null : new DialogueEvent(0, deathQuote[q]);

			mem.fusionSkill1 = (Unit.FusionSkill)fusionSkill1[q];
			mem.fusionSkill2 = (Unit.FusionSkill)fusionSkill2[q];
			mem.fusionSkillBonus = (Unit.FusionSkill)fusionSkillBonus[q];

			mem.setTalkIcon(talkIconName[q]);

			CampaignData.members.Add(mem);
		}

		CampaignData.prisoners.Clear();
		for (int q = 0; q < punitName.Length; q++)
		{
			Unit mem = MonoBehaviour.Instantiate(unitToInstantiate);
			mem.constructor(punitName[q], CampaignData.classes[punitClass[q]], pdescription[q], pmaxHP[q], pstrength[q], pmagic[q],
			pskill[q], pspeed[q], pluck[q], pdefense[q], presistance[q], pconstitution[q], pmovement[q],
			phpGrowth[q], pstrengthGrowth[q], pmagicGrowth[q], pskillGrowth[q], pspeedGrowth[q], pluckGrowth[q],
			pdefenseGrowth[q], presistanceGrowth[q], ppersonalItemId[q] == -1 ? null : CampaignData.items[ppersonalItemId[q]].clone(), (Weapon.WeaponType)pweaponType[q], pproficiency[q],
			Unit.UnitTeam.ENEMY, psupportId1[q], psupportId2[q], (Unit.Affinity)pAffinity[q], spriteName[q]);
			mem.currentHP = currentHP[q];
			mem.level = plevel[q];
			mem.experience = pexperience[q];
			if (mem.personalItem != null)
			{
				mem.personalItem.usesLeft = ppersonalItemUsesLeft[q];
			}
			if (pheldWeaponId[q] != -1)
			{
				mem.heldWeapon = (Weapon)CampaignData.items[pheldWeaponId[q]].clone();
				mem.heldWeapon.usesLeft = pheldWeaponUsesLeft[q];
			}
			if (pheldItemId[q] != -1)
			{
				mem.heldItem = CampaignData.items[pheldItemId[q]].clone();
				mem.heldItem.usesLeft = pheldItemUsesLeft[q];
			}

			mem.isEssential = pisEssential[q];
			if (mem.isEssential)
            {
				mem.setTalkIcon("crystal_gem_star.png");
            }
			mem.isLeader = pisLeader[q];
			mem.equipped = pequipped[q]; //0 = personal, 1 = held, 2 = none
			mem.isExhausted = pisExhausted[q];

			mem.deathQuote = pdeathQuote[q].Equals("") ? null : new DialogueEvent(0, pdeathQuote[q]);

			mem.fusionSkill1 = (Unit.FusionSkill)pfusionSkill1[q];
			mem.fusionSkill2 = (Unit.FusionSkill)pfusionSkill2[q];
			mem.fusionSkillBonus = (Unit.FusionSkill)pfusionSkillBonus[q];

			mem.setTalkIcon(ptalkIconName[q]);

			CampaignData.prisoners.Add(new Gemstone(mem));
		}
		CampaignData.supportLog = supportLog;
		CampaignData.supportLevels = supportLevels;
		CampaignData.supportRequirements = supportRequirements;
		CampaignData.scene = scene;
		if (CampaignData.scene == 0)
        {
			CampaignData.scene = 1;
        }
		for (int q = 0; q < CampaignData.convoyIds.Length; q++)
        {
			CampaignData.convoyIds[q] = new List<int>(convoyIds[q]);
			CampaignData.convoyDurabilities[q] = new List<int>(convoyDurabilities[q]);
		}
		CampaignData.savefile = savefile;
		CampaignData.chapterPrep = chapterPrep;
		CampaignData.positions = positions;
	}
}
