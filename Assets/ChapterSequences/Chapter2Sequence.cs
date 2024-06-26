using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter2Sequence : MonoBehaviour
{
    public Tile tile;

    public Sprite lunar_sea_spire_floor;
    public Sprite sky_arena_rubble;
    public Sprite lunar_sea_spire_pillar;
    public Sprite warp_pad;
    public Sprite deep_water;
    public Sprite lunar_sea_spire_wall;
    public Sprite lunar_sea_spire_chest;
    public Sprite lunar_sea_spire_throne;
    public Sprite heal_tile;


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
        Cutscene firstScene = Instantiate(cutScene);
        firstScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter2_preintro.txt"), cam.GetComponent<Camera>());
        seqMem = firstScene;
        Debug.Log("seqMem set to " + seqMem);
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
                ChapterTitle title = Instantiate(chapterTitle);
                title.constructor(cam.GetComponent<Camera>(), "Chapter 2 - Sea of Troubles");
                seqMem = title;
            }
            else if (sequenceNum == 2)
            {
                Cutscene introScene = Instantiate(cutScene);
                introScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter2_intro.txt"), cam.GetComponent<Camera>());
                seqMem = introScene;
            }
            else if (sequenceNum == 3) {
                SaveMechanism.loadGame(CampaignData.savefile, unit);
                if (CampaignData.chapterPrep != CampaignData.scene) {
                    //Based on FE5 Lifis
                    string ruby_desc = "A stalwart guardian for Turquoise";
                    Unit ruby = Instantiate(unit);
                    Item ruby_pike = CampaignData.getItems()[8].clone();
                    ruby.constructor("Ruby", CampaignData.getUnitClasses()[5], ruby_desc,
                            20, 3, 0, 4, 10, 1, 2, 0, 6, 5,
                            65, 35, 10, 25, 45, 5, 15, 10,
                            ruby_pike, Weapon.WeaponType.WHIP, 0, Unit.UnitTeam.PLAYER, 6, -1,
                            Unit.Affinity.FIRE, "ruby.png");
                    CampaignData.members.Add(ruby);

                    CampaignData.addToConvoy(CampaignData.getItems()[35].clone());
                }

                //TODO everything to do
                int[] playerPos = {0, -1, -1, -1, -1, -1};
                if (CampaignData.chapterPrep == CampaignData.scene)
                {
                    playerPos = CampaignData.positions;
                }
                Sprite[] tileSprites = {lunar_sea_spire_floor, null, lunar_sea_spire_pillar, null, deep_water,
                    lunar_sea_spire_wall, lunar_sea_spire_chest, lunar_sea_spire_throne, heal_tile};
                UnitClass[] classes = CampaignData.getUnitClasses();
                enemy = new Unit[]{genericNoble(classes[4]), genericPriestess(classes[6]), genericPriestess(classes[6]),
                    genericPriestess(classes[6]), moonstone(), turquoise(), genericNoble(classes[4]), genericNoble(classes[4]),
                    genericNoble(classes[4]), genericNoble(classes[4]), genericNoble(classes[4]),
                    genericNoble(classes[4]), genericPriestess(classes[6]), genericPriestess(classes[6]),
                    genericPriestess(classes[6]), genericPriestess(classes[6])};
                ally = new Unit[]{ };
                other = new Unit[]{ };

                PreBattleMenu pbm = Instantiate(preparations);
                pbm.constructor("Assets/Map/chapter2.txt", "Assets/Map/chapter2positions.txt",
                    tile, tileSprites,
                    playerPos, enemy, ally, other, cam,
                    new SeizeObjective(), "Chapter 2 - Sea of Troubles", unit);

                seqMem = pbm;
            }
            else if (sequenceNum == 4)
            {
                //                SaveMechanism.loadGame(CampaignData.savefile, unit); //We load before Battle Prep
                List<int> pos = ((PreBattleMenu)seqMem).player;
                player = new Unit[pos.Count];
                for (int q = 0; q < pos.Count; q++)
                {
                    if (pos[q] == -1)
                    {
                        player[q] = null;
                    } else
                    {
                        player[q] = CampaignData.members[pos[q]];
                    }
                }
                seqMem = makeChapter();
            }
            else if (sequenceNum == 5)
            {
                turnsTaken = ((GridMap)seqMem).turn;
                Cutscene finalScene = Instantiate(cutScene);
                finalScene.constructor(new DialogueEvent(0, "Assets/Dialogue/chapter2_end.txt"), cam.GetComponent<Camera>());
                seqMem = finalScene;
            }
            else if (sequenceNum == 6)
            {
                CampaignData.dealWithGemstones();
                CampaignData.refreshUnits();
                CampaignData.registerRemainingSupports(playerList, turnsTaken);
                CampaignData.scene++;
                SaveScreen save = Instantiate(saveScreen);
                save.constructor(cam.GetComponent<Camera>());
                seqMem = save;
            }
            else if (sequenceNum == 7)
            {
                SceneManager.LoadScene("Chapter" + CampaignData.scene);
            }
        }
    }

    private GridMap makeChapter()
    {
        UnitClass[] classes = CampaignData.getUnitClasses();

        Sprite[] tileSprites = {lunar_sea_spire_floor, null, lunar_sea_spire_pillar, null, deep_water,
        lunar_sea_spire_wall, lunar_sea_spire_chest, lunar_sea_spire_throne, heal_tile};

        int[] loot = {0, 2, 0, -1,/*2 Steel*/0, 0, 0, 11,/*Iron Lance*/
            0, 0, 0, 24,/*Moon Goddess Icon*/0, 0, 0, 15,/*Iron Whip*/4, 0, 0, -1/*4 Iron*/};

        string[] teamNames = {"Crystal Gems", "Homeworld", "", ""};

        GridMap map = Instantiate(gridmap);
        map.constructor("Assets/Map/chapter2.txt", "Assets/Map/chapter2positions.txt", "Assets/Map/chapter2loot.txt",
            tile, tileSprites, player, enemy, ally, other, cam, battleBackPicture,
            new SeizeObjective(), "Chapter 2 - Name Pending", teamNames, loot, 20, unit);
        playerList = map.player;

        return map;
    }

    private Unit genericQuartz(UnitClass soldier)
    {
        string quartz_desc = "Soldier serving the diamonds against the rebellion";
        Weapon wep = (Weapon)CampaignData.getItems()[6].clone();
        Unit quartz = Instantiate(unit);
        quartz.constructor("Quartz", soldier, quartz_desc,
                24, 8, 0, 3, 6, 2, 4, 0, 9, 6,
                80, 40, 5, 20, 30, 30, 30, 5,
                wep, Weapon.WeaponType.FIST, 0, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.FIRE, "Quartz_Silhouette.jpg");
        quartz.ai1 = Unit.AIType.ATTACK; quartz.ai2 = Unit.AIType.GUARD;
        return quartz;
    }
    private Unit genericNoble(UnitClass nobleClass)
    {
        //Based on FE3 Merric
        string noble_desc = "A noble in the court of a Diamond";
        Weapon wep = (Weapon)CampaignData.getItems()[7].clone();
        Unit noble = Instantiate(unit);
        noble.constructor("Noble", nobleClass, noble_desc,
                20, 0, 6, 3, 6, 3, 4, 3, 5, 6,
                80, 5, 20, 30, 50, 50, 20, 3,
                wep, Weapon.WeaponType.SWORD, 0, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.ICE, "Noble.jpg");
        noble.ai1 = Unit.AIType.GUARD; noble.ai2 = Unit.AIType.GUARD;
        noble.movement = 0;
        return noble;
    }

    private Unit genericPriestess(UnitClass priestClass)
    {
        //Based on FE3 Gordon
        string priest_desc = "A priestess in the service of the Moon Goddess";
        Weapon wep = (Weapon)CampaignData.getItems()[10].clone();
        Unit priest = Instantiate(unit);
        priest.constructor("Priestess", priestClass, priest_desc,
                18, 5, 0, 5, 4, 4, 6, 0, 5, 6,
                40, 30, 3, 30, 30, 40, 10, 3,
                wep, Weapon.WeaponType.SWORD, 0, Unit.UnitTeam.ENEMY, -1, -1,
                Unit.Affinity.LIGHT, "Priestess.jpg");
        priest.ai1 = Unit.AIType.ATTACK; priest.ai2 = Unit.AIType.GUARD;
        return priest;
    }

    private Unit moonstone()
    {
        //Based on FE5 Tanya
        string moon_desc = "A priestess in the service of the Moon Goddess";
        Unit moon = Instantiate(unit);
        Item moon_bow = CampaignData.getItems()[9].clone();
        moon.constructor("Moonstone", CampaignData.getUnitClasses()[6], moon_desc,
                20, 3, 1, 6, 10, 6, 2, 1, 4, 6,
                60, 35, 15, 55, 70, 60, 15, 15,
                moon_bow, Weapon.WeaponType.LANCE, 0, Unit.UnitTeam.ENEMY, 10, -1,
                Unit.Affinity.LIGHT, "Moonstone.png");
        moon.setTalkConvo(new DialogueEvent(0, "Assets/Dialogue/chapter2_moonstone.txt"), true, null);
        moon.setTalkIcon("rose_talk_icon.jpg");
        moon.ai1 = Unit.AIType.ATTACK; moon.ai2 = Unit.AIType.GUARD;
        return moon;
    }

    private Unit turquoise()
    {
        //Based on FE5 Asvel
        string turq_desc = "An emissary from Blue Diamond's court";
        Unit turq = Instantiate(unit);
        Item turq_laser = CampaignData.getItems()[7].clone();
        turq.constructor("Turquoise", CampaignData.getUnitClasses()[4], turq_desc,
                22, 0, 4, 3, 7, 5, 0, 4, 4, 6,
                55, 15, 35, 55, 75, 35, 10, 35,
                turq_laser, Weapon.WeaponType.SPECIAL, 0, Unit.UnitTeam.ENEMY, 6, 10,
                Unit.Affinity.ICE, "Turquoise.jpg");
        turq.setTalkConvo(new DialogueEvent(0, "Assets/Dialogue/chapter2_turquoise.txt"), false, null);
        turq.setTalkIcon("talk_icon.jpg");
        turq.ai1 = Unit.AIType.GUARD; turq.ai2 = Unit.AIType.GUARD;
        return turq;
    }
}
