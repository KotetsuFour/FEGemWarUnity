using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Sequence : MonoBehaviour
{
    public Tile tile;

    public Sprite floor;
    public Sprite grass;
    public Sprite tree;
    public Sprite mountain;
    public Sprite peak;
    public Sprite cave;
    public Sprite house;
    public Sprite cliff;
    public Sprite heal_tile;

    private Sprite[] tileSprites;


    public GameObject cam;

    public Unit unit;
    public PreBattleMenu preparations;
    public GridMap gridmap;
    public Cutscene cutScene;
    public SaveScreen saveScreen;
    public ChapterTitle chapterTitle;
    public Sprite battleBackPicture;

    public int sequenceNum;
    public SequenceMember seqMem;

    public List<Unit> playerList;
    private int turnsTaken;

    public Unit[] player;
    public Unit[] enemy;
    public Unit[] ally;
    public Unit[] other;


    // Start is called before the first frame update
    void Start()
    {
        ChapterTitle title = Instantiate(chapterTitle);
        title.constructor(cam.GetComponent<Camera>(), "Chapter 3 - The Answer");
        seqMem = title;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            seqMem.LEFT_MOUSE(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            seqMem.RIGHT_MOUSE(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
        {
            seqMem.UP();
        }
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            seqMem.LEFT();
        }
        if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
        {
            seqMem.DOWN();

        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            seqMem.RIGHT();

        }
        if (Input.GetKeyDown("z") || Input.GetKeyDown("p"))
        {
            seqMem.Z();

        }
        if (Input.GetKeyDown("x") || Input.GetKeyDown("o"))
        {
            seqMem.X();
        }
        if (Input.GetKeyDown("c") || Input.GetKeyDown("i"))
        {
            seqMem.C();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            seqMem.ENTER();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            seqMem.ESCAPE();
        }
        if (seqMem.completed())
        {
            Debug.Log("done with " + seqMem);
            sequenceNum++;
            if (sequenceNum == 1)
            {
                Cutscene introScene = Instantiate(cutScene);
                introScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter3_intro.txt"), cam.GetComponent<Camera>());
                seqMem = introScene;
            }
            else if (sequenceNum == 2)
            {
                SaveMechanism.loadGame(CampaignData.savefile, unit);
                if (CampaignData.chapterPrep != CampaignData.scene)
                {
                    CampaignData.members.Add(nephrite());

                    CampaignData.addToConvoy(CampaignData.getItems()[18].clone());
                    CampaignData.addToConvoy(CampaignData.getItems()[18].clone());
                }

                //TODO everything to do
                int[] playerPos = { -1, -1, -1, -1, -1, 0, -1, -1, -1 };
                if (CampaignData.chapterPrep == CampaignData.scene)
                {
                    playerPos = CampaignData.positions;
                }
                tileSprites = new Sprite[] {floor, //FLOOR
                    null, //RUBBLE
                    null, //PILLAR
                    null, //WARP PAD
                    null, //DEEP WATER
                    null, //WALL
                    null, //CHEST
                    null, //SEIZE POINT
                    heal_tile, //HEAL TILE
                    grass, //PLAIN
                    tree, //TREE
                    peak, //PEAK
                    mountain, //MOUNTAIN
                    cave, //CAVE
                    house, //HOUSE
                    cliff //CLIFF
                };
                enemy = new Unit[]{genericTopazFusion(), genericPriestess(), genericQuartz(), genericGuard(),
                    genericPriestess(), genericElite(), genericGuard(), genericGuard(), genericQuartz(),
                    genericQuartz(), genericQuartz(), genericQuartz(), genericGuard(), genericGuard(),
                    genericGuard(), genericGuard(), genericGuard(), genericQuartz(), genericQuartz(),
                    genericQuartz()};
                ally = new Unit[] { citrine(), flint(), aventurine(), chert() };
                other = new Unit[] { };

                PreBattleMenu pbm = Instantiate(preparations);
                pbm.constructor("Assets/Map/chapter3.txt", "Assets/Map/chapter3positions.txt",
                    tile, tileSprites,
                    playerPos, enemy, ally, other, cam,
                    new RoutObjective(), "Chapter 3 - The Answer", unit);

                seqMem = pbm;
            }
            else if (sequenceNum == 3)
            {
                List<int> pos = ((PreBattleMenu)seqMem).player;
                player = new Unit[pos.Count];
                for (int q = 0; q < pos.Count; q++)
                {
                    if (pos[q] == -1)
                    {
                        player[q] = null;
                    }
                    else
                    {
                        player[q] = CampaignData.members[pos[q]];
                    }
                }
                seqMem = makeChapter();
            }
            else if (sequenceNum == 4)
            {
                turnsTaken = ((GridMap)seqMem).turn;
                Cutscene finalScene = Instantiate(cutScene);
                finalScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter3_end.txt"), cam.GetComponent<Camera>());
                seqMem = finalScene;
            }
            else if (sequenceNum == 5)
            {
                CampaignData.dealWithGemstones();
                CampaignData.refreshUnits();
                CampaignData.registerRemainingSupports(playerList, turnsTaken);
                CampaignData.scene++;
                SaveScreen save = Instantiate(saveScreen);
                save.constructor(cam.GetComponent<Camera>());
                seqMem = save;
            }
            else if (sequenceNum == 6)
            {
                //                SceneManager.LoadScene("Chapter" + CampaignData.scene);
                SpecialMenuLogic.mainMenu(unit);
            }
        }
    }

    private GridMap makeChapter()
    {
        UnitClass[] classes = CampaignData.getUnitClasses();
        Item[] items = CampaignData.getItems();

        int[] loot = { };

        string[] teamNames = { "Crystal Gems", "Homeworld", "Refraction Stones", "" };

        GridMap map = Instantiate(gridmap);
        map.constructor("Assets/Map/chapter3.txt", "Assets/Map/chapter3positions.txt", "Assets/Map/chapter3loot.txt",
            tile, tileSprites, player, enemy, ally, other, cam, battleBackPicture,
            new RoutObjective(), "Chapter 3 - The Answer", teamNames, loot, 15, unit);
        playerList = map.player;

        return map;
    }

    private Unit genericQuartz()
    {
        string quartz_desc = "Soldier serving the diamonds against the rebellion";
        Weapon wep = (Weapon)CampaignData.getItems()[6].clone();
        Unit quartz = Instantiate(unit);
        UnitClass soldier = CampaignData.getUnitClasses()[2];
        quartz.constructor("Quartz", soldier, quartz_desc,
                24, 8, 0, 3, 6, 2, 4, 0, 9, 6,
                80, 40, 5, 20, 30, 30, 30, 5,
                wep, Weapon.WeaponType.FIST, 20, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.FIRE, "Quartz_Silhouette.jpg");
        quartz.ai1 = Unit.AIType.ATTACK; quartz.ai2 = Unit.AIType.GUARD;
        return quartz;
    }
    private Unit genericPriestess()
    {
        //Based on FE3 Gordon
        string priest_desc = "A priestess in the service of the Moon Goddess";
        Weapon wep = (Weapon)CampaignData.getItems()[10].clone();
        Unit priest = Instantiate(unit);
        UnitClass priestClass = CampaignData.getUnitClasses()[6];
        priest.constructor("Priestess", priestClass, priest_desc,
                18, 5, 0, 5, 4, 4, 6, 0, 5, 6,
                40, 30, 3, 30, 30, 40, 10, 3,
                wep, Weapon.WeaponType.SWORD, 0, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.LIGHT, "Priestess.jpg");
        priest.ai1 = Unit.AIType.ATTACK; priest.ai2 = Unit.AIType.GUARD;
        return priest;
    }
    private Unit nephrite()
    {
        //Based on FE5 Karin
        string neph_desc = "A rebellious pilot from Yellow Diamond's navy";
        Weapon wep = (Weapon)CampaignData.getItems()[28].clone();
        Unit neph = Instantiate(unit);
        UnitClass pilot = CampaignData.getUnitClasses()[7];
        neph.constructor("Nephrite", pilot, neph_desc,
                18, 4, 7, 4, 14, 12, 4, 7, 4, 8,
                55, 30, 15, 35, 70, 70, 15, 15,
                wep, Weapon.WeaponType.LANCE, 10, Unit.UnitTeam.PLAYER, -1, -1,
                Unit.Affinity.DARK, "nephrite.jpg");
        neph.level = 10;
        neph.heldItem = CampaignData.getItems()[18].clone();
        return neph;
    }
    private Unit citrine()
    {
        //Based on FE3 Oguma
        //1 point MOV buff
        string citr_desc = "A rebel soldier and Aventurine's soulmate";
        Weapon wep = (Weapon)CampaignData.getItems()[30].clone();
        Unit citr = Instantiate(unit);
        UnitClass eliteClass = CampaignData.getUnitClasses()[8];
        citr.constructor("Citrine", eliteClass, citr_desc,
                22, 6, 0, 11, 12, 3, 6, 0, 6, 7,
                80, 40, 3, 20, 30, 40, 30, 3,
                wep, Weapon.WeaponType.LANCE, 30, Unit.UnitTeam.ALLY, -1, -1,
                Unit.Affinity.EARTH, "citrine.jpg");
        citr.ai1 = Unit.AIType.ATTACK; citr.ai2 = Unit.AIType.PURSUE;
        return citr;
    }
    private Unit aventurine()
    {
        //Based on FE3 Navarre
        string aven_desc = "A rebel soldier and Citrine's soulmate";
        Weapon wep = (Weapon)CampaignData.getItems()[31].clone();
        Unit aven = Instantiate(unit);
        UnitClass soldier = CampaignData.getUnitClasses()[2];
        aven.constructor("Aventurine", soldier, aven_desc,
                19, 5, 0, 9, 11, 8, 6, 0, 8, 6,
                90, 50, 3, 40, 50, 60, 20, 3,
                wep, Weapon.WeaponType.LANCE, 20, Unit.UnitTeam.ALLY, -1, -1,
                Unit.Affinity.WIND, "aventurine.png");
        aven.ai1 = Unit.AIType.ATTACK; aven.ai2 = Unit.AIType.PURSUE;
        return aven;
    }
    private Unit chert()
    {
        //Based on FE3 Oguma (Book 2)
        string chert_desc = "A rebel soldier and Flint's best friend";
        Weapon wep = (Weapon)CampaignData.getItems()[32].clone();
        Unit chert = Instantiate(unit);
        UnitClass soldier = CampaignData.getUnitClasses()[2];
        chert.constructor("Chert", soldier, chert_desc,
                25, 8, 0, 14, 14, 5, 8, 0, 8, 6,
                80, 40, 3, 30, 30, 40, 30, 3,
                wep, Weapon.WeaponType.SPECIAL, 0, Unit.UnitTeam.ALLY, -1, -1,
                Unit.Affinity.ANIMA, "chert.png");
        chert.ai1 = Unit.AIType.IDLE; chert.ai2 = Unit.AIType.IDLE;
        return chert;
    }
    private Unit flint()
    {
        //Based on FE3 Navarre (Book 2)
        string flint_desc = "A rebel soldier and Chert's best friend";
        Weapon wep = (Weapon)CampaignData.getItems()[32].clone();
        Unit flint = Instantiate(unit);
        UnitClass soldier = CampaignData.getUnitClasses()[2];
        flint.constructor("Flint", soldier, flint_desc,
                23, 9, 0, 16, 16, 7, 10, 0, 8, 6,
                90, 50, 3, 40, 50, 60, 20, 3,
                wep, Weapon.WeaponType.CLUB, 0, Unit.UnitTeam.ALLY, -1, -1,
                Unit.Affinity.ANIMA, "flint.png");
        flint.ai1 = Unit.AIType.IDLE; flint.ai2 = Unit.AIType.IDLE;
        return flint;
    }
    private Unit genericElite()
    {
        //Based on FE3 Oguma and Hardin (best parts of both)
        //1 point MOV buff
        string elite_desc = "A specialized officer in the Diamonds' armies";
        Weapon wep = (Weapon)CampaignData.getItems()[29].clone();
        Unit elite = Instantiate(unit);
        UnitClass eliteClass = CampaignData.getUnitClasses()[8];
        elite.constructor("Elite Quartz", eliteClass, elite_desc,
                22, 6, 0, 11, 12, 8, 6, 0, 9, 7,
                90, 50, 3, 40, 50, 60, 30, 3,
                wep, Weapon.WeaponType.AXE, 30, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.EARTH, "Citrine_Silhouette.jpg");
        elite.ai1 = Unit.AIType.ATTACK; elite.ai2 = Unit.AIType.ATTACK;
        return elite;
    }
    private Unit genericTopazFusion()
    {
        //Based on FE5 Xavier
        string topaz_desc = "A powerful soldier, fused for extra strength";
        Weapon wep = (Weapon)CampaignData.getItems()[33].clone();
        Unit topaz = Instantiate(unit);
        UnitClass topazFusion = CampaignData.getUnitClasses()[9];
        topaz.constructor("Officer", topazFusion, topaz_desc,
                38, 13, 3, 9, 6, 3, 13, 3, 15, 6,
                50, 40, 45, 55, 50, 70, 15, 45,
                wep, Weapon.WeaponType.CLUB, 40, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.LIGHTNING, "Topaz_Silhouette.png");
        topaz.ai1 = Unit.AIType.GUARD; topaz.ai2 = Unit.AIType.GUARD;
        topaz.movement = 0;
        return topaz;
    }
    private Unit genericGuard()
    {
        //Based on FE3 Julian
        string ruby_desc = "A common guard in the service of the Diamonds";
        Weapon wep = (Weapon)CampaignData.getItems()[34].clone();
        Unit ruby = Instantiate(unit);
        UnitClass guard = CampaignData.getUnitClasses()[5];
        ruby.constructor("Ruby Guard", guard, ruby_desc,
                17, 4, 0, 6, 7, 7, 4, 0, 5, 5,
                50, 40, 45, 55, 50, 70, 15, 45,
                wep, Weapon.WeaponType.CLUB, 10, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.FIRE, "Ruby_Silhouette.png");
        ruby.ai1 = Unit.AIType.ATTACK; ruby.ai2 = Unit.AIType.ATTACK;
        return ruby;
    }
}
