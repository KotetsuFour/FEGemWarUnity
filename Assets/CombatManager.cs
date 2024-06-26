using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager
{
	/*
	 * [0] = atkHP
	 * [1] = atkMt
	 * [2] = atkHit
	 * [3] = atkCrit
	 * [4] = atkAttacks (1 for one attack, 2 for double attack)
	 * [5] = atkBrave (1 if no brave effect, 2 if brave effect)
	 * [6] = dfdHP
	 * [7] = dfdMt
	 * [8] = dfdHit
	 * [9] = dfdCrit
	 * [10] = dfdAttacks (0 for no counter, 1 for one attack, 2 for double attack)
	 * [11] = dfdBrave (1 if no brave effect, 2 if brave effect)
	 */
	public static int[] getBattleForecast(Unit atk, Unit dfd, Weapon atkWep, Weapon dfdWep,
			Tile atkTile, Tile dfdTile, List<Unit> atkAllies, List<Unit> dfdAllies)
	{
		int[] ret = new int[12];
		ret[0] = atk.currentHP;
		if (atkWep != null && atkWep.magic)
		{
			ret[1] = atk.magic + atkWep.might - dfd.resistance;
		}
		else
		{
			ret[1] = atk.strength - dfd.defense;
			if (atkWep != null)
			{
				ret[1] += atkWep.might;
			}
		}
		if (dfdWep is Armor) {
			ret[1] -= ((Armor)dfdWep).protection;
		}
		ret[2] = atk.getBaseAccuracy() - dfd.getBaseAvoidance();
		if (atkWep != null)
		{
			ret[2] += atkWep.hit;
		}
		if (!dfd.unitClass.isFlying())
		{
			ret[2] -= dfdTile.avoidBonus;
		}
		if (atkWep != null && atkWep.isAdvantageousAgainst(dfdWep))
		{
			ret[2] += 15;
		}
		ret[3] = atk.getBaseCrit() - dfd.luck;
		if (atkWep != null)
		{
			ret[3] += atkWep.crit;
		}
		int atkAttackSpeed = atk.speed;
		if (atkWep != null)
		{
			atkAttackSpeed -= Mathf.Max(0, atkWep.weight - atk.constitution);
		}
		int dfdAttackSpeed = dfd.speed;
		if (dfdWep != null)
		{
			dfdAttackSpeed -= Mathf.Max(0, dfdWep.weight - dfd.constitution);
		}
		if (atkAttackSpeed - dfdAttackSpeed >= 4)
		{
			ret[4] = 2;
		}
		else
		{
			ret[4] = 1;
		}
		if (atkWep != null && atkWep.brave)
		{
			ret[5] = 2;
		}
		else
		{
			ret[5] = 1;
		}

		ret[6] = dfd.currentHP;

		int distance = Mathf.Abs(atkTile.x - dfdTile.x) + Mathf.Abs(atkTile.y - dfdTile.y);
		if ((distance != 1 && dfdWep == null) ||
			(dfdWep != null && (distance < dfdWep.minRange || distance > dfdWep.maxRange)))
		{
			for (int q = 0; q < ret.Length; q++)
			{
				ret[q] = Mathf.Max(0, ret[q]);
			}
			ret[10] = 0;

			handleSupports(atk, dfd, atkAllies, dfdAllies, ret);
			return ret;
		}

		if (dfdWep != null && dfdWep.magic)
		{
			ret[7] = dfd.magic + dfdWep.might - atk.resistance;
		}
		else
		{
			ret[7] = dfd.strength - atk.defense;
			if (dfdWep != null)
			{
				ret[7] += dfdWep.might;
			}
		}
		if (atkWep is Armor) {
			ret[7] -= ((Armor)atkWep).protection;
		}
		ret[8] = dfd.getBaseAccuracy() - atk.getBaseAvoidance();
		if (dfdWep != null)
		{
			ret[8] += dfdWep.hit;
		}
		if (dfdWep != null && dfdWep.isAdvantageousAgainst(atkWep))
		{
			ret[8] += 15;
		}
		if (!atk.unitClass.isFlying())
		{
			ret[8] -= atkTile.avoidBonus;
		}
		ret[9] = dfd.getBaseCrit() - atk.luck;
		if (dfdWep != null)
		{
			ret[9] += dfdWep.crit;
		}
		if (dfdAttackSpeed - atkAttackSpeed >= 4)
		{
			ret[10] = 2;
		}
		else
		{
			ret[10] = 1;
		}
		if (dfdWep != null && dfdWep.brave)
		{
			ret[11] = 2;
		}
		else
		{
			ret[11] = 1;
		}
		for (int q = 0; q < ret.Length; q++)
        {
			ret[q] = Mathf.Max(0, ret[q]);
        }
		handleSupports(atk, dfd, atkAllies, dfdAllies, ret);
		return ret;
	}

	/*
	 * Continual sequence of
	 * [0] = attacker (0 is atk, 1 is dfd),
	 * [1] = result of attack (0 is miss, 1 is hit, 2 is crit),
	 * [2] = special notes (maybe used for different skills)
	 */
	public static int[] decideBattle(int[] forecast)
	{
		int[] ret = new int[((forecast[4] * forecast[5]) + (forecast[10] * forecast[11])) * 3];
		int idx1 = 0;
		int idx2 = 1;
		int idx3 = 2;
		for (int q = 0; q < forecast[5]; q++)
		{
			ret[idx1] = 0;
			int rngNumber = trueHit();
			if (rngNumber < forecast[2])
			{
				if (Random.Range(0, 100) < forecast[3])
				{
					ret[idx2] = 2;
				}
				else
				{
					ret[idx2] = 1;
				}
			}
			else
			{
				ret[idx2] = 0;
			}
			idx1 += 3;
			idx2 += 3;
			idx3 += 3;
		}
		int num = forecast[10] * forecast[11];
		for (int q = 0; q < num; q++)
		{
			ret[idx1] = 1;
			int rngNumber = trueHit();
			if (rngNumber < forecast[8])
			{
				if (Random.Range(0, 100) < forecast[9])
				{
					ret[idx2] = 2;
				}
				else
				{
					ret[idx2] = 1;
				}
			}
			else
			{
				ret[idx2] = 0;
			}
			idx1 += 3;
			idx2 += 3;
			idx3 += 3;
		}
		if (forecast[4] == 2)
		{
			for (int q = 0; q < forecast[5]; q++)
			{
				ret[idx1] = 0;
				int rngNumber = trueHit();
				if (rngNumber < forecast[2])
				{
					if (Random.Range(0, 100) < forecast[3])
					{
						ret[idx2] = 2;
					}
					else
					{
						ret[idx2] = 1;
					}
				}
				else
				{
					ret[idx2] = 0;
				}
				idx1 += 3;
				idx2 += 3;
				idx3 += 3;
			}
		}

		return ret;
	}

	public static bool performAttack(Unit atk, Weapon atkWep, Unit dfd, int damage, bool crit)
	{
		atkWep.loseDurability(1);
		return dfd.takeDamage(damage, crit);
	}

	private static int trueHit()
    {
		return (Random.Range(0, 100) + Random.Range(0, 100)) / 2;
    }

	private static void handleSupports(Unit atk, Unit dfd,
		List<Unit> atkAllies, List<Unit> dfdAllies, int[] forecast)
    {
		Unit[] atkPartners = CampaignData.findLivingSupportPartners(atk);
		if (atkAllies.Contains(atkPartners[0]))
        {
			int[] buffs = getSupportBuffs(atk, atkPartners[0], atk.supportId1);
			applySupportBuffs(buffs, forecast, true);
        }
		if (atkAllies.Contains(atkPartners[1]))
		{
			int[] buffs = getSupportBuffs(atk, atkPartners[1], atk.supportId2);
			applySupportBuffs(buffs, forecast, true);
		}

		if (atk.isFusion())
		{
			int[] buffs = getFusionBuffs(atk);
			applySupportBuffs(buffs, forecast, true);
		}

		Unit[] dfdPartners = CampaignData.findLivingSupportPartners(dfd);
		if (dfdAllies.Contains(dfdPartners[0]))
        {
			int[] buffs = getSupportBuffs(dfd, dfdPartners[0], dfd.supportId1);
			applySupportBuffs(buffs, forecast, false);
        }
		if (dfdAllies.Contains(dfdPartners[1]))
		{
			int[] buffs = getSupportBuffs(dfd, dfdPartners[1], dfd.supportId2);
			applySupportBuffs(buffs, forecast, false);
		}
		if (dfd.isFusion())
		{
			int[] buffs = getFusionBuffs(dfd);
			applySupportBuffs(buffs, forecast, false);
		}
	}

	/*
	 * [0] = ATK
	 * [1] = DEF
	 * [2] = HIT
	 * [3] = AVO
	 * [4] = CRT
	 * [5] = SEC
	 */
	private static int[] getSupportBuffs(Unit partner1, Unit partner2, int supportIdx)
    {
		int multiplier = Mathf.Min(3, CampaignData.getSupportLevelAtIndex(supportIdx));
		float[] buffsHalf1 = null;
		float[] buffsHalf2 = null;
		if (partner1.affinity == Unit.Affinity.FIRE)
        {
			buffsHalf1 = new float [] { 0.5f, 0, 2.5f, 2.5f, 2.5f, 0};
        } else if (partner1.affinity == Unit.Affinity.WATER)
        {
			buffsHalf1 = new float[] { 0.5f, 0.5f, 0, 0, 2.5f, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.EARTH)
		{
			buffsHalf1 = new float[] { 0, 0, 0, 5f, 2.5f, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.WIND)
		{
			buffsHalf1 = new float[] { 0.5f, 0, 2.5f, 0, 2.5f, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.LIGHTNING)
		{
			buffsHalf1 = new float[] { 0, 0.5f, 0, 2.5f, 2.5f, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.ICE)
		{
			buffsHalf1 = new float[] { 0, 0.5f, 2.5f, 2.5f, 0, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.ANIMA)
		{
			buffsHalf1 = new float[] { 0.5f, 0.5f, 0, 2.5f, 0, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.LIGHT)
		{
			buffsHalf1 = new float[] { 0.5f, 0.5f, 2.5f, 0, 2.5f, 0 };
		}
		else if (partner1.affinity == Unit.Affinity.DARK)
		{
			buffsHalf1 = new float[] { 0, 0, 2.5f, 2.5f, 2.5f, 2.5f };
		}
		else if (partner1.affinity == Unit.Affinity.HEAVEN)
		{
			buffsHalf1 = new float[] { 0.5f, 0.5f, 5f, 0, 0, 0 };
		}

		if (partner2.affinity == Unit.Affinity.FIRE)
		{
			buffsHalf2 = new float[] { 0.5f, 0, 2.5f, 2.5f, 2.5f, 0 };
		}
		else if (partner2.affinity == Unit.Affinity.WATER)
		{
			buffsHalf2 = new float[] { 0.5f, 0.5f, 0, 0, 2.5f, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.EARTH)
		{
			buffsHalf2 = new float[] { 0, 0, 0, 5f, 2.5f, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.WIND)
		{
			buffsHalf2 = new float[] { 0.5f, 0, 2.5f, 0, 2.5f, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.LIGHTNING)
		{
			buffsHalf2 = new float[] { 0, 0.5f, 0, 2.5f, 2.5f, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.ICE)
		{
			buffsHalf2 = new float[] { 0, 0.5f, 2.5f, 2.5f, 0, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.ANIMA)
		{
			buffsHalf2 = new float[] { 0.5f, 0.5f, 0, 2.5f, 0, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.LIGHT)
		{
			buffsHalf2 = new float[] { 0.5f, 0.5f, 2.5f, 0, 2.5f, 0 };
		}
		else if (partner2.affinity == Unit.Affinity.DARK)
		{
			buffsHalf2 = new float[] { 0, 0, 2.5f, 2.5f, 2.5f, 2.5f };
		}
		else if (partner2.affinity == Unit.Affinity.HEAVEN)
        {
			buffsHalf2 = new float[] { 0.5f, 0.5f, 5f, 0, 0, 0};
        }

		float[] buffsTotal = new float[buffsHalf1.Length];
		for (int q = 0; q < buffsTotal.Length; q++)
        {
			buffsTotal[q] = (buffsHalf1[q] + buffsHalf2[q]) * multiplier;
        }

		int[] ret = new int[buffsTotal.Length];
		for (int q = 0; q < ret.Length; q++)
        {
			ret[q] = Mathf.FloorToInt(buffsTotal[q]);
        }

		return ret;
	}

	private static int[] getFusionBuffs(Unit fusion)
    {
		if (fusion.affinity == Unit.Affinity.FIRE)
		{
			return new int[] { 1, 0, 2, 2, 2, 0 };
		}
		else if (fusion.affinity == Unit.Affinity.WATER)
		{
			return new int[] { 1, 1, 0, 0, 2, 2 };
		}
		else if (fusion.affinity == Unit.Affinity.EARTH)
		{
			return new int[] { 0, 0, 0, 5, 2, 2 };
		}
		else if (fusion.affinity == Unit.Affinity.WIND)
		{
			return new int[] { 1, 0, 2, 0, 2, 2 };
		}
		else if (fusion.affinity == Unit.Affinity.LIGHTNING)
		{
			return new int[] { 0, 1, 0, 2, 2, 2 };
		}
		else if (fusion.affinity == Unit.Affinity.ICE)
		{
			return new int[] { 0, 1, 2, 2, 0, 2 };
		}
		else if (fusion.affinity == Unit.Affinity.ANIMA)
		{
			return new int[] { 1, 1, 0, 2, 0, 2 };
		}
		else if (fusion.affinity == Unit.Affinity.LIGHT)
		{
			return new int[] { 1, 1, 2, 0, 2, 0 };
		}
		else if (fusion.affinity == Unit.Affinity.DARK)
		{
			return new int[] { 0, 0, 2, 2, 2, 2 };
		} else if (fusion.affinity == Unit.Affinity.HEAVEN)
        {
			return new int[] { 1, 1, 5, 0, 0, 0};
        }
		return null;
	}

	private static void applySupportBuffs(int[] buffs, int[] forecast, bool atk)
    {
		if (atk)
        {
			forecast[1] += buffs[0];
			forecast[7] -= buffs[1];
			forecast[2] += buffs[2];
			forecast[8] -= buffs[3];
			forecast[3] += buffs[4];
			forecast[9] -= buffs[5];
        } else
        {
			forecast[7] += buffs[0];
			forecast[1] -= buffs[1];
			forecast[8] += buffs[2];
			forecast[2] -= buffs[3];
			forecast[9] += buffs[4];
			forecast[3] -= buffs[5];
		}
	}
}
