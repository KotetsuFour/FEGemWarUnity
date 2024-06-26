using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter1Sequence : MonoBehaviour
{

    public Tile tile;

    public Sprite sky_arena_floor;
    public Sprite sky_arena_rubble;
    public Sprite sky_arena_pillar;
    public Sprite warp_pad;

    public GameObject cam;

    public Unit unit;
    public GridMap gridmap;
    public Cutscene cutScene;
    public SaveScreen saveScreen;
    public ChapterTitle chapterTitle;
    public Sprite battleBackPicture;

    public int sequenceNum;
    public SequenceMember seqMem;

    public List<Unit> playerList;

    private int turnsTaken;

    // Start is called before the first frame update
    void Start()
    {
        ChapterTitle title = Instantiate(chapterTitle);
        title.constructor(cam.GetComponent<Camera>(), "Chapter 1 - Rebellion");
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
            sequenceNum++;
            if (sequenceNum == 1)
            {
                Cutscene firstScene = Instantiate(cutScene);
                firstScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter1_preintro.txt"), cam.GetComponent<Camera>());
                seqMem = firstScene;
            }
            else if (sequenceNum == 2)
            {
                Cutscene introScene = Instantiate(cutScene);
                introScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter1_intro.txt"), cam.GetComponent<Camera>());
                seqMem = introScene;
            }
            else if (sequenceNum == 3)
            {
                seqMem = makeChapter();
            }
            else if (sequenceNum == 4)
            {
                turnsTaken = ((GridMap)seqMem).turn;
                Cutscene finalScene = Instantiate(cutScene);
                finalScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter1_end.txt"), cam.GetComponent<Camera>());
                seqMem = finalScene;
            } else if (sequenceNum == 5)
            {
                CampaignData.dealWithGemstones();
                CampaignData.refreshUnits();
                CampaignData.registerRemainingSupports(playerList, turnsTaken);
                CampaignData.scene++;

                string bismuth_desc = "An architect given to Pink Diamond for the Earth colony";
                Unit bismuth = Instantiate(unit);
                Item bismuth_hammer = CampaignData.getItems()[4].clone();
                bismuth.constructor("Bismuth", CampaignData.getUnitClasses()[3], bismuth_desc,
                    29, 9, 0, 3, 2, 0, 11, 0, 15, 6,
                    60, 50, 5, 40, 30, 25, 30, 5,
                        bismuth_hammer, Weapon.WeaponType.ARMOR, 0, Unit.UnitTeam.ENEMY, -1, -1,
                        Unit.Affinity.ANIMA, "Bismuth.jpg");
                bismuth.isEssential = true;
                bismuth.setTalkIcon("crystal_gem_star.png");
                bismuth.GetComponent<SpriteRenderer>().size = new Vector2(Unit.spriteDimension, Unit.spriteDimension);
                CampaignData.members.Add(bismuth);

                SaveScreen save = Instantiate(saveScreen);
                save.constructor(cam.GetComponent<Camera>());
                seqMem = save;
            } else if (sequenceNum == 6)
            {
                SceneManager.LoadScene("Chapter" + CampaignData.scene);
            }
        }
    }

    private GridMap makeChapter()
    {
        UnitClass[] classes = CampaignData.getUnitClasses();
        Item[] items = CampaignData.getItems();
        Sprite[] tileSprites = { sky_arena_floor, sky_arena_rubble, sky_arena_pillar, warp_pad };
        string[] teamNames = { "Rose's Rebels", "Homeworld", "", "" };
        Dictionary<string, int> dict = new Dictionary<string, int>();
        dict.Add("b", 2);
        Debug.Log(dict["b"]);

        //Based on Fergus (FE5)
        //10% STR growth buff
        //1 point DEF buff
        string rose_desc = "The leader of the Crystal Gem rebellion";
        Unit rose_quartz = Instantiate(unit);
        Item rose_shield = items[0].clone();
        rose_quartz.constructor("Rose Quartz", classes[0], rose_desc,
                26, 6, 5, 7, 7, 6, 6, 5, 8, 6,
                65, 45, 10, 45, 35, 40, 25, 10,
                rose_shield, Weapon.WeaponType.SWORD, 0, Unit.UnitTeam.PLAYER, 0, 1,
                Unit.Affinity.EARTH, "Rose Quartz.jpg");
        rose_quartz.isEssential = true;
        rose_quartz.isLeader = true;
        rose_quartz.setTalkIcon("crystal_gem_star.png");
        CampaignData.members.Add(rose_quartz);

        //Based on Machua (FE5)
        //2 point SPD nerf
        //1 point DEF buff
        string pearl_desc = "Rose Quartz's loyal companion";
        Unit pearl = Instantiate(unit);
        Item pearl_spear = items[1].clone();
        pearl.constructor("Pearl", classes[1], pearl_desc,
                22, 4, 1, 10, 9, 6, 5, 1, 6, 6,
                60, 30, 10, 55, 60, 35, 25, 10,
                pearl_spear, Weapon.WeaponType.SWORD, 0, Unit.UnitTeam.PLAYER, 0, 2,
                Unit.Affinity.WATER, "Pearl.jpg");
        pearl.isEssential = true;
        pearl.setTalkIcon("crystal_gem_star.png");
        CampaignData.members.Add(pearl);

        //Based on Halvan (FE5)
        string biggs_desc = "A soldier fighting in the war";
        Unit biggs = Instantiate(unit);
        Item biggs_whip = items[2].clone();
        biggs.constructor("Biggs", classes[2], biggs_desc,
                28, 7, 0, 7, 7, 2, 5, 0, 12, 6,
                80, 40, 5, 20, 30, 30, 30, 5,
                biggs_whip, Weapon.WeaponType.CLUB, 20, Unit.UnitTeam.ENEMY, 9, -1,
                Unit.Affinity.FIRE, "Biggs.jpg");
        biggs.setTalkConvo(new DialogueEvent(0, "Assets/Dialogue/chapter1_biggs.txt"), true, null);
        biggs.setTalkIcon("rose_talk_icon.jpg");
        biggs.ai1 = Unit.AIType.ATTACK; biggs.ai2 = Unit.AIType.GUARD;

        //Based on Othin (FE5)
        string ocean_desc = "A soldier fighting in the war";
        Unit ocean = Instantiate(unit);
        Item ocean_club = items[3].clone();
        ocean.constructor("Ocean", classes[2], ocean_desc,
                27, 6, 0, 7, 9, 3, 4, 0, 11, 6,
                85, 30, 5, 25, 35, 55, 25, 5,
                ocean_club, Weapon.WeaponType.WHIP, 20, Unit.UnitTeam.ENEMY, 9, -1,
                Unit.Affinity.WATER, "Ocean.jpg");
        ocean.setTalkConvo(new DialogueEvent(0, "Assets/Dialogue/chapter1_ocean.txt"), false, null);
        ocean.setTalkIcon("talk_icon.jpg");
        ocean.ai1 = Unit.AIType.ATTACK; ocean.ai2 = Unit.AIType.GUARD;

        //Based on Dashin (FE5)
        //5% SPD growth buff and 10% DEF growth buff
        string bismuth_desc = "An architect given to Pink Diamond for the Earth colony";
        Unit bismuth = Instantiate(unit);
        Item bismuth_hammer = items[4].clone();
        Item iron_sword = items[5].clone();
        bismuth.constructor("Bismuth", classes[3], bismuth_desc,
                29, 9, 0, 3, 2, 0, 11, 0, 15, 6,
                60, 50, 5, 40, 30, 25, 30, 5,
                bismuth_hammer, Weapon.WeaponType.ARMOR, 0, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.ANIMA, "Bismuth.jpg");
        bismuth.isEssential = true;
        bismuth.setTalkConvo(new DialogueEvent(0, "Assets/Dialogue/chapter1_bismuth.txt"), true, iron_sword);
        bismuth.setTalkIcon("rose_talk_icon.jpg");
        bismuth.ai1 = Unit.AIType.IDLE; bismuth.ai2 = Unit.AIType.IDLE;

        Unit[] player = { rose_quartz, pearl };
        Unit[] enemy = {bismuth, genericQuartz(classes[2]), genericQuartz(classes[2]),
            genericQuartz(classes[2]), genericQuartz(classes[2]), genericQuartz(classes[2]),
            genericQuartz(classes[2]), biggs, ocean, genericQuartz(classes[2])};
        Unit[] ally = { };
        Unit[] other = { };
        int[] loot = { };

        GridMap map = Instantiate(gridmap);
        map.constructor("Assets/Map/chapter1.txt", "Assets/Map/chapter1positions.txt", "Assets/Map/chapter1loot.txt",
            tile, tileSprites, player, enemy, ally, other, cam, battleBackPicture,
            new EscapeObjective(), "Chapter 1 - Rebellion", teamNames, loot, 10, unit);
        playerList = map.player;

        return map;
    }

    private Unit genericQuartz(UnitClass soldier)
    {

        //2 point STR nerf
        string quartz_desc = "Soldier serving the diamonds against the rebellion";
        Weapon wep = (Weapon)CampaignData.getItems()[6].clone();
        Unit quartz = Instantiate(unit);
        quartz.constructor("Quartz", soldier, quartz_desc,
                24, 6, 0, 3, 6, 2, 4, 0, 9, 6,
                80, 40, 5, 20, 30, 30, 30, 5,
                wep, Weapon.WeaponType.FIST, 20, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.FIRE, "Quartz_Silhouette.jpg");
        quartz.ai1 = Unit.AIType.ATTACK; quartz.ai2 = Unit.AIType.GUARD;
        return quartz;
    }
}
