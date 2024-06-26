using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class GridMap : SequenceMember {
    private int width;
    private int height;
    private float cellSize = 2;
    private Unit instantiatableUnit; //For loading saves
    public int cameraOrthographicSize = 6;

    public bool testingMode;

    public List<Unit> player;
    public List<Unit> enemy;
    public List<Unit> ally;
    public List<Unit> other;
    private string[] teamNames;

    private Objective objective;
    public bool seized;
    private string chapterName;
    private bool objectiveComplete;

    public int turn;
    private int turnPar;

    private Tile tile;

    private Tile[,] gridArray;
    private List<Tile> healTiles;

    private float timer;

    public GameObject cursor;
    public GameObject cam;
    public GameObject menuBackground;
    public GameObject instantiatedMenuBackground;
    public float menuSizeX;
    public TextMeshProUGUI menuOption;
    public List<TextMeshProUGUI> menuOptions;
    public List<MenuChoice> menuElements;
    public int menuIdx;
    public GameObject forecastBackground;
    public GameObject instantiatedForecastBackground;
    public float forecastSize;

    public int cursorX;
    public int cursorY;

    public SelectionMode selectionMode = SelectionMode.ROAM;

    public Dictionary<Tile, object> traversableTiles;
    public Dictionary<Tile, object> attackableTiles;
    public Tile selectedTile;
    public Unit selectedUnit;
    public Tile moveDest;
    public List<Tile> interactableUnits;
    public int interactIdx;
    public Tile targetTile;
    public Unit targetEnemy;

    public GameObject battleBackground;
    public GameObject instantiatedBattleBackground;
    public GameObject battleUI;
    public GameObject instantiatedBattleUI;
    public GameObject battleTile;
    public GameObject battleEnemyTile;
    public GameObject battlePlayerTile;
    public GameObject battleCombatant;
    public GameObject battleCombatantEnemy;
    public GameObject battleCombatantPlayer;
    public float battleMoveSpeed; //Set in UI

    private List<AttackComponent> attackParts;
    private int attackMode; //0 = attack, 1 = return
    private int attackPartIdx;
    private bool keepBattling;

    private int enemyIdx;
    private Unit currentEnemy;
    private object[] enemyAction;
    private Tile enemyStartTile;
    private Tile enemyDestTile;
    private float moveTime;

    private int allyIdx;
    private Unit currentAlly;
    private object[] allyAction;
    private Tile allyStartTile;
    private Tile allyDestTile;

    public GameObject dialogueBox;
    private GameObject instantiatedDialogueBox;
    public GameObject speakerPortrait;
    private List<GameObject> instantiatedSpeakerPortraits;
    private DialogueEvent currentDialogue;
    private Tile talkerTile;

    public GameObject statsPageBackground;
    private GameObject instantiatedStatsPageBackground;

    private Weapon brokenWeapon;
    private bool showingBrokenWeapon;
    private bool showingEXP;
    private bool showingLevelUp;
    private bool findBattleEndStart;
    private int expToAdd;
    private Unit expUnit;
    private bool[] levelGrowths;
    public GameObject breakAndExpPane;
    private GameObject instantiatedBreakAndExpPane;
    public GameObject levelUpBackground;
    private GameObject instantiatedLevelUpBackground;

    public GameObject mapHUD;
    private GameObject instantiatedMapHUD;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI tileInfoText;

    public GameObject controlsPage;
    public GameObject statusPage;

    public GameObject gameOver;
    public GameObject escapeMenu;
    private GameObject instantiatedSpecialMenu;
    private int specialMenuIdx;

    public GameObject itemNote;
    private GameObject instantiatedItemNote;

    public void constructor(string input, string positions, string lootPositions, Tile tile, Sprite[] tileSprites,
        Unit[] playerUnits, Unit[] enemyUnits, Unit[] allyUnits, Unit[] otherUnits, GameObject cam,
        Sprite battleBackgroundPic, Objective objective, string chapterName, string[] teamNames, int[] loot, int par,
        Unit instantUnit)
    {
        instantiatableUnit = instantUnit;
        this.tile = tile;
        this.player = new List<Unit>();
        this.enemy = new List<Unit>();
        this.ally = new List<Unit>();
        this.other = new List<Unit>();
        this.teamNames = teamNames;
        this.objective = objective;
        seized = false;
        turn = 1;
        this.turnPar = par;
        this.chapterName = chapterName;
        controlsPage = Instantiate(controlsPage);
        statusPage = Instantiate(statusPage);
        controlsPage.SetActive(false);
        statusPage.SetActive(false);
        instantiatedBattleBackground = Instantiate(battleBackground, new Vector3(0, 0, BATTLE_BACKGROUND_LAYER),
            Quaternion.identity);
        instantiatedBattleBackground.GetComponent<SpriteRenderer>().sprite = battleBackgroundPic;
        instantiatedBattleBackground.SetActive(false);
        instantiatedMapHUD = Instantiate(mapHUD);
        objectiveText = instantiatedMapHUD.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        tileInfoText = instantiatedMapHUD.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();

        objectiveText.text = objective.getName();
        if (objectiveText.text.Length >= 27)
        {
            objectiveText.fontSize = 15;
        }

        for (int q = 0; q < playerUnits.Length; q++)
        {
            player.Add(playerUnits[q]);
        }
        for (int q = 0; q < enemyUnits.Length; q++)
        {
            enemy.Add(enemyUnits[q]);
        }
        for (int q = 0; q < allyUnits.Length; q++)
        {
            ally.Add(allyUnits[q]);
        }
        for (int q = 0; q < otherUnits.Length; q++)
        {
            other.Add(otherUnits[q]);
        }
        this.cam = cam;

        StreamReader read = new StreamReader(input);
        List<List<char>> counterList = new List<List<char>>();
        while (read.Peek() != -1)
        {
            string s = read.ReadLine();
            int idx = counterList.Count;
            counterList.Add(new List<char>());
            for (int q = 0; q < s.Length; q++)
            {
                counterList[idx].Add(s[q]);
            }
        }
        read.Close();

        StreamReader posRead = new StreamReader(positions);
        List<List<char>> posList = new List<List<char>>();
        while (posRead.Peek() != -1)
        {
            string s = posRead.ReadLine();
            int idx = posList.Count;
            posList.Add(new List<char>());
            for (int q = 0; q < s.Length; q++)
            {
                posList[idx].Add(s[q]);
            }
        }
        posRead.Close();

        StreamReader lootRead = new StreamReader(lootPositions);
        List<List<char>> lootList = new List<List<char>>();
        while (lootRead.Peek() != -1)
        {
            string s = lootRead.ReadLine();
            int idx = lootList.Count;
            lootList.Add(new List<char>());
            for (int q = 0; q < s.Length; q++)
            {
                lootList[idx].Add(s[q]);
            }
        }
        lootRead.Close();

        if (posList.Count != counterList.Count || posList[0].Count != counterList[0].Count)
        {
            Debug.Log("Tile and positions maps are not the same size");
            return;
        }

        cursor = Instantiate(cursor);
        cursor.GetComponent<SpriteRenderer>().size = new Vector2(cellSize, cellSize);
        cursor.SetActive(true);

        this.width = counterList[0].Count;
        this.height = counterList.Count;
        gridArray = new Tile[width, height];
        healTiles = new List<Tile>();
        int playerIdx = 0;
        int enemyIdx = 0;
        int allyIdx = 0;
        int otherIdx = 0;
        int lootIdx = 0;
        for (int q = 0; q < counterList.Count; q++)
        {
            for (int w = 0; w < counterList[0].Count; w++)
            {
                Tile toPut = Instantiate(tile, new Vector3(w * cellSize, q * cellSize), Quaternion.identity);
                char c = counterList[q][w];
                if (c == '_')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[0];
                    toPut.setValues("FLOOR", 1, 5, 0);
                } else if (c == 'R')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[1];
                    toPut.setValues("RUBBLE", 2, 1, 0);
                }
                else if (c == '|')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[2];
                    toPut.setValues("PILLAR", 2, 6, 20);
                }
                else if (c == 'r')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[3];
                    toPut.setValues("WARP PAD", 2, 3, 0);
                } else if (c == '~')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[4];
                    toPut.setValues("DEEP WATER", int.MaxValue, 1, 0);
                }
                else if (c == 'W')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[5];
                    toPut.setValues("WALL", int.MaxValue, 1, 0);
                } else if (c == 'e')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[6];
                    toPut.setValues("CHEST", 1, 4, 0);
                } else if (c == 'T')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[7];
                    toPut.setValues("SEIZE POINT", 1, 4, 20);
                    healTiles.Add(toPut);
                } else if (c == '+')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[8];
                    toPut.setValues("HEAL TILE", 1, 1, 20);
                    healTiles.Add(toPut);
                }
                else if (c == '-')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[9];
                    toPut.setValues("PLAIN", 1, 1, 0);
                }
                else if (c == 'A')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[10];
                    toPut.setValues("TREE", 2, 1, 20);
                }
                else if (c == 'P')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[11];
                    toPut.setValues("PEAK", 7, 1, 40);
                }
                else if (c == '^')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[12];
                    toPut.setValues("MOUNTAIN", 4, 1, 30);
                }
                else if (c == 'C')
                {
                    //TODO add visit event
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[13];
                    toPut.setValues("CAVE", 1, 1, 10);
                }
                else if (c == 'H')
                {
                    //TODO add visit event
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[14];
                    toPut.setValues("HOUSE", 1, 1, 10);
                }
                else if (c == 'c')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[15];
                    toPut.setValues("CLIFF", int.MaxValue, 1, 0);
                }
                toPut.GetComponent<SpriteRenderer>().size = new Vector2(cellSize, cellSize); 

                gridArray[w, q] = toPut;
                toPut.x = w;
                toPut.y = q;

                if (posList[q][w] == '*' && playerIdx < player.Count)
                {
                    toPut.setOccupant(player[playerIdx]);
                    if (player[playerIdx] != null && player[playerIdx].isLeader)
                    {
                        cursorX = w;
                        cursorY = q;
                        cursor.transform.position = getWorldPosition(cursorX, cursorY, CURSOR_LAYER);
                    }
                    playerIdx++;
                } else if (posList[q][w] == 'x' && enemyIdx < enemy.Count)
                {
                    toPut.setOccupant(enemy[enemyIdx]);
                    enemyIdx++;
                }
                else if (posList[q][w] == 'o' && allyIdx < ally.Count)
                {
                    toPut.setOccupant(ally[allyIdx]);
                    allyIdx++;
                }
                else if (posList[q][w] == '-' && otherIdx < other.Count)
                {
                    toPut.setOccupant(other[otherIdx]);
                    otherIdx++;
                }

                if (lootList[q][w] == 'e' && lootIdx < loot.Length)
                {
                    toPut.ironLoot = loot[lootIdx];
                    toPut.steelLoot = loot[lootIdx + 1];
                    toPut.silverLoot = loot[lootIdx + 2];
                    toPut.itemLoot = loot[lootIdx + 3] == -1 ? null : CampaignData.getItems()[loot[lootIdx + 3]];
                    lootIdx += 4;
                }
            }
        }
        for (int q = 0; q < player.Count; q++)
        {
            if (player[q] == null)
            {
                player.RemoveAt(q);
                q--;
            }
        }
        tileInfoText.text = "Tile: " + gridArray[cursorX, cursorY].tileName
            + "\nAvoid Bonus: " + gridArray[cursorX, cursorY].avoidBonus;

        initializeCamera();
    }

    public void initializeCamera()
    {
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = cameraOrthographicSize;
        setCameraPosition(cursorX, cursorY);
    }

    public Vector3 getWorldPosition(int x, int y, float z)
    {
        return new Vector3(x * cellSize, y * cellSize, z);
    }

    public override void LEFT_MOUSE(float mouseX, float mouseY)
    {
        /*
        if (selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.MOVE
            )
        {
            Tile tile = getTileAtMouse(mouseX, mouseY);
            if (tile != null)
            {
                cursorX = tile.x;
                cursorY = tile.y;
                updateCursor();
                Z();
            }
        }
        //TODO the other things
        */
    }
    public override void RIGHT_MOUSE(float mouseX, float mouseY)
    {
//        X();
    }
    public override void Z()
    {
        if (selectionMode == SelectionMode.ROAM)
        {
            if (gridArray[cursorX, cursorY].getOccupant() != null)
            {
                selectedTile = gridArray[cursorX, cursorY];
                selectedUnit = selectedTile.getOccupant();
                fillTraversableTiles(selectedUnit, cursorX, cursorY);
                selectionMode = SelectionMode.MOVE;
            }
            else
            {
                instantiatedMenuBackground = Instantiate(menuBackground);

                getMapMenuOptions();
                selectionMode = SelectionMode.MAP_MENU;
            }
        }
        else if (selectionMode == SelectionMode.MOVE)
        {
            Tile currentTile = gridArray[cursorX, cursorY];
            if (selectedUnit.team == Unit.UnitTeam.PLAYER && traversableTiles.ContainsKey(currentTile) &&
                (currentTile.isVacant() || currentTile.getOccupant() == selectedUnit)
                && (!selectedUnit.isExhausted || testingMode))
            {
                moveDest = gridArray[cursorX, cursorY];
                unfillTraversableTiles();
                moveDest.moveSpriteOutline.SetActive(true);
                moveDest.moveSprite.SetActive(true);
                moveDest.moveSprite.GetComponent<SpriteRenderer>().sprite = selectedUnit.GetComponent<SpriteRenderer>().sprite;
                moveDest.moveSprite.GetComponent<SpriteRenderer>().size = new Vector2(Unit.spriteDimension, Unit.spriteDimension);

                instantiatedMenuBackground = Instantiate(menuBackground);

                getMenuOptions();

                selectionMode = SelectionMode.MENU;

            }
        }
        else if (selectionMode == SelectionMode.MENU || selectionMode == SelectionMode.MAP_MENU
            || selectionMode == SelectionMode.ITEM_MENU)
        {
            selectMenuOption(menuElements[menuIdx]);
        }
        else if (selectionMode == SelectionMode.SELECT_WEAPON)
        {
            foreach (TextMeshProUGUI opt in menuOptions)
            {
                opt.color = Color.black;
            }
            if (menuElements[menuIdx] == MenuChoice.EQUIP_PERSONAL)
            {
                selectedUnit.equip(0);
                unfillAttackableTiles();
                fillAttackableTiles();
                menuOptions[menuIdx].color = Color.green;
            }
            else if (menuElements[menuIdx] == MenuChoice.EQUIP_HELD)
            {
                selectedUnit.equip(1);
                unfillAttackableTiles();
                fillAttackableTiles();
                menuOptions[menuIdx].color = Color.green;
            } else if (menuElements[menuIdx] == MenuChoice.EQUIP_NONE) {
                selectedUnit.equip(2);
                unfillAttackableTiles();
                fillAttackableTiles();
                menuOptions[menuIdx].color = Color.green;
            }
            else
            {
                selectMenuOption(menuElements[menuIdx]);
            }
        }
        else if (selectionMode == SelectionMode.SELECT_GEM)
        {
            selectedUnit.heldItem = moveDest.gemstones[menuIdx];
            moveDest.gemstones.Remove((Gemstone)selectedUnit.heldItem);

            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;

            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.SELECT_ENEMY)
        {
            createForecast();
        }
        else if (selectionMode == SelectionMode.FORECAST)
        {
            Destroy(instantiatedForecastBackground);

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);
            performBattle(selectedUnit, targetEnemy, selectedUnit.getEquippedWeapon(),
                targetEnemy.getEquippedWeapon(), moveDest, targetTile, true);

            selectionMode = SelectionMode.BATTLE;
        }
        else if (selectionMode == SelectionMode.SELECT_TALKER)
        {
            talkerTile = interactableUnits[interactIdx];

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            unfillAttackableTiles();
            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            performTalk();

            selectionMode = SelectionMode.IN_CONVO;
        }
        else if (selectionMode == SelectionMode.IN_CONVO)
        {
            if (!nextSaying())
            {
                deconstructConversation();
                if (talkerTile.getOccupant() != null)
                {
                    talkerTile.getOccupant().talkConvo = null;
                }
                selectionMode = SelectionMode.ROAM;
            }
        } else if (selectionMode == SelectionMode.SELECT_TRADER) {
            Unit trader = interactableUnits[interactIdx].getOccupant();

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            unfillAttackableTiles();
            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Item temp = trader.heldItem;
            trader.heldItem = selectedUnit.heldItem;
            selectedUnit.heldItem = temp;

            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.SELECT_WEAPON_TRADER)
        {
            Unit trader = interactableUnits[interactIdx].getOccupant();

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            unfillAttackableTiles();
            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Weapon temp = trader.heldWeapon;
            trader.heldWeapon = selectedUnit.heldWeapon;
            selectedUnit.heldWeapon = temp;

            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.GAMEOVER || selectionMode == SelectionMode.ESCAPE_MENU)
        {
            if (specialMenuIdx == 0)
            {
                SpecialMenuLogic.restartChapter(instantiatableUnit);
            } else if (specialMenuIdx == 1)
            {
                SpecialMenuLogic.mainMenu(instantiatableUnit);
            }
        }
    }
    public override void X()
    {
        if (selectionMode == SelectionMode.MOVE)
        {
            unfillTraversableTiles();
            selectionMode = SelectionMode.ROAM;
            //                selectedUnit = null;
        }
        else if (selectionMode == SelectionMode.MENU)
        {
            unfillAttackableTiles();

            fillTraversableTiles(selectedUnit, selectedTile.x, selectedTile.y);
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            selectionMode = SelectionMode.MOVE;
        }
        else if (selectionMode == SelectionMode.SELECT_ENEMY || selectionMode == SelectionMode.SELECT_TALKER
            || selectionMode == SelectionMode.SELECT_TRADER || selectionMode == SelectionMode.SELECT_WEAPON_TRADER)
        {
            unfillAttackableTiles();

            instantiatedMenuBackground = Instantiate(menuBackground);

            getMenuOptions();

            selectionMode = SelectionMode.MENU;
        }
        else if (selectionMode == SelectionMode.SELECT_WEAPON || selectionMode == SelectionMode.SELECT_GEM
            || selectionMode == SelectionMode.ITEM_MENU)
        {
            //TODO
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            menuOptions.Clear();

            getMenuOptions();

            selectionMode = SelectionMode.MENU;
        }
        else if (selectionMode == SelectionMode.FORECAST)
        {
            Destroy(instantiatedForecastBackground);

            Dictionary<Tile, object> attackable = getAttackableBattlegroundTilesFromDestination(selectedUnit, moveDest);
            interactableUnits = getAttackableTilesWithEnemies(attackable, selectedUnit);
            foreach (Tile t in interactableUnits)
            {
                t.attackHighlight.SetActive(true);
            }
            interactIdx = 0;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();

            selectionMode = SelectionMode.SELECT_ENEMY;
        }
        else if (selectionMode == SelectionMode.MAP_MENU)
        {
            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.STATS_PAGE)
        {
            deconstructStatsPage();

            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.CONTROLS)
        {
            controlsPage.SetActive(false);
            Camera camCam = cam.GetComponent<Camera>();
            camCam.orthographicSize = cameraOrthographicSize;
            setCameraPosition(cursorX, cursorY);
            instantiatedMapHUD.SetActive(true);

            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.STATUS)
        {
            statusPage.SetActive(false);
            Camera camCam = cam.GetComponent<Camera>();
            camCam.orthographicSize = cameraOrthographicSize;
            setCameraPosition(cursorX, cursorY);
            instantiatedMapHUD.SetActive(true);

            selectionMode = SelectionMode.ROAM;
        } else if (selectionMode == SelectionMode.ESCAPE_MENU)
        {
            Destroy(instantiatedSpecialMenu);
            instantiatedMapHUD.SetActive(true);
            selectionMode = SelectionMode.ROAM;
        }
    }
    public override void C()
    {
        if (selectionMode == SelectionMode.ROAM && gridArray[cursorX, cursorY].getOccupant() != null)
        {
            selectedTile = gridArray[cursorX, cursorY];
            selectedUnit = selectedTile.getOccupant();
            constructStatsPage();

            selectionMode = SelectionMode.STATS_PAGE;
        }
    }
    public override void UP()
    {
        if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.MOVE)
    && cursorY != height - 1)
        {
            cursorY++;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.MENU || selectionMode == SelectionMode.SELECT_WEAPON
          || selectionMode == SelectionMode.MAP_MENU || selectionMode == SelectionMode.SELECT_GEM
          || selectionMode == SelectionMode.ITEM_MENU)
        {
            menuOptions[menuIdx].color = Color.black;
            menuIdx--;
            if (menuIdx < 0)
            {
                menuIdx = menuOptions.Count - 1;
            }
            menuOptions[menuIdx].color = Color.white;
        }
        else if (selectionMode == SelectionMode.SELECT_ENEMY || selectionMode == SelectionMode.SELECT_TALKER
            || selectionMode == SelectionMode.SELECT_TRADER || selectionMode == SelectionMode.SELECT_WEAPON_TRADER)
        {
            interactIdx = (interactIdx + 1) % interactableUnits.Count;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();
        } else if (selectionMode == SelectionMode.GAMEOVER || selectionMode == SelectionMode.ESCAPE_MENU)
        {
            instantiatedSpecialMenu.transform.GetChild(0).GetChild(specialMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            specialMenuIdx--;
            if (specialMenuIdx < 0)
            {
                specialMenuIdx = instantiatedSpecialMenu.transform.GetChild(0).childCount - 1;
            }
            instantiatedSpecialMenu.transform.GetChild(0).GetChild(specialMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.cyan;
        }
    }
    public override void LEFT()
    {
        if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.MOVE)
    && cursorX != 0)
        {
            cursorX--;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.SELECT_ENEMY || selectionMode == SelectionMode.SELECT_TALKER
            || selectionMode == SelectionMode.SELECT_TRADER || selectionMode == SelectionMode.SELECT_WEAPON_TRADER)
        {
            interactIdx--;
            if (interactIdx < 0)
            {
                interactIdx = interactableUnits.Count - 1;
            }
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();
        }
    }
    public override void DOWN()
    {
        if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.MOVE)
    && cursorY != 0)
        {
            cursorY--;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.MENU || selectionMode == SelectionMode.SELECT_WEAPON
            || selectionMode == SelectionMode.MAP_MENU || selectionMode == SelectionMode.SELECT_GEM
            || selectionMode == SelectionMode.ITEM_MENU)
        {
            menuOptions[menuIdx].color = Color.black;
            menuIdx = (menuIdx + 1) % menuOptions.Count;
            menuOptions[menuIdx].color = Color.white;
        }
        else if (selectionMode == SelectionMode.SELECT_ENEMY || selectionMode == SelectionMode.SELECT_TALKER
            || selectionMode == SelectionMode.SELECT_TRADER || selectionMode == SelectionMode.SELECT_WEAPON_TRADER)
        {
            interactIdx--;
            if (interactIdx < 0)
            {
                interactIdx = interactableUnits.Count - 1;
            }
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.GAMEOVER || selectionMode == SelectionMode.ESCAPE_MENU)
        {
            instantiatedSpecialMenu.transform.GetChild(0).GetChild(specialMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            specialMenuIdx = (specialMenuIdx + 1) % instantiatedSpecialMenu.transform.GetChild(0).childCount;
            instantiatedSpecialMenu.transform.GetChild(0).GetChild(specialMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.cyan;
        }
    }
    public override void RIGHT()
    {
        if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.MOVE)
            && cursorX != width - 1)
        {
            cursorX++;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.SELECT_ENEMY || selectionMode == SelectionMode.SELECT_TALKER
            || selectionMode == SelectionMode.SELECT_TRADER || selectionMode == SelectionMode.SELECT_WEAPON_TRADER)
        {
            interactIdx = (interactIdx + 1) % interactableUnits.Count;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();
        }
    }
    public override void ENTER()
    {
        //TODO maybe something?
    }
    public override void ESCAPE()
    {
        if (selectionMode == SelectionMode.ROAM) {
            instantiatedSpecialMenu = Instantiate(escapeMenu);
            instantiatedSpecialMenu.transform.position = new Vector3(cam.transform.position.x,
                cam.transform.position.y, SPECIAL_MENU_LAYER);
            instantiatedMapHUD.SetActive(false);
            instantiatedSpecialMenu.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.cyan;
            specialMenuIdx = 0;
            selectionMode = SelectionMode.ESCAPE_MENU;
        }
    }

    private Tile getTileAtMouse(float mouseX, float mouseY)
    {
        int xCoord = Mathf.FloorToInt((mouseX + cam.transform.position.x) / cellSize);
        int yCoord = Mathf.FloorToInt((mouseY + cam.transform.position.y) / cellSize);
        if (xCoord < 0 || xCoord >= width || yCoord < 0 || yCoord >= height)
        {
            return null;
        }
        return gridArray[xCoord, yCoord];
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.ENEMYPHASE_SELECT_UNIT)
        {
            if (!objectiveComplete && objective.checkFailed(this))
            {
                //TODO game over
                selectionMode = SelectionMode.GAMEOVER;
                instantiatedMapHUD.SetActive(false);
                instantiatedSpecialMenu = Instantiate(gameOver);
                instantiatedSpecialMenu.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.cyan;
                instantiatedSpecialMenu.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, SPECIAL_MENU_LAYER);
                specialMenuIdx = 0;
                Debug.Log("CHAPTER FAILED");
            }
            else if (!objectiveComplete && objective.checkComplete(this))
            {
                selectionMode = SelectionMode.STANDBY;
                if (turn < turnPar)
                {
                    CampaignData.bonusEXP += (turnPar - turn) * 50;
                }
                instantiatedMapHUD.SetActive(false);
                objectiveComplete = true;
                Debug.Log("CHAPTER COMPLETE");
            } 
        }
        if (selectionMode == SelectionMode.BATTLE || selectionMode == SelectionMode.ENEMYPHASE_ATTACK
            || selectionMode == SelectionMode.ALLYPHASE_ATTACK)
        {
            if (attackPartIdx == attackParts.Count)
            {
                //Weapon broke
                //Gained EXP
                //Leveled up
                if (findBattleEndStart) {
                    if (brokenWeapon != null)
                    {
                        Debug.Log("Start with broken weapon");
                        instantiatedBreakAndExpPane = Instantiate(breakAndExpPane);
                        instantiatedBreakAndExpPane.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text
                            = "Your " + brokenWeapon.itemName + " broke!";
                        GameObject canvas = GameObject.Find("Canvas");

                        showingBrokenWeapon = true;
                        brokenWeapon = null;
                        timer = 3;
                    } else if (expToAdd > 0 && expUnit.isAlive())
                    {
                        Debug.Log("Start with EXP");
                        instantiatedBreakAndExpPane = Instantiate(breakAndExpPane);
                        int lvl = expUnit.level;
                        int exp = expUnit.experience;
                        levelGrowths = expUnit.addExperience(expToAdd);
                        string msg = "LVL " + lvl + " : " + exp + " EXP => LVL "
                            + expUnit.level + " : " + expUnit.experience + " EXP";
                        instantiatedBreakAndExpPane.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text
                            = msg;
                        GameObject canvas = GameObject.Find("Canvas");

                        showingEXP = true;
                        timer = 3;
                    }
                    else
                    {
                        Debug.Log("Immediately deconstruct");
                        deconstructBattle();
                        //Set selectionMode in deconstructor to make code prettier
                    }
                    findBattleEndStart = false;
                }
                if (timer <= 0)
                {
                    if (showingBrokenWeapon)
                    {
                        Debug.Log("Done with broken weapon go to EXP");
                        showingBrokenWeapon = false;
                        int lvl = expUnit.level;
                        int exp = expUnit.experience;
                        levelGrowths = expUnit.addExperience(expToAdd);
                        string msg = "LVL " + lvl + " : " + exp + " EXP => LVL "
                            + expUnit.level + " : " + expUnit.experience + " EXP";
                        instantiatedBreakAndExpPane.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text
                            = msg;

                        showingEXP = true;
                        timer = 3;
                    } else if (showingEXP)
                    {
                        Debug.Log("Done with EXP");
                        Destroy(instantiatedBreakAndExpPane);
                        showingEXP = false;
                        if (levelGrowths != null)
                        {
                            Debug.Log("Go to LVL");
                            instantiatedLevelUpBackground = Instantiate(levelUpBackground);
                            instantiatedLevelUpBackground.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                                = expUnit.GetComponent<SpriteRenderer>().sprite;
                            Transform canvas = instantiatedLevelUpBackground.transform.GetChild(2);
                            canvas.GetChild(0).GetComponent<TextMeshProUGUI>().text = expUnit.unitName;
                            canvas.GetChild(1).GetComponent<TextMeshProUGUI>().text = "HP       " + expUnit.maxHP;
                            canvas.GetChild(2).GetComponent<TextMeshProUGUI>().text = "STR       " + expUnit.strength;
                            canvas.GetChild(3).GetComponent<TextMeshProUGUI>().text = "MAG       " + expUnit.magic;
                            canvas.GetChild(4).GetComponent<TextMeshProUGUI>().text = "SKL       " + expUnit.skill;
                            canvas.GetChild(5).GetComponent<TextMeshProUGUI>().text = "SPD       " + expUnit.speed;
                            canvas.GetChild(6).GetComponent<TextMeshProUGUI>().text = "LCK       " + expUnit.luck;
                            canvas.GetChild(7).GetComponent<TextMeshProUGUI>().text = "DEF       " + expUnit.defense;
                            canvas.GetChild(8).GetComponent<TextMeshProUGUI>().text = "RES       " + expUnit.resistance;
                            for (int q = 0; q < levelGrowths.Length; q++)
                            {
                                if (levelGrowths[q])
                                {
                                    canvas.GetChild(q + 1).GetComponent<TextMeshProUGUI>().color = Color.cyan;
                                    canvas.GetChild(q + 1).GetComponent<TextMeshProUGUI>().text += " (+1)";
                                }
                            }
                            for (int q = 0; q < instantiatedBattleUI.transform.GetChild(1).childCount; q++)
                            {
                                instantiatedBattleUI.SetActive(false);
                            }
                            timer = 3;
                            showingLevelUp = true;
                        } else
                        {
                            Debug.Log("Deconstruct");
                            deconstructBattle();
                            //Set selectionMode in deconstructor to make code prettier
                        }
                    } else if (showingLevelUp)
                    {
                        Debug.Log("Done with LVL deconstruct");
                        Destroy(instantiatedLevelUpBackground);

                        showingLevelUp = false;

                        deconstructBattle();
                        //Set selectionMode in deconstructor to make code prettier
                    }
                }
                return;

            }
            AttackComponent currentATK = attackParts[attackPartIdx];
            if (attackMode == 0)
            {
                if (Mathf.Abs(currentATK.atkSprite.transform.position.x - currentATK.xDest) > 1)
                {
                    currentATK.atkSprite.transform.position += (Vector3.right * currentATK.direction * battleMoveSpeed * Time.deltaTime);
                }
                else
                {
                    if (currentATK.atkUnit.team == Unit.UnitTeam.PLAYER
                        && currentATK.atkUnit.equipped == 1 && currentATK.atkUnit.heldWeapon != null)
                    {
                        currentATK.atkUnit.proficiency++;
                    }
                    if (currentATK.hitStatus != 0)
                    {
                        keepBattling = CombatManager.performAttack(currentATK.atkUnit, currentATK.atkWeapon,
                            currentATK.dfdUnit, currentATK.damage, currentATK.hitStatus == 2);
                        currentATK.dfdHPText.text = currentATK.dfdUnit.currentHP + "/" + currentATK.dfdUnit.maxHP;
                        if (!keepBattling)
                        {
                            currentATK.dfdTile.setOccupant(null);
                            if (!currentATK.dfdUnit.isEssential
                                && (currentATK.dfdUnit.team == Unit.UnitTeam.PLAYER || currentATK.dfdUnit.team == Unit.UnitTeam.ENEMY))
                            {
                                currentATK.dfdTile.gemstones.Add(new Gemstone(currentATK.dfdUnit));
                                if (currentATK.dfdUnit.heldItem is Gemstone)
                                {
                                    currentATK.dfdTile.gemstones.Add((Gemstone)currentATK.dfdUnit.heldItem);
                                }
                            }
                            currentATK.dfdUnit.outline.enabled = false;
                            currentATK.dfdUnit.gameObject.SetActive(false);
//                            currentATK.dfdUnit.GetComponent<SpriteRenderer>().enabled = false;
                            player.Remove(currentATK.dfdUnit);
                            enemy.Remove(currentATK.dfdUnit);
                            ally.Remove(currentATK.dfdUnit);
                            other.Remove(currentATK.dfdUnit);
                            if (currentATK.atkUnit.team == Unit.UnitTeam.PLAYER)
                            {
                                expToAdd = currentATK.dfdUnit.rawEXPReward();
                                expUnit = currentATK.atkUnit;
                            }
                        } else if (currentATK.atkUnit.team == Unit.UnitTeam.PLAYER)
                        {
                            if (currentATK.hitStatus > 0) {
                                expToAdd = Mathf.Max(expToAdd, 10);
                            } else
                            {
                                expToAdd = Mathf.Max(expToAdd, 1);
                            }
                            expUnit = currentATK.atkUnit;
                        }
                        else if (currentATK.dfdUnit.team == Unit.UnitTeam.PLAYER)
                        {
                            expToAdd = Mathf.Max(expToAdd, 1);
                            expUnit = currentATK.dfdUnit;
                        }
                        if (currentATK.atkUnit.team == Unit.UnitTeam.PLAYER && currentATK.atkWeapon.uses > -1 && currentATK.atkWeapon.usesLeft <= 0)
                        {
                            brokenWeapon = currentATK.atkWeapon;
                            currentATK.atkUnit.breakEquippedWeapon();
                        }
                    }
                    attackMode++;
                }
            } else if (attackMode == 1)
            {
                if (Mathf.Abs(currentATK.atkSprite.transform.position.x - currentATK.startPos) > 1)
                {
                    currentATK.atkSprite.transform.position += (Vector3.left * currentATK.direction * battleMoveSpeed * Time.deltaTime);
                } else
                {
                    attackMode = 0;
                    attackPartIdx++;
                    if (!keepBattling)
                    {
                        attackPartIdx = attackParts.Count;
                    }
                }
            }
        }
        else if (selectionMode == SelectionMode.ENEMYPHASE_SELECT_UNIT)
        {
            enemyIdx++;
            if (enemyIdx >= enemy.Count)
            {
                foreach (Tile t in healTiles)
                {
                    if (t.getOccupant() != null && t.getOccupant().team == Unit.UnitTeam.ALLY)
                    {
                        t.getOccupant().heal(10);
                    }
                }
                enemyIdx = -1; //ALLYPHASE_SELECT_UNIT increments it to 0 first
                instantiatedMapHUD.transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.green;
                selectionMode = SelectionMode.ALLYPHASE_SELECT_UNIT;
                return;
            }
            currentEnemy = enemy[enemyIdx];
            enemyAction = performUnitAI(currentEnemy);
            if (enemyAction != null)
            {
                Unit.AIType ai = (Unit.AIType)enemyAction[0];
                if (ai == Unit.AIType.ATTACK)
                {
                    enemyStartTile = (Tile)enemyAction[1];
                    enemyDestTile = (Tile)enemyAction[2];
                    if (enemyAction[1] == enemyAction[2])
                    {
                        setCameraPosition(enemyStartTile.x, enemyStartTile.y);

                        timer = (float)1.2;
                        Tile dfdTile = (Tile)enemyAction[3];
                        cursorX = dfdTile.x;
                        cursorY = dfdTile.y;
                        updateCursor();

                        selectionMode = SelectionMode.ENEMYPHASE_COMBAT_PAUSE;
                    }
                    else
                    {
                        if (enemyDestTile == null)
                        {
                            return;
                        }
                        int midPointX = (enemyStartTile.x + enemyDestTile.x) / 2;
                        int midPointY = (enemyStartTile.y + enemyDestTile.y) / 2;
                        setCameraPosition(midPointX, midPointY);

                        moveTime = (Mathf.Abs(enemyStartTile.x - enemyDestTile.x) + Mathf.Abs(enemyStartTile.y - enemyDestTile.y)) / (float)3.0;
                        selectionMode = SelectionMode.ENEMYPHASE_MOVE;
                    }
                }
                else if (ai == Unit.AIType.GUARD)
                {
                    enemyStartTile = (Tile)enemyAction[1];
                    enemyDestTile = (Tile)enemyAction[2]; //Same as enemyStartTile
                    setCameraPosition(enemyStartTile.x, enemyStartTile.y);

                    timer = (float)1.2;
                    Tile dfdTile = (Tile)enemyAction[3];
                    cursorX = dfdTile.x;
                    cursorY = dfdTile.y;
                    updateCursor();

                    selectionMode = SelectionMode.ENEMYPHASE_COMBAT_PAUSE;
                }
                else if (ai == Unit.AIType.BURN)
                {
                    //TODO
                } else if (ai == Unit.AIType.PURSUE)
                {
                    //TODO
                }
            }
        } else if (selectionMode == SelectionMode.ENEMYPHASE_MOVE)
        {
            if (Mathf.Abs(currentEnemy.transform.position.x - enemyDestTile.transform.position.x) > 0.1
                || Mathf.Abs(currentEnemy.transform.position.y - enemyDestTile.transform.position.y) > 0.1)
            {
                currentEnemy.transform.position += new Vector3((enemyDestTile.transform.position.x - enemyStartTile.transform.position.x) * Time.deltaTime / moveTime,
                    (enemyDestTile.transform.position.y - enemyStartTile.transform.position.y) * Time.deltaTime / moveTime, 0);
            } else
            {
                currentEnemy.transform.position = new Vector3(enemyDestTile.transform.position.x,
                    enemyDestTile.transform.position.y, UNIT_SPRITE_LAYER);
                enemyDestTile.setOccupant(currentEnemy);
                enemyStartTile.setOccupant(null);

                Unit.AIType ai = (Unit.AIType)enemyAction[0];
                if (ai == Unit.AIType.ATTACK)
                {
                    timer = (float)1.2;
                    Tile dfdTile = (Tile)enemyAction[3];
                    cursorX = dfdTile.x;
                    cursorY = dfdTile.y;
                    updateCursor();

                    selectionMode = SelectionMode.ENEMYPHASE_COMBAT_PAUSE;
                }
                else if (ai == Unit.AIType.BURN)
                {
                    //TODO
                }
                else if (ai == Unit.AIType.PURSUE)
                {
                    if (enemyAction[3] == null)
                    {
                        selectionMode = SelectionMode.ENEMYPHASE_SELECT_UNIT;
                    } else
                    {
                        timer = (float)1.2;
                        Tile dfdTile = (Tile)enemyAction[3];
                        cursorX = dfdTile.x;
                        cursorY = dfdTile.y;
                        updateCursor();

                        selectionMode = SelectionMode.ENEMYPHASE_COMBAT_PAUSE;
                    }
                }
            }
        } else if (selectionMode == SelectionMode.ENEMYPHASE_COMBAT_PAUSE)
        {
            if (timer <= 0)
            {
                Tile dfdTile = (Tile)enemyAction[3];
                Unit dfd = dfdTile.getOccupant();
                performBattle(dfd, currentEnemy, dfd.getEquippedWeapon(),
                    currentEnemy.getEquippedWeapon(), dfdTile, enemyDestTile, false);

                selectionMode = SelectionMode.ENEMYPHASE_ATTACK;
            }
        }
        else if (selectionMode == SelectionMode.ALLYPHASE_SELECT_UNIT)
        {
            allyIdx++;
            if (allyIdx >= ally.Count)
            {
                //TODO everything for switching to player phase
                foreach (Tile t in healTiles)
                {
                    if (t.getOccupant() != null && t.getOccupant().team == Unit.UnitTeam.PLAYER)
                    {
                        t.getOccupant().heal(10);
                    }
                }
                foreach (Unit u in player)
                {
                    u.isExhausted = false;
                    u.outline.color = Color.blue;
                }
                instantiatedMapHUD.transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.blue;
                Unit firstUnit = player[0];
                int[] firstTile = findUnit(firstUnit);
                cursorX = firstTile[0];
                cursorY = firstTile[1];
                updateCursor();
                setCameraPosition(cursorX, cursorY);
                turn++;
                selectionMode = SelectionMode.ROAM;
                return;
            }
            currentAlly = ally[allyIdx];
            allyAction = performUnitAI(currentAlly);
            if (allyAction != null)
            {
                Unit.AIType ai = (Unit.AIType)allyAction[0];
                if (ai == Unit.AIType.ATTACK)
                {
                    allyStartTile = (Tile)allyAction[1];
                    allyDestTile = (Tile)allyAction[2];
                    if (allyAction[1] == allyAction[2])
                    {
                        setCameraPosition(allyStartTile.x, allyStartTile.y);

                        timer = (float)1.2;
                        Tile dfdTile = (Tile)allyAction[3];
                        cursorX = dfdTile.x;
                        cursorY = dfdTile.y;
                        updateCursor();

                        selectionMode = SelectionMode.ALLYPHASE_COMBAT_PAUSE;
                    }
                    else
                    {
                        if (allyDestTile == null)
                        {
                            return;
                        }
                        int midPointX = (allyStartTile.x + allyDestTile.x) / 2;
                        int midPointY = (allyStartTile.y + allyDestTile.y) / 2;
                        setCameraPosition(midPointX, midPointY);

                        moveTime = (Mathf.Abs(allyStartTile.x - allyDestTile.x) + Mathf.Abs(allyStartTile.y - allyDestTile.y)) / (float)3.0;
                        selectionMode = SelectionMode.ALLYPHASE_MOVE;
                    }
                }
                else if (ai == Unit.AIType.GUARD)
                {
                    allyStartTile = (Tile)allyAction[1];
                    allyDestTile = (Tile)allyAction[2]; //Same as enemyStartTile
                    setCameraPosition(allyStartTile.x, allyStartTile.y);

                    timer = (float)1.2;
                    Tile dfdTile = (Tile)allyAction[3];
                    cursorX = dfdTile.x;
                    cursorY = dfdTile.y;
                    updateCursor();

                    selectionMode = SelectionMode.ALLYPHASE_COMBAT_PAUSE;
                }
                else if (ai == Unit.AIType.BURN)
                {
                    //TODO
                } else if (ai == Unit.AIType.PURSUE)
                {
                    //TODO
                }
            }
        } else if (selectionMode == SelectionMode.ALLYPHASE_MOVE)
        {
            if (Mathf.Abs(currentAlly.transform.position.x - allyDestTile.transform.position.x) > 0.1
                || Mathf.Abs(currentAlly.transform.position.y - allyDestTile.transform.position.y) > 0.1)
            {
                currentAlly.transform.position += new Vector3((allyDestTile.transform.position.x - allyStartTile.transform.position.x) * Time.deltaTime / moveTime,
                    (allyDestTile.transform.position.y - allyStartTile.transform.position.y) * Time.deltaTime / moveTime, 0);
            } else
            {
                currentAlly.transform.position = new Vector3(allyDestTile.transform.position.x,
                    allyDestTile.transform.position.y, UNIT_SPRITE_LAYER);
                allyDestTile.setOccupant(currentAlly);
                allyStartTile.setOccupant(null);

                Unit.AIType ai = (Unit.AIType)allyAction[0];
                if (ai == Unit.AIType.ATTACK)
                {
                    timer = (float)1.2;
                    Tile dfdTile = (Tile)allyAction[3];
                    cursorX = dfdTile.x;
                    cursorY = dfdTile.y;
                    updateCursor();

                    selectionMode = SelectionMode.ALLYPHASE_COMBAT_PAUSE;
                }
                else if (ai == Unit.AIType.BURN)
                {
                    //TODO
                }
                else if (ai == Unit.AIType.PURSUE)
                {
                    if (allyAction[3] == null)
                    {
                        selectionMode = SelectionMode.ALLYPHASE_SELECT_UNIT;
                    } else
                    {
                        timer = (float)1.2;
                        Tile dfdTile = (Tile)allyAction[3];
                        cursorX = dfdTile.x;
                        cursorY = dfdTile.y;
                        updateCursor();

                        selectionMode = SelectionMode.ALLYPHASE_COMBAT_PAUSE;
                    }
                }
            }
        } else if (selectionMode == SelectionMode.ALLYPHASE_COMBAT_PAUSE)
        {
            if (timer <= 0)
            {
                Tile dfdTile = (Tile)allyAction[3];
                Unit dfd = dfdTile.getOccupant();
                performBattle(currentAlly, dfd, currentAlly.getEquippedWeapon(),
                    dfd.getEquippedWeapon(), allyDestTile, dfdTile, true);

                selectionMode = SelectionMode.ALLYPHASE_ATTACK;
            }
        } 
        else if (selectionMode == SelectionMode.NOTIFICATION)
        {
            if (timer <= 0)
            {
                Destroy(instantiatedItemNote);

                selectionMode = SelectionMode.ROAM;
            }
        }
    }

    public void selectMenuOption(MenuChoice choice)
    {
        if (choice == MenuChoice.TALK)
        {
            unfillAttackableTiles();
            interactableUnits = getTalkableTiles(moveDest, selectedUnit);
            foreach (Tile t in interactableUnits)
            {
                t.interactHighlight.SetActive(true);
            }
            interactIdx = 0;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectionMode = SelectionMode.SELECT_TALKER;

        }
        else if (choice == MenuChoice.ATTACK)
        {
            unfillAttackableTiles();
            Dictionary<Tile, object> attackable = getAttackableBattlegroundTilesFromDestination(selectedUnit, moveDest);
            interactableUnits = getAttackableTilesWithEnemies(attackable, selectedUnit);
            foreach (Tile t in interactableUnits)
            {
                t.attackHighlight.SetActive(true);
            }
            interactIdx = 0;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectionMode = SelectionMode.SELECT_ENEMY;
        }
        else if (choice == MenuChoice.WEAPON)
        {
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            menuOptions.Clear();
            menuElements.Clear();
            if (selectedUnit.personalItem is Weapon)
            {
                TextMeshProUGUI wep = Instantiate(menuOption);
                wep.text = "Equip " + selectedUnit.personalItem.itemName;
                wep.fontSize = 13;
                if (selectedUnit.equipped == 0)
                {
                    wep.color = Color.green;
                }
                menuOptions.Add(wep);
                menuElements.Add(MenuChoice.EQUIP_PERSONAL);
            }
            if (selectedUnit.heldWeapon != null)
            {
                if (selectedUnit.weaponType == selectedUnit.heldWeapon.weaponType
                    && selectedUnit.proficiency >= selectedUnit.heldWeapon.proficiency)
                {
                    TextMeshProUGUI wep = Instantiate(menuOption);
                    wep.text = "Equip " + selectedUnit.heldWeapon.itemName;
                    wep.fontSize = 13;
                    if (selectedUnit.equipped == 1)
                    {
                        wep.color = Color.green;
                    }
                    menuOptions.Add(wep);
                    menuElements.Add(MenuChoice.EQUIP_HELD);
                }
                if (getAdjacentTilesWithAllies(moveDest, selectedUnit).Count > 0)
                {
                    TextMeshProUGUI wep = Instantiate(menuOption);
                    wep.text = "Trade " + selectedUnit.heldWeapon.itemName;
                    wep.fontSize = 13;
                    menuOptions.Add(wep);
                    menuElements.Add(MenuChoice.TRADE_WEAPON);
                }
                TextMeshProUGUI drop = Instantiate(menuOption);
                drop.text = "Drop " + selectedUnit.heldWeapon.itemName;
                drop.fontSize = 13;
                menuOptions.Add(drop);
                menuElements.Add(MenuChoice.DROP_WEAPON);
            }
            TextMeshProUGUI none = Instantiate(menuOption);
            none.text = "Equip None";
            none.fontSize = 13;
            menuOptions.Add(none);
            menuElements.Add(MenuChoice.EQUIP_NONE);

            if (!menuOptions[0].color.Equals(Color.green))
            {
                menuOptions[0].color = Color.white;
            }
            menuIdx = 0;

            for (int q = 0; q < menuOptions.Count; q++)
            {
                menuOptions[q].transform.SetParent(instantiatedMenuBackground.transform.GetChild(0), false);
                menuOptions[q].transform.position = new Vector3(menuOptions[q].transform.position.x,
                    menuOptions[q].transform.position.y - (50 * q), menuOptions[q].transform.position.z);
            }

            selectionMode = SelectionMode.SELECT_WEAPON;
        }
        else if (choice == MenuChoice.ESCAPE)
        {
            unfillAttackableTiles();

            player.Remove(selectedUnit);
            CampaignData.registerSupportUponEscape(selectedUnit, player, turn);
            selectedUnit.outline.enabled = false;
            selectedUnit.gameObject.SetActive(false);
//            selectedUnit.GetComponent<SpriteRenderer>().enabled = false;
            selectedTile.setOccupant(null);

            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectionMode = SelectionMode.ROAM;
        }
        else if (choice == MenuChoice.ITEM)
        {
            //TODO options for USE, DROP, and TRADE
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            menuOptions.Clear();
            menuElements.Clear();
            if (selectedUnit.personalItem is UsableItem)
            {
                TextMeshProUGUI pers = Instantiate(menuOption);
                pers.text = "Use " + selectedUnit.personalItem.itemName;
                if (pers.text.Length > 7)
                {
                    pers.fontSize = 13;
                }
                menuOptions.Add(pers);
                menuElements.Add(MenuChoice.USE_PERSONAL);
            }
            if (selectedUnit.heldItem is UsableItem)
            {
                TextMeshProUGUI held = Instantiate(menuOption);
                held.text = "Use " + selectedUnit.heldItem.itemName;
                if (held.text.Length > 7)
                {
                    held.fontSize = 13;
                }
                menuOptions.Add(held);
                menuElements.Add(MenuChoice.USE_HELD);
            }
            if (selectedUnit.heldItem != null)
            {
                if (getAdjacentTilesWithAllies(moveDest, selectedUnit).Count > 0)
                {
                    TextMeshProUGUI trade = Instantiate(menuOption);
                    trade.text = "Trade Items";
                    if (trade.text.Length > 7)
                    {
                        trade.fontSize = 13;
                    }
                    menuOptions.Add(trade);
                    menuElements.Add(MenuChoice.TRADE);
                }
                TextMeshProUGUI drop = Instantiate(menuOption);
                drop.text = "Drop " + selectedUnit.heldItem.itemName;
                if (drop.text.Length > 7)
                {
                    drop.fontSize = 13;
                }
                menuOptions.Add(drop);
                menuElements.Add(MenuChoice.DROP);
            }

            menuOptions[0].color = Color.white;
            menuIdx = 0;

            for (int q = 0; q < menuOptions.Count; q++)
            {
                menuOptions[q].transform.SetParent(instantiatedMenuBackground.transform.GetChild(0), false);
                menuOptions[q].transform.position = new Vector3(menuOptions[q].transform.position.x,
                    menuOptions[q].transform.position.y - (50 * q), menuOptions[q].transform.position.z);
            }

            selectionMode = SelectionMode.ITEM_MENU;

        } else if (choice == MenuChoice.USE_PERSONAL)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;


            ((UsableItem)selectedUnit.personalItem).use(selectedUnit);

            if (moveDest.hasLoot() && Random.Range(0, 100) < selectedUnit.luck) //OR selectedUnit is a thief
            {
                takeItem(selectedUnit, moveDest);
                //Show what was taken for a couple seconds
            }
            else
            {
                selectionMode = SelectionMode.ROAM;
            }
        }
        else if (choice == MenuChoice.USE_HELD)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;

            ((UsableItem)selectedUnit.heldItem).use(selectedUnit);
            if (selectedUnit.heldItem.usesLeft <= 0)
            {
                selectedUnit.heldItem = null;
            }

            if (moveDest.hasLoot() && Random.Range(0, 100) < selectedUnit.luck) //OR selectedUnit is a thief
            {
                takeItem(selectedUnit, moveDest);
                //Show what was taken for a couple seconds
            }
            else
            {
                selectionMode = SelectionMode.ROAM;
            }
        }
        else if (choice == MenuChoice.TRADE)
        {
            unfillAttackableTiles();
            interactableUnits = getAdjacentTilesWithAllies(moveDest, selectedUnit);
            foreach (Tile t in interactableUnits)
            {
                t.interactHighlight.SetActive(true);
            }
            interactIdx = 0;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectionMode = SelectionMode.SELECT_TRADER;
        }
        else if (choice == MenuChoice.TRADE_WEAPON)
        {
            unfillAttackableTiles();
            interactableUnits = getAdjacentTilesWithAllies(moveDest, selectedUnit);
            foreach (Tile t in interactableUnits)
            {
                t.interactHighlight.SetActive(true);
            }
            interactIdx = 0;
            cursorX = interactableUnits[interactIdx].x; cursorY = interactableUnits[interactIdx].y;
            updateCursor();

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectionMode = SelectionMode.SELECT_WEAPON_TRADER;
        }
        else if (choice == MenuChoice.DROP)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;

            if (selectedUnit.heldItem is Gemstone)
            {
                moveDest.gemstones.Add((Gemstone)selectedUnit.heldItem);
            }
            selectedUnit.heldItem = null;

            if (moveDest.hasLoot() && Random.Range(0, 100) < selectedUnit.luck) //OR selectedUnit is a thief
            {
                takeItem(selectedUnit, moveDest);
                //Show what was taken for a couple seconds
            }
            else
            {
                selectionMode = SelectionMode.ROAM;
            }
        }
        else if (choice == MenuChoice.DROP_WEAPON)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;

            selectedUnit.heldWeapon = null;

            if (moveDest.hasLoot() && Random.Range(0, 100) < selectedUnit.luck) //OR selectedUnit is a thief
            {
                takeItem(selectedUnit, moveDest);
                //Show what was taken for a couple seconds
            }
            else
            {
                selectionMode = SelectionMode.ROAM;
            }
        }
        else if (choice == MenuChoice.CHEST)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;

//            moveDest.GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage("open_chest.jpg");
            //^ done in takeItem
            takeItem(selectedUnit, moveDest);
            //Show what was taken for a couple seconds through takeItem. Do not set selectionMode
        }
        else if (choice == MenuChoice.SEIZE)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            menuOptions.Clear();

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;
            seized = true;

            selectionMode = SelectionMode.ROAM;

        } else if (choice == MenuChoice.GEM)
        {
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            menuOptions.Clear();
            for (int q = 0; q < moveDest.gemstones.Count; q++)
            {
                Gemstone gem = moveDest.gemstones[q];
                TextMeshProUGUI text = Instantiate(menuOption);
                text.text = gem.unit.unitName;
                if (text.text.Length > 7)
                {
                    text.fontSize = 13;
                }
                menuOptions.Add(text);
            }
            menuOptions[0].color = Color.white;
            menuIdx = 0;

            for (int q = 0; q < menuOptions.Count; q++)
            {
                menuOptions[q].transform.SetParent(instantiatedMenuBackground.transform.GetChild(0), false);
                menuOptions[q].transform.position = new Vector3(menuOptions[q].transform.position.x,
                    menuOptions[q].transform.position.y - (50 * q), menuOptions[q].transform.position.z);
            }

            selectionMode = SelectionMode.SELECT_GEM;

        }
        else if (choice == MenuChoice.WAIT)
        {
            unfillAttackableTiles();
            moveDest.moveSpriteOutline.SetActive(false);
            moveDest.moveSprite.SetActive(false);

            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            selectedTile.setOccupant(null);
            moveDest.setOccupant(selectedUnit);
            selectedUnit.isExhausted = true;
            selectedUnit.outline.color = Color.grey;
            if (moveDest.hasLoot() && Random.Range(0, 100) < selectedUnit.luck) //OR selectedUnit is a thief
            {
                takeItem(selectedUnit, moveDest);
                //Show what was taken for a couple seconds
            } else
            {
                selectionMode = SelectionMode.ROAM;
            }
        } else if (choice == MenuChoice.STATUS)
        {
            //TODO show status page
            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            cam.transform.position = new Vector3(0, 0, CAMERA_LAYER);
            Camera camCam = cam.GetComponent<Camera>();
            camCam.orthographicSize = (float)17.5;
            instantiatedMapHUD.SetActive(false);
            statusPage.SetActive(true);
            Transform canvas = statusPage.transform.GetChild(0);
            canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = chapterName;
            canvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = teamNames[0] + ": " + player.Count;
            canvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = teamNames[1] + ": " + enemy.Count;
            canvas.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = teamNames[2] + (ally.Count > 0 ? (": " + ally.Count) : "");
            canvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Objective: " + objective.getName() + "\n\nDefeat: " + objective.getFailure();
            canvas.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Turn " + turn;
            canvas.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "(Par: " + turnPar + ")";
            canvas.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "Iron: " + CampaignData.iron
                + "\nSteel: " + CampaignData.steel
                + "\nSilver: " + CampaignData.silver;

            selectionMode = SelectionMode.STATUS;
        }
        else if (choice == MenuChoice.CONTROLS)
        {
            //TODO show controls page
            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }
            cam.transform.position = new Vector3(0, 0, CAMERA_LAYER);
            Camera camCam = cam.GetComponent<Camera>();
            camCam.orthographicSize = (float)17.5;
            instantiatedMapHUD.SetActive(false);
            controlsPage.SetActive(true);

            selectionMode = SelectionMode.CONTROLS;
        }
        else if (choice == MenuChoice.END)
        {
            Destroy(instantiatedMenuBackground);
            foreach (TextMeshProUGUI t in menuOptions)
            {
                Destroy(t);
            }

            foreach (Tile t in healTiles)
            {
                if (t.getOccupant() != null && t.getOccupant().team == Unit.UnitTeam.ENEMY)
                {
                    t.getOccupant().heal(10);
                }
            }
            enemyIdx = -1; //ENEMYPHASE_SELECT_UNIT increments it to 0 first
            instantiatedMapHUD.transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.red;
            selectionMode = SelectionMode.ENEMYPHASE_SELECT_UNIT;
        }
    }

    private void updateCursor()
    {
        cursor.transform.position = new Vector3(cursorX * cellSize, cursorY * cellSize, CURSOR_LAYER);
        tileInfoText.text = "Tile: " + gridArray[cursorX, cursorY].tileName
            + "\nAvoid Bonus: " +  gridArray[cursorX, cursorY].avoidBonus;
        updateCameraPosition();
    }
    private void setCameraPosition(int x, int y)
    {
        Camera camCam = cam.GetComponent<Camera>();
        float pos75x = (width * cellSize) - (camCam.orthographicSize * camCam.aspect / 2);
        float pos25x = camCam.orthographicSize * camCam.aspect / 2;
        float pos75y = (height * cellSize) - (camCam.orthographicSize / 2);
        float pos25y = camCam.orthographicSize / 2;
        cam.transform.position = new Vector3(Mathf.Max(pos25x, Mathf.Min(pos75x, x * cellSize)),
            Mathf.Max(pos25y, Mathf.Min(pos75y, y * cellSize)), CAMERA_LAYER);
    }

    private void updateCameraPosition()
    {
        Camera camCam = cam.GetComponent<Camera>();
        if (cursorX * cellSize >= cam.transform.position.x + (camCam.orthographicSize * camCam.aspect / 2))
        {
            cam.transform.Translate(cellSize, 0, 0);
        }
        if (cursorX * cellSize <= cam.transform.position.x - (camCam.orthographicSize * camCam.aspect / 2))
        {
            cam.transform.Translate(-cellSize, 0, 0);
        }
        if (cursorY * cellSize >= cam.transform.position.y + (camCam.orthographicSize / 2))
        {
            cam.transform.Translate(0, cellSize, 0);
        }
        if (cursorY * cellSize <= cam.transform.position.y - (camCam.orthographicSize / 2))
        {
            cam.transform.Translate(0, -cellSize, 0);
        }
    }

    private void fillTraversableTiles(Unit u, int x, int y)
    {
        traversableTiles = getTraversableTiles(u, x, y);
        attackableTiles = getAttackableTiles(traversableTiles, u);
        foreach (Tile t in traversableTiles.Keys)
        {
            t.traverseHighlight.SetActive(true);
        }
        foreach (Tile t in attackableTiles.Keys)
        {
            if (!traversableTiles.ContainsKey(t))
            {
                t.attackHighlight.SetActive(true);
            }
        }
    }

    private void unfillTraversableTiles()
    {
        foreach (Tile t in traversableTiles.Keys)
        {
            t.traverseHighlight.SetActive(false);
        }
        foreach (Tile t in attackableTiles.Keys)
        {
            t.attackHighlight.SetActive(false);
        }
    }

    public Dictionary<Tile, object> getTraversableTiles(Unit u, int x, int y)
    {
        //TODO
        Dictionary<Tile, object> traversable = new Dictionary<Tile, object>();
        LinkedList<int[]> searchList = new LinkedList<int[]>();
        searchList.AddFirst(new int[] { x, y, u.movement });
        int[] dimensions = new int[] { width, height };
        while (searchList.Count != 0)
        {
            int[] from = searchList.First.Value;
            searchList.RemoveFirst();
            Tile fromTile = gridArray[from[0], from[1]];
            addKeyAndValue(traversable, fromTile, from[2]);
            if (from[2] == 0)
            {
                continue;
            }
            if (from[0] > 0)
            {
                int checkX = from[0] - 1;
                int checkY = from[1];
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)outInt < from[2])
                        && from[2] - check.getMoveCost(u) >= 0
                        && (check.isVacant()
                                || enemy.Contains(check.getOccupant()) == enemy.Contains(u)))
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
                }
            }
            if (from[0] < dimensions[0] - 1)
            {
                int checkX = from[0] + 1;
                int checkY = from[1];
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)outInt < from[2])
                        && from[2] - check.getMoveCost(u) >= 0
                        && (check.isVacant()
                                || enemy.Contains(check.getOccupant()) == enemy.Contains(u)))
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
                }
            }
            if (from[1] > 0)
            {
                int checkX = from[0];
                int checkY = from[1] - 1;
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)outInt < from[2])
                        && from[2] - check.getMoveCost(u) >= 0
                        && (check.isVacant()
                                || enemy.Contains(check.getOccupant()) == enemy.Contains(u)))
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
                }
            }
            if (from[1] < dimensions[1] - 1)
            {
                int checkX = from[0];
                int checkY = from[1] + 1;
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)outInt < from[2])
                        && from[2] - check.getMoveCost(u) >= 0
                        && (check.isVacant()
                                || enemy.Contains(check.getOccupant()) == enemy.Contains(u)))
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
                }
            }
        }
        return traversable;
    }
    public Dictionary<Tile, object> getAttackableTiles(Dictionary<Tile, object> traversable, Unit selected)
    {
        Dictionary<Tile, object> ret = new Dictionary<Tile, object>();
        Dictionary<Tile, object>.KeyCollection keys = traversable.Keys;
        foreach (Tile t in keys)
        {
            Dictionary<Tile, object> att = getAttackableBattlegroundTilesFromDestination(selected, t);
            Dictionary<Tile, object>.KeyCollection check = att.Keys;
            foreach (Tile c in check)
            {
                object outInt;
                if (!ret.TryGetValue(c, out outInt) || (int)ret[c] > (int)att[c])
                {
                    addKeyAndValue(ret, c, att[c]);
                }
            }
        }
        return ret;
    }

    public Dictionary<Tile, object> getAttackableBattlegroundTilesFromDestination(Unit u, Tile dest)
    {
        int x = dest.x;
        int y = dest.y;
        int minRange = 1;
        int maxRange = 1;
        Weapon w = u.getEquippedWeapon();
        if (w != null)
        {
            minRange = w.minRange;
            maxRange = w.maxRange;
        }
        //TODO find actual range
        Dictionary<Tile, object> traversable = new Dictionary<Tile, object>();
        Dictionary<Tile, object> attackable = new Dictionary<Tile, object>(); //Gives distance from attacker for each target
        LinkedList<int[]> searchList = new LinkedList<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
        searchList.AddFirst(new int[] { x, y, maxRange });
        int[] dimensions = new int[] { width, height };
        while (searchList.Count != 0)
        {
            int[] from = searchList.First.Value; searchList.RemoveFirst();
            Tile fromTile = gridArray[from[0], from[1]];
            addKeyAndValue(traversable, fromTile, from[2]);
            int distance = maxRange - from[2];
            if (distance >= minRange
                    && (fromTile.getOccupant() == null ||
                    enemy.Contains(fromTile.getOccupant()) != enemy.Contains(u)))
            {
                addKeyAndValue(attackable, fromTile, distance);
            }
            if (from[2] == 0)
            {
                continue;
            }
            if (from[0] > 0)
            {
                int checkX = from[0] - 1;
                int checkY = from[1];
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)traversable[check] < from[2])
                        && from[2] - 1 >= 0)
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - 1 });
                }
            }
            if (from[0] < dimensions[0] - 1)
            {
                int checkX = from[0] + 1;
                int checkY = from[1];
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)traversable[check] < from[2])
                        && from[2] - 1 >= 0)
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - 1 });
                }
            }
            if (from[1] > 0)
            {
                int checkX = from[0];
                int checkY = from[1] - 1;
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)traversable[check] < from[2])
                        && from[2] - 1 >= 0)
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - 1 });
                }
            }
            if (from[1] < dimensions[1] - 1)
            {
                int checkX = from[0];
                int checkY = from[1] + 1;
                Tile check = gridArray[checkX, checkY];
                object outInt;
                if ((!traversable.TryGetValue(check, out outInt) || (int)traversable[check] < from[2])
                        && from[2] - 1 >= 0)
                {
                    searchList.AddLast(new int[] { checkX, checkY, from[2] - 1 });
                }
            }
        }
        return attackable;
    }

    public List<Tile> getTalkableTiles(Tile dest, Unit selected)
    {
        List<Tile> ret = new List<Tile>();
        int x = dest.x;
        int y = dest.y;
        if (x > 0)
        {
            Tile t = gridArray[x - 1, y];
            Unit u = t.getOccupant();
            if (u != null && u.talkConvo != null
                    && (!u.talkRestricted || selected == player[0]))
            {
                ret.Add(t);
            }
        }
        if (x < width - 1)
        {
            Tile t = gridArray[x + 1, y];
            Unit u = t.getOccupant();
            if (u != null && u.talkConvo != null
                    && (!u.talkRestricted || selected == player[0]))
            {
                ret.Add(t);
            }
        }
        if (y > 0)
        {
            Tile t = gridArray[x, y - 1];
            Unit u = t.getOccupant();
            if (u != null && u.talkConvo != null
                    && (!u.talkRestricted || selected == player[0]))
            {
                ret.Add(t);
            }
        }
        if (y < height - 1)
        {
            Tile t = gridArray[x, y + 1];
            Unit u = t.getOccupant();
            if (u != null && u.talkConvo != null
                    && (!u.talkRestricted || selected == player[0]))
            {
                ret.Add(t);
            }
        }
        return ret;
    }

    public static List<Tile> getAttackableTilesWithEnemies(Dictionary<Tile, object> attackable,
        Unit u)
    {
        // TODO Auto-generated method stub
        List<Tile> ret = new List<Tile>();
        Dictionary<Tile, object>.KeyCollection keys = attackable.Keys;
        foreach (Tile t in keys)
        {
            if (t.getOccupant() != null)
            {
                ret.Add(t);
            }
        }
        return ret;
    }

    public List<Tile> getAdjacentTilesWithAllies(Tile here, Unit unit)
    {
        List<Tile> ret = new List<Tile>();
        if (here.x > 0)
        {
            Tile attempt = gridArray[here.x - 1, here.y];
            if (attempt.getOccupant() != null && attempt.getOccupant() != unit && attempt.getOccupant().team == unit.team)
            {
                ret.Add(attempt);
            }
        }
        if (here.x < width - 1)
        {
            Tile attempt = gridArray[here.x + 1, here.y];
            if (attempt.getOccupant() != null && attempt.getOccupant() != unit && attempt.getOccupant().team == unit.team)
            {
                ret.Add(attempt);
            }
        }
        if (here.y > 0)
        {
            Tile attempt = gridArray[here.x, here.y - 1];
            if (attempt.getOccupant() != null && attempt.getOccupant() != unit && attempt.getOccupant().team == unit.team)
            {
                ret.Add(attempt);
            }
        }
        if (here.y < height - 1)
        {
            Tile attempt = gridArray[here.x, here.y + 1];
            if (attempt.getOccupant() != null && attempt.getOccupant() != unit && attempt.getOccupant().team == unit.team)
            {
                ret.Add(attempt);
            }
        }
        return ret;
    }

    public void getMapMenuOptions()
    {
        menuOptions = new List<TextMeshProUGUI>();
        menuElements = new List<MenuChoice>();

        TextMeshProUGUI status = Instantiate(menuOption);
        status.text = "Status";
        menuOptions.Add(status);
        menuElements.Add(MenuChoice.STATUS);

        TextMeshProUGUI controls = Instantiate(menuOption);
        controls.text = "Controls";
        menuOptions.Add(controls);
        menuElements.Add(MenuChoice.CONTROLS);

        TextMeshProUGUI end = Instantiate(menuOption);
        end.text = "End Turn";
        menuOptions.Add(end);
        menuElements.Add(MenuChoice.END);

        menuOptions[0].color = Color.white;
        menuIdx = 0;

        for (int q = 0; q < menuOptions.Count; q++)
        {
            menuOptions[q].transform.SetParent(instantiatedMenuBackground.transform.GetChild(0), false);
            menuOptions[q].transform.position = new Vector3(menuOptions[q].transform.position.x,
                menuOptions[q].transform.position.y - (50 * q), menuOptions[q].transform.position.z);
        }

    }

    public void getMenuOptions()
    {
        menuOptions = new List<TextMeshProUGUI>();
        menuElements = new List<MenuChoice>();
        fillAttackableTiles();
        if (selectedUnit.isLeader && moveDest.tileName.Equals("SEIZE POINT"))
        {
            TextMeshProUGUI seize = Instantiate(menuOption);
            seize.text = "Seize";
            menuOptions.Add(seize);
            menuElements.Add(MenuChoice.SEIZE);
        }
        List<Tile> talkable = getTalkableTiles(moveDest, selectedUnit);
        if (talkable.Count != 0)
        {
            TextMeshProUGUI talk = Instantiate(menuOption);
            talk.text = "Talk";
            menuOptions.Add(talk);
            menuElements.Add(MenuChoice.TALK);
        }
        Dictionary<Tile, object> attackable = getAttackableBattlegroundTilesFromDestination(selectedUnit, moveDest);
        List<Tile> reallyAttackable = getAttackableTilesWithEnemies(attackable, selectedUnit);
        if (reallyAttackable.Count != 0)
        {
            TextMeshProUGUI attack = Instantiate(menuOption);
            attack.text = "Attack";
            menuOptions.Add(attack);
            menuElements.Add(MenuChoice.ATTACK);
        }
        if (moveDest.tileName.CompareTo("WARP PAD") == 0)
        {
            TextMeshProUGUI escape = Instantiate(menuOption);
            escape.text = "Escape";
            menuOptions.Add(escape);
            menuElements.Add(MenuChoice.ESCAPE);
        }
        if (moveDest.tileName.Equals("SEIZE") && selectedUnit.isLeader)
        {
            TextMeshProUGUI seize = Instantiate(menuOption);
            seize.text = "Seize";
            menuOptions.Add(seize);
            menuElements.Add(MenuChoice.SEIZE);
        }
        if (moveDest.tileName == "CHEST" && moveDest.hasLoot())
        {
            TextMeshProUGUI chest = Instantiate(menuOption);
            chest.text = "Chest";
            menuOptions.Add(chest);
            menuElements.Add(MenuChoice.CHEST);
        }
        if (selectedUnit.heldItem != null || selectedUnit.personalItem is UsableItem)
        {
            TextMeshProUGUI item = Instantiate(menuOption);
            item.text = "Item";
            menuOptions.Add(item);
            menuElements.Add(MenuChoice.ITEM);
        }
        if (selectedUnit.personalItem is Weapon || selectedUnit.heldWeapon != null)
        {
            TextMeshProUGUI weapon = Instantiate(menuOption);
            weapon.text = "Weapon";
            menuOptions.Add(weapon);
            menuElements.Add(MenuChoice.WEAPON);
        }
        if (moveDest.gemstones.Count > 0 && selectedUnit.heldItem == null)
        {
            TextMeshProUGUI gem = Instantiate(menuOption);
            gem.text = "Pick Up Gem";
            gem.fontSize = 18;
            menuOptions.Add(gem);
            menuElements.Add(MenuChoice.GEM);
        }

        TextMeshProUGUI wait = Instantiate(menuOption);
        wait.text = "Wait";
        menuOptions.Add(wait);
        menuElements.Add(MenuChoice.WAIT);

        menuOptions[0].color = Color.white;
        menuIdx = 0;

        for (int q = 0; q < menuOptions.Count; q++)
        {
            menuOptions[q].transform.SetParent(instantiatedMenuBackground.transform.GetChild(0), false);
            menuOptions[q].transform.position = new Vector3(menuOptions[q].transform.position.x,
                menuOptions[q].transform.position.y - (50 * q), menuOptions[q].transform.position.z);
        }
    }

    private void takeItem(Unit taker, Tile takeFrom)
    {
        takeFrom.GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage("open_chest.jpg");
        string note = takeFrom.takeLoot(taker);
        instantiatedItemNote = Instantiate(itemNote);
        instantiatedItemNote.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
        instantiatedItemNote.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, MAP_HUD_LAYER);
        timer = 2;
        selectionMode = SelectionMode.NOTIFICATION;
    }
    private void createForecast()
    {
        unfillAttackableTiles();
        targetTile = gridArray[cursorX, cursorY];
        targetEnemy = targetTile.getOccupant();
        Camera camCam = cam.GetComponent<Camera>();
        instantiatedForecastBackground = Instantiate(forecastBackground);

        int[] forecast = CombatManager.getBattleForecast(selectedUnit, targetEnemy,
            selectedUnit.getEquippedWeapon(), targetEnemy.getEquippedWeapon(),
            moveDest, targetTile, player, enemy);
        TextMeshProUGUI enemyName =
        instantiatedForecastBackground.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>();
        enemyName.text = targetEnemy.unitName;
        if (targetEnemy.getEquippedWeapon() != null)
        {
            enemyName.text += " (" + targetEnemy.getEquippedWeapon().itemName + ")";
        }
        TextMeshProUGUI playerName =
        instantiatedForecastBackground.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>();
        playerName.text = selectedUnit.unitName;
        if (selectedUnit.getEquippedWeapon() != null)
        {
            playerName.text += " (" + selectedUnit.getEquippedWeapon().itemName + ")";
        }

        TextMeshProUGUI playerData = instantiatedForecastBackground.transform.GetChild(0).GetChild(8).GetComponent<TextMeshProUGUI>();
        playerData.text = forecast[0] + "\n\n\n"
            + forecast[1] + " x " + (forecast[4] * forecast[5]) + "\n\n\n"
            + forecast[2] + "%\n\n\n"
            + forecast[3] + "%";

        TextMeshProUGUI enemyData = instantiatedForecastBackground.transform.GetChild(0).GetChild(9).GetComponent<TextMeshProUGUI>();
        enemyData.text = forecast[6] + "\n\n\n"
            + forecast[7] + " x " + (forecast[10] * forecast[11]) + "\n\n\n"
            + forecast[8] + "%\n\n\n"
            + forecast[9] + "%";

        selectionMode = SelectionMode.FORECAST;
    }

    private void fillAttackableTiles()
    {
        foreach (Tile t in interactableUnits)
        {
            t.attackHighlight.SetActive(false);
            t.interactHighlight.SetActive(false);
        }
        Dictionary<Tile, object> attackable = getAttackableBattlegroundTilesFromDestination(selectedUnit, moveDest);
        interactableUnits.Clear();
        foreach (Tile t in attackable.Keys)
        {
            t.attackHighlight.SetActive(true);
            interactableUnits.Add(t);
        }
    }
    private void unfillAttackableTiles()
    {
        foreach (Tile t in interactableUnits)
        {
            t.attackHighlight.SetActive(false);
            t.interactHighlight.SetActive(false);
        }
        interactableUnits.Clear();
    }



    private void addKeyAndValue(Dictionary<Tile, object> dict, Tile key, object val)
    {
        if (dict.ContainsKey(key))
        {
            dict.Remove(key);
        }
        dict.Add(key, val);
    }

    private void constructStatsPage()
    {
        cam.transform.position = new Vector3(0, 0, CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;
        instantiatedStatsPageBackground = Instantiate(statsPageBackground);
        instantiatedStatsPageBackground.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite
            = selectedUnit.GetComponent<SpriteRenderer>().sprite;
        instantiatedMapHUD.SetActive(false);
        if (selectedUnit.team == Unit.UnitTeam.PLAYER && selectedUnit.isEssential)
        {
            instantiatedStatsPageBackground.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
        }
        if (selectedUnit.team == Unit.UnitTeam.ENEMY)
        {
            instantiatedStatsPageBackground.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.yellow;
        } else if (selectedUnit.team == Unit.UnitTeam.ALLY)
        {
            instantiatedStatsPageBackground.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.cyan;
        }
        else if (selectedUnit.team == Unit.UnitTeam.OTHER)
        {
            instantiatedStatsPageBackground.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
        }

        Transform canvas = instantiatedStatsPageBackground.transform.GetChild(0);
        canvas.GetChild(8).GetComponent<TextMeshProUGUI>().text = selectedUnit.unitName;
        canvas.GetChild(9).GetComponent<TextMeshProUGUI>().text = selectedUnit.unitClass.className;
        canvas.GetChild(10).GetComponent<TextMeshProUGUI>().text = "LVL " + selectedUnit.level + " : " + selectedUnit.experience + " EXP";
        canvas.GetChild(11).GetComponent<TextMeshProUGUI>().text = "HP " + selectedUnit.currentHP + "/" + selectedUnit.maxHP;// + " (" + selectedUnit.hpGrowth + "% Growth)";
        canvas.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Strength: " + selectedUnit.strength// + " (" + selectedUnit.strengthGrowth + "% Growth)"
        + "\nMagic: " + selectedUnit.magic// + " (" + selectedUnit.magicGrowth + "% Growth)"
        + "\nSkill: " + selectedUnit.skill// + " (" + selectedUnit.skillGrowth + "% Growth)"
             + "\nSpeed: " + selectedUnit.speed// + " (" + selectedUnit.speedGrowth + "% Growth)"
             + "\nLuck: " + selectedUnit.luck// + " (" + selectedUnit.luckGrowth + "% Growth)"
             + "\nDefense: " + selectedUnit.defense// + " (" + selectedUnit.defenseGrowth + "% Growth)"
             + "\nResistance: " + selectedUnit.resistance;// + " (" + selectedUnit.resistanceGrowth + "% Growth)";
        canvas.GetChild(13).GetComponent<TextMeshProUGUI>().text = "MOVE: " + selectedUnit.movement + "\nCON: " + selectedUnit.constitution;
        canvas.GetChild(14).GetComponent<TextMeshProUGUI>().text = "ATK: " + selectedUnit.getAttackPower()
            + "\nACC: " + selectedUnit.getAccuracy()
            + "\nCRIT: " + selectedUnit.getCrit()
            + "\nAVO: " + selectedUnit.getAvoidance();
        if (selectedUnit.personalItem == null)
        {
            canvas.GetChild(15).GetComponent<TextMeshProUGUI>().text = "";
            canvas.GetChild(16).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            canvas.GetChild(15).GetComponent<TextMeshProUGUI>().text = selectedUnit.personalItem.itemName;
            if (selectedUnit.equipped == 0)
            {
                canvas.GetChild(15).GetComponent<TextMeshProUGUI>().text += " (E)";
            }
            canvas.GetChild(16).GetComponent<TextMeshProUGUI>().text = selectedUnit.personalItem.description();
        }
        if (selectedUnit.heldWeapon == null)
        {
            canvas.GetChild(17).GetComponent<TextMeshProUGUI>().text = "";
            canvas.GetChild(18).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            canvas.GetChild(17).GetComponent<TextMeshProUGUI>().text = selectedUnit.heldWeapon.itemName;
            if (selectedUnit.equipped == 1)
            {
                canvas.GetChild(17).GetComponent<TextMeshProUGUI>().text += " (E)";
            }
            canvas.GetChild(18).GetComponent<TextMeshProUGUI>().text = selectedUnit.heldWeapon.description();
        }
        if (selectedUnit.heldItem == null)
        {
            canvas.GetChild(19).GetComponent<TextMeshProUGUI>().text = "";
            canvas.GetChild(20).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            canvas.GetChild(19).GetComponent<TextMeshProUGUI>().text = selectedUnit.heldItem.itemName;
            canvas.GetChild(20).GetComponent<TextMeshProUGUI>().text = selectedUnit.heldItem.description();
        }
        canvas.GetChild(21).GetComponent<TextMeshProUGUI>().text = Weapon.weaponTypeName(selectedUnit.weaponType) + ": " + selectedUnit.proficiency;
    }

    private void deconstructStatsPage()
    {
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = cameraOrthographicSize;
        setCameraPosition(cursorX, cursorY);
        instantiatedMapHUD.SetActive(true);

        Destroy(instantiatedStatsPageBackground);
    }

    private void performTalk()
    {
        cam.transform.position = new Vector3(0, 0, CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;
        instantiatedBattleBackground.GetComponent<SpriteRenderer>().size = new Vector2(camCam.orthographicSize * camCam.aspect * 2, camCam.orthographicSize * 2);
        instantiatedBattleBackground.SetActive(true);
        instantiatedMapHUD.SetActive(false);

        instantiatedDialogueBox = Instantiate(dialogueBox);
        instantiatedSpeakerPortraits = new List<GameObject>();
        for (int q = 0; q < 5; q++)
        {
            instantiatedSpeakerPortraits.Add(Instantiate(speakerPortrait));
        }
        instantiatedSpeakerPortraits[0].transform.Translate(new Vector3(-16, 0, 0));
        instantiatedSpeakerPortraits[1].transform.Translate(new Vector3(16, 0, 0));
        instantiatedSpeakerPortraits[2].transform.Translate(new Vector3(-32, 0, 0));
        instantiatedSpeakerPortraits[3].transform.Translate(new Vector3(32, 0, 0));
        //        instantiatedSpeakerPortraits[4].transform.Translate(new Vector3(0, 0, 0));
        foreach (GameObject g in instantiatedSpeakerPortraits)
        {
            g.GetComponent<SpriteRenderer>().sprite = null;
        }

        currentDialogue = talkerTile.getOccupant().talkConvo;

        nextSaying();
    }

    private bool nextSaying()
    {
        List<string> component = currentDialogue.nextDialogueComponent();
        if (component == null)
        {
            return false;
        }
        int idx = 0;
        while (char.IsNumber(component[idx][0]))
        {
            int command = component[idx][0] - '0';
            string pic = component[idx].Substring(2);
            if (command == 0)
            {
                if (pic.Equals("clear"))
                {
                    instantiatedBattleBackground.GetComponent<SpriteRenderer>().sprite = null;
                }
                else
                {
                    instantiatedBattleBackground.GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage(pic);
                }
            } else if (command == 9)
            {
                if (pic.Equals("leave"))
                {
                    enemy.Remove(talkerTile.getOccupant());
                    ally.Remove(talkerTile.getOccupant());
                    talkerTile.getOccupant().setTalkIcon(null);
                    Destroy(talkerTile.getOccupant().outline);
                    Destroy(talkerTile.getOccupant().GetComponent<SpriteRenderer>());
                    talkerTile.setOccupant(null);
                }
                else if (pic.Equals("give"))
                {
                    if (talkerTile.getOccupant().talkReward is Weapon && selectedUnit.heldWeapon == null)
                    {
                        selectedUnit.heldWeapon = (Weapon)talkerTile.getOccupant().talkReward.clone();
                    } else if (!(talkerTile.getOccupant().talkReward is Weapon) && selectedUnit.heldItem == null)
                    {
                        selectedUnit.heldItem = talkerTile.getOccupant().talkReward.clone();
                    }
                    else
                    {
                        CampaignData.addToConvoy(talkerTile.getOccupant().talkReward.clone());
                    }
                } else if (pic.Equals("giveall"))
                {
                    foreach (Unit u in player)
                    {
                        //TODO make these clones of the item
                        if (talkerTile.getOccupant().talkReward is Weapon && u.heldWeapon == null)
                        {
                            u.heldWeapon = (Weapon)talkerTile.getOccupant().talkReward.clone();
                            Debug.Log("Weapon " + u.heldWeapon.might + "," + u.heldWeapon.hit);
                        }
                        else if (!(talkerTile.getOccupant().talkReward is Weapon) && u.heldItem == null)
                        {
                            u.heldItem = talkerTile.getOccupant().talkReward.clone();
                            Debug.Log("Item");
                        }
                        else
                        {
                            CampaignData.addToConvoy(talkerTile.getOccupant().talkReward.clone());
                        }
                    }
                }
                else if (pic.Equals("join"))
                {
                    enemy.Remove(talkerTile.getOccupant());
                    ally.Remove(talkerTile.getOccupant());
                    player.Add(talkerTile.getOccupant());
                    talkerTile.getOccupant().setTalkIcon(null);
                    talkerTile.getOccupant().outline.color = Color.blue;
                    talkerTile.getOccupant().team = Unit.UnitTeam.PLAYER;
                    CampaignData.members.Add(talkerTile.getOccupant());

                }
            } else //Only 1-5
            {
                if (pic.Equals("clear"))
                {
                    instantiatedSpeakerPortraits[command - 1].GetComponent<SpriteRenderer>().sprite = null;
                }
                else
                {
                    instantiatedSpeakerPortraits[command - 1].GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage(pic);
                    instantiatedSpeakerPortraits[command - 1].GetComponent<SpriteRenderer>().size = new Vector3(1, 1);
                }
            }
            idx++;
        }
        TextMeshProUGUI speakerName = instantiatedDialogueBox.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI dialogueText = instantiatedDialogueBox.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        speakerName.text = component[idx].Equals("-") ? "" : component[idx];
        dialogueText.text = component[idx + 1].Equals("-") ? "" : component[idx + 1];
        return true;
    }

    private void deconstructConversation()
    {
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = cameraOrthographicSize;
        setCameraPosition(cursorX, cursorY);
        instantiatedMapHUD.SetActive(true);

        instantiatedBattleBackground.SetActive(false);
        Destroy(instantiatedDialogueBox);
        foreach (GameObject ob in instantiatedSpeakerPortraits)
        {
            Destroy(ob);
        }
        instantiatedSpeakerPortraits.Clear();
    }

    private void performBattle(Unit plyr, Unit enem, Weapon plyrWep, Weapon enemWep, Tile plyrTile,
        Tile enemTile, bool playerTurn)
    {
        cam.transform.position = new Vector3(0, 0, CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;
        instantiatedBattleBackground.GetComponent<SpriteRenderer>().size = new Vector2(camCam.orthographicSize * camCam.aspect * 2, camCam.orthographicSize * 2);
        instantiatedBattleBackground.SetActive(true);
        instantiatedMapHUD.SetActive(false);

        instantiatedBattleUI = Instantiate(battleUI);
        int[] forecast;
        if (playerTurn) {
            forecast = CombatManager.getBattleForecast(plyr, enem, plyrWep, enemWep,
                plyrTile, enemTile, player, enemy);
        } else
        {
            forecast = CombatManager.getBattleForecast(enem, plyr, enemWep, plyrWep,
                enemTile, plyrTile, enemy, player);
        }
        Transform canvas = instantiatedBattleUI.transform.GetChild(1);
        canvas.GetChild(0).GetComponent<TextMeshProUGUI>().text = enem.unitName;
        canvas.GetChild(1).GetComponent<TextMeshProUGUI>().text = plyr.unitName;
        canvas.GetChild(2).GetComponent<TextMeshProUGUI>().text = "HIT: " + forecast[8] + "\nDMG: " + forecast[7] + "\nCRIT: " + forecast[9];
        canvas.GetChild(3).GetComponent<TextMeshProUGUI>().text = "HIT: " + forecast[2] + "\nDMG: " + forecast[1] + "\nCRIT: " + forecast[3];
        if (!playerTurn)
        {
            Vector3 temp = canvas.GetChild(2).position;
            canvas.GetChild(2).position = canvas.GetChild(3).position;
            canvas.GetChild(3).position = temp;
        }
        canvas.GetChild(4).GetComponent<TextMeshProUGUI>().text = enem.currentHP + "/" + enem.maxHP;
        canvas.GetChild(5).GetComponent<TextMeshProUGUI>().text = plyr.currentHP + "/" + plyr.maxHP;
        canvas.GetChild(6).GetComponent<TextMeshProUGUI>().text = enem.getEquippedWeaponName();
        canvas.GetChild(7).GetComponent<TextMeshProUGUI>().text = plyr.getEquippedWeaponName();

        battleEnemyTile = Instantiate(battleTile);
        battlePlayerTile = Instantiate(battleTile);
        battlePlayerTile.transform.Translate((float)42.5, 0, 0);
        battleEnemyTile.GetComponent<SpriteRenderer>().sprite = enemTile.GetComponent<SpriteRenderer>().sprite;
        battlePlayerTile.GetComponent<SpriteRenderer>().sprite = plyrTile.GetComponent<SpriteRenderer>().sprite;

        battleCombatantEnemy = Instantiate(battleCombatant);
        battleCombatantPlayer = Instantiate(battleCombatant);
        battleCombatantPlayer.transform.Translate((float)42.5, 0, 0);
        battleCombatantEnemy.GetComponent<SpriteRenderer>().sprite = enem.GetComponent<SpriteRenderer>().sprite;
        battleCombatantPlayer.GetComponent<SpriteRenderer>().sprite = plyr.GetComponent<SpriteRenderer>().sprite;

        int playerDMG;
        int enemyDMG;
        int[] battleResults = CombatManager.decideBattle(forecast);
        if (playerTurn)
        {
            playerDMG = forecast[1];
            enemyDMG = forecast[7];
        } else
        {
            playerDMG = forecast[7];
            enemyDMG = forecast[1];
        }
        attackParts = new List<AttackComponent>();
        int plyrWepDamage = 0;
        int enemWepDamage = 0;
        for (int q = 0; q < battleResults.Length; q += 3)
        {
            if ((playerTurn && battleResults[q] == 0) || (!playerTurn && battleResults[q] == 1)) //If player is attacking
            {
                if (plyrWep != null && plyrWep.uses > 0 && plyrWep.usesLeft <= plyrWepDamage)
                {
                    continue;
                }
                attackParts.Add(new AttackComponent(battleCombatantPlayer, battleCombatantEnemy.transform.position.x + 10,
                    battleCombatantEnemy, plyr, plyrWep, enem, canvas.GetChild(4).GetComponent<TextMeshProUGUI>(), playerDMG,
                    battleResults[q + 1], enemTile));
                plyrWepDamage++;
            } else
            {
                if (enemWep != null && enemWep.uses > 0 && enemWep.usesLeft <= enemWepDamage)
                {
                    continue;
                }
                attackParts.Add(new AttackComponent(battleCombatantEnemy, battleCombatantPlayer.transform.position.x - 10,
                    battleCombatantPlayer, enem, enemWep, plyr, canvas.GetChild(5).GetComponent<TextMeshProUGUI>(), enemyDMG,
                    battleResults[q + 1], plyrTile));
                enemWepDamage++;
            }
        }
        attackMode = 0;
        attackPartIdx = 0;
        keepBattling = true;
        brokenWeapon = null;
        expToAdd = 0;
        findBattleEndStart = true;
        //TODO
    }

    private void deconstructBattle()
    {
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = cameraOrthographicSize;
        setCameraPosition(cursorX, cursorY);
        instantiatedMapHUD.SetActive(true);

        instantiatedBattleBackground.SetActive(false);
        Destroy(instantiatedBattleUI);
        Destroy(battleEnemyTile);
        Destroy(battlePlayerTile);
        Destroy(battleCombatantEnemy);
        Destroy(battleCombatantPlayer);
        attackParts.Clear();

        if (selectionMode == SelectionMode.BATTLE)
        {
            selectionMode = SelectionMode.ROAM;
        }
        else if (selectionMode == SelectionMode.ENEMYPHASE_ATTACK)
        {
            selectionMode = SelectionMode.ENEMYPHASE_SELECT_UNIT;
        } else if (selectionMode == SelectionMode.ALLYPHASE_ATTACK)
        {
            selectionMode = SelectionMode.ALLYPHASE_SELECT_UNIT;
        }

    }

    private class AttackComponent
    {
        public GameObject atkSprite;
        public float xDest;
        public GameObject dfdSprite;
        public Unit atkUnit;
        public Weapon atkWeapon;
        public Unit dfdUnit;
        public TextMeshProUGUI dfdHPText;
        public int damage;
        public int hitStatus;
        public Tile dfdTile;
        public float startPos;
        public int direction;
        public AttackComponent(GameObject atkSprite, float xDest, GameObject dfdSprite, Unit atkUnit,
            Weapon atkWeapon, Unit dfdUnit, TextMeshProUGUI dfdHPText, int damage, int hitStatus, Tile dfdTile)
        {
            this.atkSprite = atkSprite;
            this.xDest = xDest;
            this.dfdSprite = dfdSprite;
            this.atkUnit = atkUnit;
            this.atkWeapon = atkWeapon;
            this.dfdUnit = dfdUnit;
            this.dfdHPText = dfdHPText;
            this.damage = damage;
            this.hitStatus = hitStatus;
            this.dfdTile = dfdTile;
            this.startPos = atkSprite.transform.position.x;
            this.direction = (atkSprite.transform.position.x > xDest) ? -1 : 1;
        }

    }

    public object[] performUnitAI(Unit u)
    {
        int[] startCoords = findUnit(u);
        List<Unit> uAllies = player.Contains(u) ? player : ally.Contains(u) ? ally
            : enemy.Contains(u) ? enemy : other.Contains(u) ? other : null;
        //TODO fix the bug where findUnit() sometimes returns null
        if (startCoords == null)
        {
            return null;
        }
        List<Tile> target = testAISuccess(u, startCoords, u.ai1);
        if (target.Count == 0)
        {
            target = testAISuccess(u, startCoords, u.ai2);
            if (target.Count == 0)
            {
                return null;
            }
            object[] retVal = new object[4];
            retVal[0] = u.ai2;
            actOnUnitAI(u, startCoords, u.ai2, target, retVal, uAllies);
            return retVal;
        }
        object[] ret = new object[4];
        ret[0] = u.ai1;
        actOnUnitAI(u, startCoords, u.ai1, target, ret, uAllies);
        return ret;
    }
    public int[] findUnit(Unit u)
    {
        for (int q = 0; q < width; q++)
        {
            for (int w = 0; w < height; w++)
            {
                if (gridArray[q, w].getOccupant() == u)
                {
                    return new int[] { q, w };
                }
            }
        }
        //		System.out.println("Didn't find");
        return null;
    }
    public List<Tile> testAISuccess(Unit u, int[] start, Unit.AIType ai)
    {
        List<Tile> ret = new List<Tile>();
        Tile startTile = gridArray[start[0], start[1]];
        if (ai == Unit.AIType.ATTACK)
        {
            Dictionary<Tile, object> traversable = getTraversableTiles(u, startTile.x, startTile.y);
            Dictionary<Tile, object>.KeyCollection dests = traversable.Keys;
            foreach (Tile dest in dests)
            {
                if (dest.isVacant() || dest.getOccupant() == u)
                {
                    Dictionary<Tile, object> att = getAttackableBattlegroundTilesFromDestination(u, dest);
                    List<Tile> realAtt = getAttackableTilesWithEnemies(att, u);
                    if (realAtt.Count != 0)
                    {
                        ret.Add(dest);
                    }
                }
            }
        }
        else if (ai == Unit.AIType.BURN)
        {
            //TODO if there is a path to a house, add that house's tile
        }
        else if (ai == Unit.AIType.GUARD)
        {
            Dictionary<Tile, object> attackable = getAttackableBattlegroundTilesFromDestination(u, startTile);
            List<Tile> enemyTiles = getAttackableTilesWithEnemies(attackable, u);
            foreach (Tile t in enemyTiles)
            {
                ret.Add(t);
            }
        }
        else if (ai == Unit.AIType.PURSUE)
        {
            //TODO if there is a path to an enemy, add that enemy's tile
        }

        return ret;
    }
    private void actOnUnitAI(Unit u, int[] start, Unit.AIType ai,
            List<Tile> target, object[] report, List<Unit> uAllies)
    {
        Tile startTile = gridArray[start[0], start[1]];
        if (ai == Unit.AIType.ATTACK)
        {
            //TODO
            int heur = int.MinValue;
            Tile bestDest = null;
            Tile best = null;
            for (int q = 0; q < target.Count; q++)
            {
                Tile dest = target[q];
                Dictionary<Tile, object> att = getAttackableBattlegroundTilesFromDestination(u, dest);
                List<Tile> enemTiles = getAttackableTilesWithEnemies(att, u);
                for (int r = 0; r < enemTiles.Count; r++)
                {
                    Tile dfdTile = enemTiles[r];
                    int specialHeur = int.MinValue;
                    int heldHeur = int.MinValue;
                    Unit enem = dfdTile.getOccupant();
                    int dist = Mathf.Abs(dfdTile.x - dest.x) + Mathf.Abs(dfdTile.y - dest.y);
                    if (u.personalItem is Weapon)
                    {
                        Weapon w = (Weapon)u.personalItem;
                        if (w.maxRange >= dist && dist <= w.minRange)
                        {
                            specialHeur = 0;
                            List<Unit> enemAllies = player.Contains(enem) ? player : ally.Contains(enem) ? ally
                                : enemy.Contains(enem) ? enemy : other.Contains(enem) ? other : null;
                            int[] forecast = CombatManager.getBattleForecast(u, enem, w,
                                    enem.getEquippedWeapon(), dest, dfdTile, uAllies, enemAllies);
                            if (forecast[1] * forecast[5] >= forecast[6])
                            {
                                specialHeur += 50;
                            }
                            else
                            {
                                int bonus = (int)Mathf.Round((float)(forecast[1] * forecast[2] / 100.0));
                                specialHeur += Mathf.Min(40, bonus);
                            }
                            specialHeur += Mathf.Max(0, 20 - forecast[6]);
                            if (forecast[10] == 0)
                            {
                                specialHeur += 10;
                            }
                            else
                            {
                                int penalty = (int)Mathf.Round((float)(forecast[7] * forecast[8] / 100.0));
                                specialHeur -= Mathf.Min(40, penalty);
                            }
                            specialHeur -= Mathf.Max(0, 20 - (forecast[0] - forecast[7]));
                        }
                    }
                    if (u.heldWeapon != null)
                    {
                        Weapon w = (Weapon)u.heldWeapon;
                        if (w.maxRange >= dist && dist <= w.minRange)
                        {
                            heldHeur = 0;
                            List<Unit> enemAllies = player.Contains(enem) ? player : ally.Contains(enem) ? ally
                                : enemy.Contains(enem) ? enemy : other.Contains(enem) ? other : null;
                            int[] forecast = CombatManager.getBattleForecast(u, enem, w,
                                    enem.getEquippedWeapon(), dest, dfdTile, uAllies, enemAllies);
                            if (forecast[1] * forecast[5] >= forecast[6])
                            {
                                heldHeur += 50;
                            }
                            else
                            {
                                int bonus = (int)Mathf.Round((float)(forecast[1] * forecast[2] / 100.0));
                                heldHeur += Mathf.Min(40, bonus);
                            }
                            heldHeur += Mathf.Max(0, 20 - forecast[6]);
                            if (forecast[10] == 0)
                            {
                                heldHeur += 10;
                            }
                            else
                            {
                                int penalty = (int)Mathf.Round((float)(forecast[7] * forecast[8] / 100.0));
                                heldHeur -= Mathf.Min(40, penalty);
                            }
                            heldHeur -= Mathf.Max(0, 20 - (forecast[0] - forecast[7]));
                        }
                    }
                    if (specialHeur > heur)
                    {
                        heur = specialHeur;
                        best = dfdTile;
                        u.equipSpecial();
                        bestDest = dest;
                    }
                    if (heldHeur > heur)
                    {
                        heur = heldHeur;
                        best = dfdTile;
                        u.equipHeld();
                        bestDest = dest;
                    }
                }
            }

            report[1] = startTile;
            report[2] = bestDest;
            report[3] = best;
        }
        else if (ai == Unit.AIType.BURN)
        {
            //TODO move closer to the house or burn it if possible
        }
        else if (ai == Unit.AIType.GUARD)
        {
            int heur = int.MinValue;
            int best = 0;
            for (int q = 0; q < target.Count; q++)
            {
                Unit enem = target[q].getOccupant();
                Weapon specialWep = null;
                Weapon heldWep = null;
                int specialHeur = 0;
                int heldHeur = 0;
                if (u.personalItem is Weapon)
                {
                    specialWep = (Weapon)u.personalItem;
                }
                if (u.heldWeapon != null)
                {
                    heldWep = (Weapon)u.heldWeapon;
                }
                List<Unit> enemAllies = player.Contains(enem) ? player : ally.Contains(enem) ? ally
                    : enemy.Contains(enem) ? enemy : other.Contains(enem) ? other : null;
                int[] w1Forecast = CombatManager.getBattleForecast(u, enem, specialWep,
                        enem.getEquippedWeapon(), startTile, target[q], uAllies, enemAllies);
                int[] w2Forecast = CombatManager.getBattleForecast(u, enem, heldWep,
                        enem.getEquippedWeapon(), startTile, target[q], uAllies, enemAllies);
                if (w1Forecast[1] * w1Forecast[5] >= w1Forecast[6])
                {
                    specialHeur += 50;
                }
                else
                {
                    int bonus = (int)Mathf.Round((float)(w1Forecast[1] * w1Forecast[2] / 100.0));
                    specialHeur += Mathf.Min(40, bonus);
                }
                if (w2Forecast[1] * w2Forecast[4] * w2Forecast[5] >= w2Forecast[6])
                {
                    heldHeur += 50;
                }
                else
                {
                    int bonus = (int)Mathf.Round((float)(w2Forecast[1] * w2Forecast[2] / 100.0));
                    heldHeur += Mathf.Min(40, bonus);
                }
                specialHeur += Mathf.Max(0, 20 - w1Forecast[6]);
                heldHeur += Mathf.Max(0, 20 - w2Forecast[6]);
                if (w1Forecast[10] == 0)
                {
                    specialHeur += 10;
                }
                else
                {
                    int penalty = (int)Mathf.Round((float)(w1Forecast[7] * w1Forecast[8] / 100.0));
                    specialHeur -= Mathf.Min(40, penalty);
                }
                if (w2Forecast[10] == 0)
                {
                    heldHeur += 10;
                }
                else
                {
                    int penalty = (int)Mathf.Round((float)(w2Forecast[7] * w2Forecast[8] / 100.0));
                    heldHeur -= Mathf.Min(40, penalty);
                }
                specialHeur -= Mathf.Max(0, 20 - (w1Forecast[0] - w1Forecast[7]));
                heldHeur -= Mathf.Max(0, 20 - (w2Forecast[0] - w2Forecast[7]));

                if (specialHeur > heur)
                {
                    heur = specialHeur;
                    best = q;
                    if (specialWep != null)
                    {
                        u.equipSpecial();
                    }
                    else
                    {
                        u.equipNone();
                    }
                }
                if (heldHeur > heur)
                {
                    heur = heldHeur;
                    best = q;
                    if (heldWep != null)
                    {
                        u.equipHeld();
                    }
                    else
                    {
                        u.equipNone();
                    }
                }
            }
            Tile enemyTile = target[best];
            report[1] = startTile;
            report[2] = startTile; //Because uniformity is good
            report[3] = enemyTile;
        }
        else if (ai == Unit.AIType.PURSUE)
        {
            //TODO move closer to the enemy or attack if possible
        }
    }

    public override bool completed()
    {
        return objectiveComplete;
    }


    public enum SelectionMode
    {
        ROAM, MOVE, MENU, SELECT_ENEMY, SELECT_TALKER, SELECT_WEAPON, FORECAST, BATTLE, MAP_MENU, IN_CONVO,
        SELECT_GEM, STATUS, STATS_PAGE, CONTROLS, NOTIFICATION, ITEM_MENU, SELECT_TRADER, SELECT_WEAPON_TRADER,
        ENEMYPHASE_SELECT_UNIT, ENEMYPHASE_MOVE, ENEMYPHASE_ATTACK, ENEMYPHASE_BURN, ENEMYPHASE_COMBAT_PAUSE,
        ALLYPHASE_SELECT_UNIT, ALLYPHASE_MOVE, ALLYPHASE_ATTACK, ALLYPHASE_BURN, ALLYPHASE_COMBAT_PAUSE,
        STANDBY, GAMEOVER, ESCAPE_MENU
    }

    public enum MenuChoice
    {
        TALK, ATTACK, ESCAPE, SEIZE, ITEM, WEAPON, GEM, CHEST, WAIT, STATUS, CONTROLS, END,
        USE_PERSONAL, USE_HELD, TRADE, DROP, EQUIP_PERSONAL, EQUIP_HELD, EQUIP_NONE, TRADE_WEAPON,
        DROP_WEAPON
    }

    public static float BACKGROUND_LAYER = 0;
    public static float TILE_LAYER = (float)-0.5;
    public static float TILE_HIGHLIGHT_LAYER = (float)-0.6;
    public static float UNIT_OUTLINE_LAYER = (float)-0.7;
    public static float UNIT_SPRITE_LAYER = (float)-0.8;
    public static float MOVE_UNIT_OUTLINE_LAYER = (float)-0.9;
    public static float MOVE_UNIT_SPRITE_LAYER = (float)-1.0;
    public static float CURSOR_LAYER = (float)-2;
    public static float MENU_BACKGROUND_LAYER = (float)-2.1;
    public static float MENU_ELEMENTS_LAYER = (float)-2.2;
    public static float MAP_HUD_LAYER = (float)-3;
    public static float BATTLE_BACKGROUND_LAYER = (float)-5;
    public static float BATTLE_TILE_LAYER = (float)-5.05;
    public static float BATTLE_COMBATANTS_LAYER = (float)-5.1; //Also for talking portraits
    public static float BATTLE_UI_ELEMENTS_LAYER = (float)-5.2; //Also for Dialogue Box
    public static float BATTLE_UI_TEXT_LAYER = (float)-5.3; //Also for dialogue text
    public static float EXPERIENCE_PANE_LAYER = (float)-5.5;
    public static float EXPERIENCE_TEXT_LAYER = (float)-5.6;
    public static float LEVEL_BACKGROUND_LAYER = (float)-5.7;
    public static float LEVEL_TEXT_LAYER = (float)-5.8;
    public static float SPECIAL_MENU_LAYER = (float)-9;
    public static float CAMERA_LAYER = (float)-10;
}
