using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CampaignData
{
    public static UnitClass[] classes;
    public static Item[] items;


    public static int iron;
    public static int steel;
    public static int silver;
    public static int bonusEXP;

    public static List<Unit> members = new List<Unit>();
    public static List<Gemstone> prisoners = new List<Gemstone>();

    public static string[][] supportLog;
    public static int[] supportLevels;
    public static int[][] supportRequirements;

    public static int scene = 1;

    public static Unit unitToInstantiate;

    public static List<int>[] convoyIds = new List<int>[10];
    public static List<int>[] convoyDurabilities = new List<int>[10];

    public static int savefile;

    public static int chapterPrep;
    public static int[] positions;

    public static Unit findUnit(string name)
    {
        foreach (Unit u in members)
        {
            if (u.unitName.Equals(name))
            {
                return u;
            }
        }
        return null;
    }

    public static UnitClass[] getUnitClasses()
    {
        if (classes == null)
        {
            UnitClass lord = new UnitClass("Lord", 18, 4, 0, 2, 3, 0, 2, 0, 5, 6, new UnitClass.UnitType[] { }, 30, 0);
            UnitClass servant = new UnitClass("Servant", 20, 3, 0, 5, 6, 0, 2, 0, 6, 6, new UnitClass.UnitType[] { }, 30, 1);
            UnitClass soldier = new UnitClass("Soldier", 20, 4, 0, 4, 5, 0, 2, 0, 8, 6, new UnitClass.UnitType[] { UnitClass.UnitType.QUARTZ }, 30, 2);
            UnitClass architect = new UnitClass("Architect", 20, 4, 0, 4, 5, 0, 2, 0, 8, 6, new UnitClass.UnitType[] { UnitClass.UnitType.HEAVY }, 30, 3);
            UnitClass diplomat = new UnitClass("Diplomat", 18, 0, 2, 2, 3, 0, 0, 2, 4, 6, new UnitClass.UnitType[] { UnitClass.UnitType.NOBLE }, 30, 4);
            UnitClass guard = new UnitClass("Guard", 16, 1, 0, 1, 6, 0, 0, 0, 5, 5, new UnitClass.UnitType[] { }, 30, 5);
            UnitClass priestess = new UnitClass("Priestess", 16, 2, 0, 3, 5, 0, 1, 0, 4, 6, new UnitClass.UnitType[] { }, 30, 6);
            UnitClass pilot = new UnitClass("Pilot", 16, 2, 3, 3, 6, 0, 2, 3, 4, 8, new UnitClass.UnitType[] {UnitClass.UnitType.FLYING}, 30, 7);
            UnitClass elite_quartz = new UnitClass("Elite Quartz", 20, 7, 1, 5, 9, 0, 5, 1, 7, 6, new UnitClass.UnitType[] { UnitClass.UnitType.QUARTZ }, 45, 8);
            UnitClass topaz_fusion = new UnitClass("Topaz Fusion", 26, 7, 1, 5, 2, 0, 12, 1, 10, 6, new UnitClass.UnitType[] { }, 45, 9);
            classes = new UnitClass[] { lord, servant, soldier, architect, diplomat, guard, priestess, pilot,
                elite_quartz, topaz_fusion};
        }

        return classes;
    }
    public static Item[] getItems()
    {
        if (items == null)
        {
            Weapon rose_shield = new Armor("Rose Quartz Shield", -1, 1, 80, 5, 2, 1, 2, -1, 5, 0);
            Weapon pearl_spear = new Lance("Pearl Spear", -1, 7, 70, 0, 9, 1, 2, -1, 1);
            Weapon biggs_whip = new Whip("Jasper Whip", -1, 7, 70, 0, 9, 1, 2, -1, 2);
            Weapon ocean_club = new Club("Jasper Mace", -1, 7, 70, 0, 9, 1, 1, -1, 3);
            Weapon bismuth_hammer = new Axe("Bismuth Hammer", -1, 7, 70, 0, 9, 1, 1, -1, 4);
            Weapon iron_sword = new Sword("Iron Sword", 0, 6, 70, 0, 6, 1, 1, 40, 5);
            Weapon quartz_axe = new Axe("Quartz Axe", -1, 9, 60, 0, 12, 1, 1, -1, 6);
            Weapon palm_laser = new SpecialWeapon("Palm Laser", -1, 5, 70, 0, 4, 1, 2, -1, 7); palm_laser.magic = true;
            Weapon ruby_pike = new Lance("Ruby Pike", -1, 6, 100, 5, 6, 1, 1, -1, 8);
            Weapon moon_bow = new Bow("Moonstone Bow", -1, 10, 65, 0, 11, 2, 3, -1, 9);
            Weapon priest_bow = new Bow("Priestly Bow", -1, 10, 65, 0, 14, 2, 2, -1, 10);
            Weapon iron_lance = new Lance("Iron Lance", 0, 7, 70, 0, 9, 1, 1, 40, 11);
            Weapon iron_axe = new Axe("Iron Axe", 0, 9, 65, 0, 10, 1, 1, 30, 12);
            Weapon iron_gauntlet = new Fist("Iron Gauntlet", 0, 3, 85, 5, 3, 1, 1, 50, 13);
            Weapon iron_shield = new Armor("Iron Shield", 0, 1, 80, 0, 3, 1, 2, 40, 3, 14);
            Weapon iron_whip = new Whip("Iron Whip", 0, 6, 70, 0, 7, 1, 2, 40, 15);
            Weapon iron_bow = new Bow("Iron Bow", 0, 7, 65, 0, 6, 2, 2, 40, 16);
            Weapon iron_club = new Club("Iron Club", 0, 6, 75, 5, 9, 1, 1, 30, 17);
            UsableItem currentHP = new UsableItem("Rose's Tear", "Heals 10 HP", UsableItem.StatToEdit.CURRENTHP, 10, 3, 18);
            UsableItem maxHP = new UsableItem("Snerson Robe", "Increases Max HP by 5", UsableItem.StatToEdit.MAXHP, 5, 1, 19);
            UsableItem str = new UsableItem("Strength Ring", "Increases Strength by 2", UsableItem.StatToEdit.STRENGTH, 2, 1, 20);
            UsableItem mag = new UsableItem("", "Incrases Magic by 2", UsableItem.StatToEdit.MAGIC, 2, 1, 21);
            UsableItem skl = new UsableItem("", "Increases Skill by 2", UsableItem.StatToEdit.SKILL, 2, 1, 22);
            UsableItem spd = new UsableItem("", "Increases Speed by 2", UsableItem.StatToEdit.SPEED, 2, 1, 23);
            UsableItem lck = new UsableItem("Moon Goddess Icon", "Increases Luck by 2", UsableItem.StatToEdit.LUCK, 2, 1, 24);
            UsableItem def = new UsableItem("", "Increases Defense by 2", UsableItem.StatToEdit.DEFENSE, 2, 1, 25);
            UsableItem res = new UsableItem("", "Increases Resistance by 2", UsableItem.StatToEdit.RESISTANCE, 2, 1, 26);
            UsableItem mov = new UsableItem("", "Increases Movement by 2", UsableItem.StatToEdit.MOVEMENT, 2, 1, 27);
            Weapon ship_laser = new SpecialWeapon("Ship Laser", -1, 14, 70, 10, 12, 1, 2, -1, 28); ship_laser.magic = true;
            Weapon elite_sword = new Sword("Elite Quartz Sword", -1, 15, 55, 0, 16, 1, 1, -1, 29);
            Weapon citrine_sword = new Sword("Citrine Sword", -1, 14, 60, 10, 15, 1, 1, -1, 30);
            Weapon aventurine_axe = new Sword("Aventurine Axe", -1, 13, 60, 10, 14, 1, 1, -1, 31);
            Weapon pacifist_gauntlet = new Fist("Pacifist Gauntlet", -1, 0, 100, 0, 0, 1, 1, -1, 32);
            Weapon topaz_lance = new Lance("Topaz Fusion Lance", -1, 18, 60, 0, 18, 1, 1, -1, 33);
            Weapon guard_shield = new Armor("Guard Shield", -1, 7, 80, 0, 7, 1, 1, -1, 3, 34);
            Weapon iron_blade = new Sword("Iron Blade", 10, 12, 55, 0, 15, 1, 1, 30, 35); iron_blade.effectiveTypes = new UnitClass.UnitType[] {UnitClass.UnitType.QUARTZ};

            items = new Item[] {rose_shield, pearl_spear, biggs_whip, ocean_club, bismuth_hammer,
                iron_sword, quartz_axe, palm_laser, ruby_pike, moon_bow, priest_bow, iron_lance,
                iron_axe, iron_gauntlet, iron_shield, iron_whip, iron_bow, iron_club, currentHP, maxHP,
                str, mag, skl, spd, lck, def, res, mov, ship_laser, elite_sword, citrine_sword,
                aventurine_axe, pacifist_gauntlet, topaz_lance, guard_shield, iron_blade};
        }
        return items;
    }

    public static string[][] getSupportLog()
    {
        if (supportLog == null)
        {
            supportLog = new string[][] {
                new string[]{"RoseAndPearlC.txt", "RoseAndPearlB.txt", "RoseAndPearlA.txt"}, //[0]Rose and Pearl
                new string[]{"", "", ""}, //[1]Rose and Garnet
                new string[]{"", "", ""}, //[2]Pearl and Bismuth
                new string[]{"", "", ""}, //[3]Garnet and Crazy-Lace
                new string[]{"", "", ""}, //[4]Bismuth and Snowflake
                new string[]{"", "", ""}, //[5]Snowflake and Larimar
                new string[]{"", "", ""}, //[6]Ruby and Turquoise
                new string[]{"", "", ""}, //[7]Heaven Beetle and Earth Beetle
                new string[]{"", "", ""}, //[8]Aquamarine and Bloodstone
                new string[]{"", "", ""}, //[9]Biggs and Ocean
                new string[]{"", "", ""}, //[10]Turquoise and Moonstone
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
                new string[]{"", "", ""},
            };
        }
        return supportLog;
    }
    public static int[] getSupportLevels()
    {
        if (supportLevels == null)
        {
            supportLevels = new int[getSupportLog().Length];
        }
        return supportLevels;
    }
    public static int[][] getSupportRequirements()
    {
        supportRequirements = new int[][] {
                new int[]{24, 32, 40, 0}, //[0]Rose and Pearl
                new int[]{0, 0, 0, 0}, //[1]Rose and Garnet
                new int[]{0, 0, 0, 0}, //[2]Pearl and Bismuth
                new int[]{0, 0, 0, 0}, //[3]Garnet and Crazy-Lace
                new int[]{0, 0, 0, 0}, //[4]Bismuth and Snowflake
                new int[]{0, 0, 0, 0}, //[5]Snowflake and Larimar
                new int[]{0, 0, 0, 0}, //[6]Ruby and Turquoise
                new int[]{0, 0, 0, 0}, //[7]Heaven Beetle and Earth Beetle
                new int[]{0, 0, 0, 0}, //[8]Aquamarine and Bloodstone
                new int[]{0, 0, 0, 0}, //[9]Biggs and Ocean
                new int[]{0, 0, 0, 0}, //[10]Turquoise and Moonstone
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
                new int[]{0, 0, 0, 0},
        };
        return supportRequirements;
    }

    public static int getSupportLevelAtIndex(int supportIdx)
    {
        int idx = 0;
        while (getSupportRequirements()[supportIdx][idx] == -1 && idx < 4)
        {
            idx++;
        }
        return idx;
    }

    public static Unit[] findLivingSupportPartners(Unit seeker)
    {
        int id1 = seeker.supportId1;
        int id2 = seeker.supportId2;
        Unit partner1 = null;
        Unit partner2 = null;

        for (int q = 0; q < members.Count; q++)
        {
            Unit check = members[q];
            if (!check.isAlive())
            {
                continue;
            }
            if (check.supportId1 == id1 || check.supportId2 == id1)
            {
                partner1 = check;
                if (partner2 != null)
                {
                    break;
                }
            }
            if (check.supportId1 == id2 || check.supportId2 == id2)
            {
                partner2 = check;
                if (partner1 != null)
                {
                    break;
                }
            }
        }
        return new Unit[] { partner1, partner2 };
    }

    public static List<int>[] getConvoyIds()
    {
        if (convoyIds[0] == null)
        {
            for (int q = 0; q < convoyIds.Length; q++)
            {
                convoyIds[q] = new List<int>();
                convoyDurabilities[q] = new List<int>();
            }
        }
        return convoyIds;
    }
    public static List<int>[] getConvoyDurabilities()
    {
        if (convoyDurabilities[0] == null)
        {
            for (int q = 0; q < convoyIds.Length; q++)
            {
                convoyIds[q] = new List<int>();
                convoyDurabilities[q] = new List<int>();
            }
        }
        return convoyDurabilities;
    }

    public static void addToConvoy(Item item)
    {
        if (item is Weapon)
        {
            convoyIds[(int)((Weapon)item).weaponType].Add(item.id);
            convoyDurabilities[(int)((Weapon)item).weaponType].Add(item.usesLeft);
        }
        else if (item is UsableItem)
        {
            convoyIds[9].Add(item.id);
            convoyDurabilities[9].Add(item.usesLeft);
        }
    }
    public static Item takeFromConvoy(int type, int idx)
    {
        Item ret = getItems()[getConvoyIds()[type][idx]].clone();
        ret.usesLeft = getConvoyDurabilities()[type][idx];

        getConvoyIds()[type].RemoveAt(idx);
        getConvoyDurabilities()[type].RemoveAt(idx);

        return ret;
    }

    public static void registerSupportUponEscape(Unit escapee, List<Unit> player, int turn)
    {
        Unit[] sup1 = new Unit[getSupportLevels().Length];
        Unit[] sup2 = new Unit[getSupportLevels().Length];
        foreach (Unit u in player)
        {
            if (u.supportId1 > -1)
            {
                sup1[u.supportId1] = u;
            }
            if (u.supportId2 > -1)
            {
                sup2[u.supportId2] = u;
            }
        }
        if (escapee.supportId1 > -1 && sup1[escapee.supportId1] != null)
        {
            getSupportLevels()[escapee.supportId1] += turn;
        }
        if (escapee.supportId2 > -1 && sup2[escapee.supportId2] != null)
        {
            getSupportLevels()[escapee.supportId2] += turn;
        }
    }

    public static void registerRemainingSupports(List<Unit> player, int turn)
    {
        Unit[] sup1 = new Unit[getSupportLevels().Length];
        Unit[] sup2 = new Unit[getSupportLevels().Length];
        foreach (Unit u in player)
        {
            if (u.supportId1 > -1)
            {
                if (sup1[u.supportId1] == null)
                {
                    sup1[u.supportId1] = u;
                }
                else
                {
                    getSupportLevels()[u.supportId1] += turn;
                }
            }
            if (u.supportId2 > -1)
            {
                if (sup2[u.supportId2] == null)
                {
                    sup2[u.supportId2] = u;
                }
                else
                {
                    getSupportLevels()[u.supportId2] += turn;
                }
            }
        }
    }
    public static void dealWithGemstones()
    {
        foreach (Unit u in members)
        {
            if (u.heldItem is Gemstone)
            {
                Gemstone gem = (Gemstone)u.heldItem;
                gem.unit.currentHP = gem.unit.maxHP;
                gem.unit.heldWeapon = null;
                gem.unit.heldItem = null;
                if (gem.unit.team == Unit.UnitTeam.ENEMY)
                {
                    prisoners.Add(gem);
                }
                u.heldItem = null;
            }
        }
    }
    public static void refreshUnits()
    {
        foreach (Unit u in members)
        {
            if (u.isAlive() || u.isEssential)
            {
                Debug.Log(u.unitName + " " + u.currentHP);
                u.currentHP = u.maxHP;
                u.isExhausted = false;
                u.gameObject.SetActive(true);
                u.GetComponent<SpriteRenderer>().enabled = true;
                u.outline.enabled = true;
                u.outline.color = Color.blue;
//                u.outline.enabled = true;
//                u.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public static void reset()
    {
        classes = null;
        items = null;


        iron = 0;
        steel = 0;
        silver = 0;

        members = new List<Unit>();
        prisoners = new List<Gemstone>();

        supportLog = null;
        supportLevels = null;

        scene = 1;

        convoyIds = new List<int>[10];
        convoyDurabilities = new List<int>[10];

        savefile = 0;

        chapterPrep = 0;
        positions = null;

    }

    public static Transform findDeepChild(Transform parent, string childName)
    {
        LinkedList<Transform> kids = new LinkedList<Transform>();
        for (int q = 0; q < parent.childCount; q++)
        {
            kids.AddLast(parent.GetChild(q));
        }
        while (kids.Count > 0)
        {
            Transform current = kids.First.Value;
            kids.RemoveFirst();
            if (current.name == childName || current.name + "(Clone)" == childName)
            {
                return current;
            }
            for (int q = 0; q < current.childCount; q++)
            {
                kids.AddLast(current.GetChild(q));
            }
        }
        return null;
    }
}
