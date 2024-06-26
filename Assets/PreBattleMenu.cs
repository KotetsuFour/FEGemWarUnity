using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PreBattleMenu : SequenceMember
{

    private int width;
    private int height;
    private float cellSize = 2;
    private Unit instantiatableUnit; //For loading saves
    public int cameraOrthographicSize = 6;

    public List<int> player;
    public List<Unit> enemy;
    public List<Unit> ally;
    public List<Unit> other;

    private Objective objective;
    private string chapterName;

    private Tile tile;

    private Tile[,] gridArray;
    private List<Tile> playerTiles;

    public GameObject cursor;
    public GameObject cam;

    public int cursorX;
    public int cursorY;

    public GameObject pickerBackground;
    public int pickCursorX;
    public int pickCursorY;
    public int pickerWidth;
    public int pickerHeight;
    public GameObject[,] pickerArray;
    public Unit[,] pickerUnits;
    public GameObject deployerPanel;
    private GameObject pickerCursor;
    public float pickerCellSizeX = 23;
    public float pickerCellSizeY = (float)9;
    private float pickerOffsetX;
    private float pickerOffsetY;

    public SelectionMode selectionMode = SelectionMode.MAIN_MENU;

    public Dictionary<Tile, object> traversableTiles;
    public Dictionary<Tile, object> attackableTiles;
    public Tile selectedTile;
    public Unit selectedUnit;

    public GameObject statsPageBackground;
    private GameObject instantiatedStatsPageBackground;

    public GameObject mapHUD;
    private GameObject instantiatedMapHUD;
    private TextMeshProUGUI objectiveText;
    private TextMeshProUGUI tileInfoText;

    private Vector3 unusedPosition = new Vector3(-10, -10, GridMap.UNIT_SPRITE_LAYER);

    private bool done = false;

    public GameObject mainMenu;
    private GameObject instantiatedMainMenu;
    private int mainMenuIdx;

    public SaveScreen saveScreen;
    private SaveScreen instantiatedSaveScreen;

    public GameObject itemsMenu;
    private GameObject instantiatedItemsMenu;
    public TextMeshProUGUI scrollListMember;
    private List<TextMeshProUGUI> unitScrollListMembers;
    private List<TextMeshProUGUI> itemScrollListMembers;
    private List<int> currentItemIds;
    private int itemTypeIdx;
    private int itemIdx;
    private int inventoryIdx;
    private int unitForConvoyIdx;

    public void constructor(string input, string positions, Tile tile, Sprite[] tileSprites,
        int[] playerUnits, Unit[] enemyUnits, Unit[] allyUnits, Unit[] otherUnits, GameObject cam,
        Objective objective, string chapterName, Unit instantUnit)
    {
        instantiatableUnit = instantUnit;
        this.tile = tile;
        player = new List<int>(playerUnits);
        this.enemy = new List<Unit>();
        this.ally = new List<Unit>();
        this.other = new List<Unit>();
        this.objective = objective;
        this.chapterName = chapterName;

        this.playerTiles = new List<Tile>();

        instantiatedMapHUD = Instantiate(mapHUD);
        objectiveText = instantiatedMapHUD.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        tileInfoText = instantiatedMapHUD.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();

        instantiatedMapHUD.SetActive(false);

        objectiveText.text = objective.getName();
        if (objectiveText.text.Length >= 27)
        {
            objectiveText.fontSize = 15;
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

        if (posList.Count != counterList.Count || posList[0].Count != counterList[0].Count)
        {
            Debug.LogError("Tile and positions maps are not the same size");
            return;
        }

        cursor = Instantiate(cursor);
        cursor.GetComponent<SpriteRenderer>().size = new Vector2(cellSize, cellSize);
        cursor.SetActive(true);

        this.width = counterList[0].Count;
        this.height = counterList.Count;
        gridArray = new Tile[width, height];
        int playerIdx = 0;
        int enemyIdx = 0;
        int allyIdx = 0;
        int otherIdx = 0;
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
                }
                else if (c == 'R')
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
                }
                else if (c == '~')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[4];
                    toPut.setValues("DEEP WATER", int.MaxValue, 1, 0);
                }
                else if (c == 'W')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[5];
                    toPut.setValues("WALL", int.MaxValue, 1, 0);
                }
                else if (c == 'e')
                {
                    //TODO add loot
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[6];
                    toPut.setValues("CHEST", 1, 4, 0);
                }
                else if (c == 'T')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[7];
                    toPut.setValues("SEIZE POINT", 1, 4, 20);
                }
                else if (c == '+')
                {
                    toPut.GetComponent<SpriteRenderer>().sprite = tileSprites[8];
                    toPut.setValues("HEAL TILE", 1, 1, 20);
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

                if (posList[q][w] == '*')
                {
                    playerTiles.Add(toPut);
                    if (playerIdx < player.Count && player[playerIdx] != -1) {
                        CampaignData.members[player[playerIdx]].deployed = true;
                        toPut.setOccupant(CampaignData.members[player[playerIdx]]);
                        if (toPut.getOccupant().isLeader)
                        {
                            cursorX = w;
                            cursorY = q;
                            cursor.transform.position = getWorldPosition(cursorX, cursorY, GridMap.CURSOR_LAYER);
                        }
                    }
                    playerIdx++;
                }
                else if (posList[q][w] == 'x' && enemyIdx < enemy.Count)
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
            }
        }
        foreach (Unit u in CampaignData.members)
        {
            if (!u.deployed)
            {
                u.transform.position = unusedPosition;
            }
        }

        pickerWidth = 3;
        pickerHeight = (CampaignData.members.Count / 3) + (CampaignData.members.Count % 3 == 0 ? 0 : 1);
        pickerArray = new GameObject[pickerWidth, pickerHeight];
        pickerUnits = new Unit[pickerWidth, pickerHeight];
        for (int q = 0; q < CampaignData.members.Count; q++)
        {
            Unit u = CampaignData.members[q];
            pickerUnits[q % pickerWidth, q / pickerWidth] = u;
            GameObject panel = Instantiate(deployerPanel);
            pickerOffsetX = panel.transform.position.x;
            pickerOffsetY = panel.transform.position.y;
            panel.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage(u.spriteName);
            if (!u.isAlive())
            {
                panel.GetComponent<SpriteRenderer>().color = Color.black;
            }
            else if (u.deployed)
            {
                panel.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage("checkmark.png");
                panel.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                panel.transform.GetChild(1).GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
            }
//            panel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = u.unitName;
            panel.transform.Translate((q % pickerWidth) * pickerCellSizeX, -(q / pickerWidth) * pickerCellSizeY, 0);
//            panel.transform.GetChild(2).GetChild(0).Translate((q % pickerWidth) * 250, -(q / pickerWidth) * 90, 0);
            panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y, GridMap.BATTLE_TILE_LAYER);
            pickerArray[q % pickerWidth, q / pickerWidth] = panel;
        }
        pickerCursor = Instantiate(cursor);
        pickerCursor.GetComponent<SpriteRenderer>().size = new Vector2(pickerCellSizeX, pickerCellSizeY);
        pickerCursor.transform.position = new Vector3(pickerArray[0, 0].transform.position.x, pickerArray[0, 0].transform.position.y, GridMap.BATTLE_UI_ELEMENTS_LAYER);
        deconstructPickUnits();

        pickerBackground = Instantiate(pickerBackground);
        pickerBackground.SetActive(false);

        tileInfoText.text = "Tile: " + gridArray[cursorX, cursorY].tileName
            + "\nAvoid Bonus: " + gridArray[cursorX, cursorY].avoidBonus;

        toMainMenu();
    }

    private void toMainMenu()
    {
        instantiatedMainMenu = Instantiate(mainMenu);
        mainMenuIdx = 0;
        instantiatedMainMenu.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        setMainMenuInfo();
        instantiatedMainMenu.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text
            = chapterName;
        instantiatedMainMenu.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text
            = objective.getName();

        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        selectionMode = SelectionMode.MAIN_MENU;
    }
    private void setMainMenuInfo()
    {
        if (mainMenuIdx == 0)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Pick a limited number of units to deploy";
        }
        else if (mainMenuIdx == 1)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Give units weapons and items";
        }
        else if (mainMenuIdx == 2)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Look at the map to change your battle positions and plan your strategy";
        }
        else if (mainMenuIdx == 3)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Save your progress along with your units' inventories and battle positions";
        }
        else if (mainMenuIdx == 4)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Begin the battle! REMEMBER TO SAVE FIRST!";
        }
        else if (mainMenuIdx == 5)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = "Exit the chapter. REMEMBER TO SAVE FIRST!";
        }
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

    private void setCameraPosition(int x, int y)
    {
        Camera camCam = cam.GetComponent<Camera>();
        float pos75x = (width * cellSize) - (camCam.orthographicSize * camCam.aspect / 2);
        float pos25x = camCam.orthographicSize * camCam.aspect / 2;
        float pos75y = (height * cellSize) - (camCam.orthographicSize / 2);
        float pos25y = camCam.orthographicSize / 2;
        cam.transform.position = new Vector3(Mathf.Max(pos25x, Mathf.Min(pos75x, x * cellSize)),
            Mathf.Max(pos25y, Mathf.Min(pos75y, y * cellSize)), GridMap.CAMERA_LAYER);
        instantiatedMapHUD.transform.position = new Vector3((float)(cam.transform.position.x - 7.5), cam.transform.position.y + 6, GridMap.MAP_HUD_LAYER);
    }
    private void updateCursor()
    {
        cursor.transform.position = new Vector3(cursorX * cellSize, cursorY * cellSize, GridMap.CURSOR_LAYER);
        tileInfoText.text = "Tile: " + gridArray[cursorX, cursorY].tileName
            + "\nAvoid Bonus: " + gridArray[cursorX, cursorY].avoidBonus;
        updateCameraPosition();
    }
    private void updatePickCursor()
    {
        pickerCursor.transform.position = new Vector3(pickerOffsetX + (pickCursorX * pickerCellSizeX),
            pickerOffsetY - (pickCursorY * pickerCellSizeY),
            GridMap.BATTLE_UI_ELEMENTS_LAYER);
        updatePickerCameraPosition();
    }
    private void updateCameraPosition()
    {
        Camera camCam = cam.GetComponent<Camera>();
        if (cursorX * cellSize >= cam.transform.position.x + (camCam.orthographicSize * camCam.aspect / 2))
        {
            cam.transform.Translate(cellSize, 0, 0);
            instantiatedMapHUD.transform.Translate(cellSize, 0, 0);
        }
        if (cursorX * cellSize <= cam.transform.position.x - (camCam.orthographicSize * camCam.aspect / 2))
        {
            cam.transform.Translate(-cellSize, 0, 0);
            instantiatedMapHUD.transform.Translate(-cellSize, 0, 0);
        }
        if (cursorY * cellSize >= cam.transform.position.y + (camCam.orthographicSize / 2))
        {
            cam.transform.Translate(0, cellSize, 0);
            instantiatedMapHUD.transform.Translate(0, cellSize, 0);
        }
        if (cursorY * cellSize <= cam.transform.position.y - (camCam.orthographicSize / 2))
        {
            cam.transform.Translate(0, -cellSize, 0);
            instantiatedMapHUD.transform.Translate(0, -cellSize, 0);
        }
    }
    private void updatePickerCameraPosition()
    {
        Camera camCam = cam.GetComponent<Camera>();
        if (pickCursorX * pickerCellSizeX >= cam.transform.position.x + (camCam.orthographicSize * camCam.aspect / 2))
        {
//            cam.transform.Translate(pickerCellSizeX, 0, 0);
        }
        if (pickCursorX * pickerCellSizeX <= cam.transform.position.x - (camCam.orthographicSize * camCam.aspect / 2))
        {
//            cam.transform.Translate(-pickerCellSizeX, 0, 0);
        }
        if (pickerCursor.transform.position.y >= cam.transform.position.y + (camCam.orthographicSize / 2)
            && pickCursorY != 0)
        {
            cam.transform.Translate(0, pickerCellSizeY, 0);
            pickerBackground.transform.position = new Vector3(cam.transform.position.x,
                cam.transform.position.y, GridMap.BATTLE_BACKGROUND_LAYER);
        }
        if (pickerCursor.transform.position.y <= cam.transform.position.y - (camCam.orthographicSize / 2)
            && pickCursorY < pickerHeight - 1)
        {
            cam.transform.Translate(0, -pickerCellSizeY, 0);
            pickerBackground.transform.position = new Vector3(cam.transform.position.x,
                cam.transform.position.y, GridMap.BATTLE_BACKGROUND_LAYER);
        }
    }
    public override void LEFT_MOUSE(float mouseX, float mouseY)
    {
        //TODO
    }
    public override void RIGHT_MOUSE(float mouseX, float mouseY)
    {
        //TODO
    }
    public override void Z()
    {
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            if (mainMenuIdx == 0)
            {
                Destroy(instantiatedMainMenu);
                constructPickUnits();

                selectionMode = SelectionMode.PICK_UNITS;
            }
            else if (mainMenuIdx == 1)
            {
                //TODO
                Destroy(instantiatedMainMenu);
                constructItemMenu();

                selectionMode = SelectionMode.ITEM_MENU_PICK_UNIT;
            }
            else if (mainMenuIdx == 2)
            {
                Destroy(instantiatedMainMenu);
                initializeCamera();
                instantiatedMapHUD.SetActive(true);
                foreach (Tile t in playerTiles)
                {
                    t.traverseHighlight.SetActive(true);
                }
                selectionMode = SelectionMode.ROAM;
            }
            else if (mainMenuIdx == 3)
            {
                CampaignData.chapterPrep = CampaignData.scene;
                CampaignData.positions = player.ToArray();
                Destroy(instantiatedMainMenu);
                instantiatedSaveScreen = Instantiate(saveScreen);
                instantiatedSaveScreen.constructor(cam.GetComponent<Camera>());
                selectionMode = SelectionMode.SAVE;
            }
            else if (mainMenuIdx == 4)
            {
                //TODO anything else
                finish();
            }
            else if (mainMenuIdx == 5)
            {
                SpecialMenuLogic.mainMenu(instantiatableUnit);
            }
        } else if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.Z();
        } else if (selectionMode == SelectionMode.ROAM)
        {
            if (playerTiles.Contains(gridArray[cursorX, cursorY]))
            {
                selectedTile = gridArray[cursorX, cursorY];
                selectedTile.traverseHighlight.SetActive(false);
                selectedTile.interactHighlight.SetActive(true);

                selectionMode = SelectionMode.SWITCH;
            }
            else if (gridArray[cursorX, cursorY].getOccupant() != null)
            {
                selectedTile = gridArray[cursorX, cursorY];
                selectedUnit = selectedTile.getOccupant();
                fillTraversableTiles(selectedUnit, cursorX, cursorY);
                selectionMode = SelectionMode.MOVE;
            }
        } else if (selectionMode == SelectionMode.SWITCH)
        {
            if (playerTiles.Contains(gridArray[cursorX, cursorY]))
            {
                Tile second = gridArray[cursorX, cursorY];
                Unit temp = second.getOccupant();
                second.setOccupant(selectedTile.getOccupant());
                selectedTile.setOccupant(temp);
                selectedTile.traverseHighlight.SetActive(true);
                selectedTile.interactHighlight.SetActive(false);

                int pos1 = playerTiles.IndexOf(selectedTile);
                int pos2 = playerTiles.IndexOf(second);
                Debug.Log("Switch " + player[pos1] + " with " + player[pos2]);
                int temporary = player[pos1];
                player[pos1] = player[pos2];
                player[pos2] = temporary;

                selectionMode = SelectionMode.ROAM;
            }
        } else if (selectionMode == SelectionMode.PICK_UNITS)
        {
            Unit u = pickerUnits[pickCursorX, pickCursorY];
            if (!u.isLeader && u.isAlive()) {
                if (u.deployed)
                {
                    u.deployed = false;
                    int idx = CampaignData.members.IndexOf(u);
                    int mapIdx = player.IndexOf(idx);
                    playerTiles[mapIdx].setOccupant(null);
                    player[mapIdx] = -1;
                    u.transform.position = unusedPosition;
                    SpriteRenderer sr = pickerArray[pickCursorX, pickCursorY].transform.GetChild(1).GetComponent<SpriteRenderer>();
                    sr.sprite = ImageDictionary.getImage("blank_square");
                    sr.color = Color.blue;
                } else if (player.IndexOf(-1) != -1)
                {
                    u.deployed = true;
                    int idx = CampaignData.members.IndexOf(u);
                    int mapIdx = player.IndexOf(-1);
                    playerTiles[mapIdx].setOccupant(u);
                    player[mapIdx] = idx;
                    SpriteRenderer sr = pickerArray[pickCursorX, pickCursorY].transform.GetChild(1).GetComponent<SpriteRenderer>();
                    sr.size = new Vector2(1, 1);
                    sr.sprite = ImageDictionary.getImage("checkmark.png");
                    sr.color = Color.white;
                }
            }
        } else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            if (CampaignData.members[unitForConvoyIdx].isAlive())
            {
                inventoryIdx = 0;
                selectedUnit = CampaignData.members[unitForConvoyIdx];
                instantiatedItemsMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.white;
                updateItemDescription(SelectionMode.ITEM_MENU_INVENTORY);

                selectionMode = SelectionMode.ITEM_MENU_INVENTORY;
            }
        } else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            if (inventoryIdx == 1 && CampaignData.members[unitForConvoyIdx].heldWeapon != null)
            {
                Weapon w = CampaignData.members[unitForConvoyIdx].heldWeapon;
                CampaignData.addToConvoy(w);
                CampaignData.members[unitForConvoyIdx].heldWeapon = null;
                switchToItemType(itemTypeIdx, false);
                instantiatedItemsMenu.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text
                    = "_";
            } else if (inventoryIdx == 2 && CampaignData.members[unitForConvoyIdx].heldItem != null)
            {
                Item w = CampaignData.members[unitForConvoyIdx].heldItem;
                CampaignData.addToConvoy(w);
                CampaignData.members[unitForConvoyIdx].heldItem = null;
                switchToItemType(itemTypeIdx, false);
                instantiatedItemsMenu.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text
                    = "_";
            }
        } else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemScrollListMembers.Count > 0)
            {
                if (itemTypeIdx == CampaignData.getConvoyIds().Length - 1 && selectedUnit.heldItem == null)
                {
                    Item item = CampaignData.takeFromConvoy(itemTypeIdx, itemIdx);
                    Transform itemInList = instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0)
                        .GetChild(itemIdx);
                    itemInList.SetParent(null);
                    itemScrollListMembers.RemoveAt(itemIdx);
                    itemIdx = Mathf.Max(0, itemIdx - 1);
                    if (itemScrollListMembers.Count > 0)
                    {
                        itemScrollListMembers[itemIdx].color = Color.white;
                    }
                    selectedUnit.heldItem = item;
                    instantiatedItemsMenu.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text
                        = item.itemName;
                } else if (itemTypeIdx < CampaignData.getConvoyIds().Length - 1 && selectedUnit.heldWeapon == null)
                {
                    Item item = CampaignData.takeFromConvoy(itemTypeIdx, itemIdx);
                    Transform itemInList = instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0)
                        .GetChild(itemIdx);
                    itemInList.SetParent(null);
                    itemScrollListMembers.RemoveAt(itemIdx);
                    itemIdx = Mathf.Max(0, itemIdx - 1);
                    if (itemScrollListMembers.Count > 0)
                    {
                        itemScrollListMembers[itemIdx].color = Color.white;
                    }
                    selectedUnit.heldWeapon = (Weapon)item;
                    instantiatedItemsMenu.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text
                        = item.itemName;
                }
            }
        }
    }
    public override void X()
    {
        if (selectionMode == SelectionMode.ROAM)
        {
            instantiatedMapHUD.SetActive(false);
            toMainMenu();
        } else if (selectionMode == SelectionMode.MOVE)
        {
            unfillTraversableTiles();
            selectionMode = SelectionMode.ROAM;
            //                selectedUnit = null;
        } else if (selectionMode == SelectionMode.SWITCH)
        {
            selectedTile.traverseHighlight.SetActive(true);
            selectedTile.interactHighlight.SetActive(false);

            selectionMode = SelectionMode.ROAM;
        } else if (selectionMode == SelectionMode.STATS_PAGE)
        {
            deconstructStatsPage();

            selectionMode = SelectionMode.ROAM;
        } else if (selectionMode == SelectionMode.STATS_PAGE_PICK_UNITS)
        {
            deconstructStatsPage();

            selectionMode = SelectionMode.PICK_UNITS;
        }
        else if (selectionMode == SelectionMode.STATS_PAGE_ITEMS)
        {
            deconstructStatsPage();

            selectionMode = SelectionMode.ITEM_MENU_PICK_UNIT;
        }
        else if (selectionMode == SelectionMode.PICK_UNITS)
        {
            deconstructPickUnits();
            toMainMenu();
        } else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            deconstructItemMenu();
            toMainMenu();

            selectionMode = SelectionMode.MAIN_MENU;
        } else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;

            selectionMode = SelectionMode.ITEM_MENU_PICK_UNIT;
        } else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemScrollListMembers.Count > 0) {
                itemScrollListMembers[itemIdx].color = Color.black;
            }
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;

            selectionMode = SelectionMode.ITEM_MENU_INVENTORY;
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
        } else if (selectionMode == SelectionMode.PICK_UNITS)
        {
            selectedUnit = pickerUnits[pickCursorX, pickCursorY];
            constructStatsPage();
            selectionMode = SelectionMode.STATS_PAGE_PICK_UNITS;
        } else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            selectedUnit = CampaignData.members[unitForConvoyIdx];
            constructStatsPage();
            selectionMode = SelectionMode.STATS_PAGE_ITEMS;
        }
    }
    public override void UP()
    {
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            mainMenuIdx--;
            if (mainMenuIdx < 0)
            {
                mainMenuIdx = instantiatedMainMenu.transform.GetChild(0).childCount - 1;
            }
            instantiatedMainMenu.transform.GetChild(0).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            setMainMenuInfo();
        }
        else if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.UP();
        }
        else if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.SWITCH
            || selectionMode == SelectionMode.MOVE)
            && cursorY != height - 1)
        {
            cursorY++;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.PICK_UNITS && pickCursorY != 0)
        {
            pickCursorY--;
            updatePickCursor();
        } else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            unitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.black : Color.grey;
            switchUnitForConvoy(unitForConvoyIdx - 1 > -1 ? unitForConvoyIdx - 1 : unitScrollListMembers.Count - 1);
            unitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.white : Color.red;
            scrollToPoint(
            instantiatedItemsMenu.transform.GetChild(0).GetChild(0).GetComponent<ScrollRect>(),
                unitScrollListMembers[unitForConvoyIdx].transform.position);
        } else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            inventoryIdx = inventoryIdx == 0 ? 2 : inventoryIdx - 1;
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            updateItemDescription(SelectionMode.ITEM_MENU_INVENTORY);
        } else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemScrollListMembers.Count > 0)
            {
                itemScrollListMembers[itemIdx].color = Color.black;
                itemIdx = itemIdx == 0 ? itemScrollListMembers.Count - 1 : itemIdx - 1;
                itemScrollListMembers[itemIdx].color = Color.white;
                scrollToPoint(instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetComponent<ScrollRect>(),
                    itemScrollListMembers[itemIdx].transform.position);
                updateItemDescription(SelectionMode.ITEM_MENU_CONVOY);
            }
        }
    }
    public override void LEFT()
    {
        if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.SWITCH
            || selectionMode == SelectionMode.MOVE)
            && cursorX != 0)
        {
            cursorX--;
            updateCursor();
        } else if (selectionMode == SelectionMode.PICK_UNITS && pickCursorX != 0)
        {
            pickCursorX--;
            updatePickCursor();
        } else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemTypeIdx == 0)
            {
                if (itemScrollListMembers.Count > 0) {
                    itemScrollListMembers[itemIdx].color = Color.black;
                }
                instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;

                selectionMode = SelectionMode.ITEM_MENU_INVENTORY;
            } else
            {
                RectTransform rect = instantiatedItemsMenu.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>();
                rect.Translate(-rect.rect.width / 1.25f, 0, 0);
                switchToItemType(itemTypeIdx - 1, true);
            }
        }
    }
    public override void DOWN()
    {
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            mainMenuIdx = (mainMenuIdx + 1) % instantiatedMainMenu.transform.GetChild(0).childCount;
            instantiatedMainMenu.transform.GetChild(0).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            setMainMenuInfo();
        }
        else if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.DOWN();
        } else if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.SWITCH
            || selectionMode == SelectionMode.MOVE)
            && cursorY != 0)
        {
            cursorY--;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.PICK_UNITS && pickCursorY < pickerHeight - 1
            && pickerArray[pickCursorX, pickCursorY + 1] != null)
        {
            pickCursorY++;
            updatePickCursor();
        } else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            unitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.black : Color.grey;
            switchUnitForConvoy((unitForConvoyIdx + 1) % unitScrollListMembers.Count);
            unitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.white : Color.red;
            scrollToPoint(
            instantiatedItemsMenu.transform.GetChild(0).GetChild(0).GetComponent<ScrollRect>(),
                unitScrollListMembers[unitForConvoyIdx].transform.position);
        } else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            inventoryIdx = (inventoryIdx + 1) % 3;
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            updateItemDescription(SelectionMode.ITEM_MENU_INVENTORY);
        } else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemScrollListMembers.Count > 0)
            {
                itemScrollListMembers[itemIdx].color = Color.black;
                itemIdx = (itemIdx + 1) % itemScrollListMembers.Count;
                itemScrollListMembers[itemIdx].color = Color.white;
                scrollToPoint(instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetComponent<ScrollRect>(),
                    itemScrollListMembers[itemIdx].transform.position);
                updateItemDescription(SelectionMode.ITEM_MENU_CONVOY);
            }
        }
    }
    public override void RIGHT()
    {
        if ((selectionMode == SelectionMode.ROAM || selectionMode == SelectionMode.SWITCH
            || selectionMode == SelectionMode.MOVE)
                    && cursorX != width - 1)
        {
            cursorX++;
            updateCursor();
        }
        else if (selectionMode == SelectionMode.PICK_UNITS && pickCursorX < pickerWidth - 1
            && pickerArray[pickCursorX + 1, pickCursorY] != null)
        {
            pickCursorX++;
            updatePickCursor();
        } else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            switchToItemType(itemTypeIdx, true);
            if (itemScrollListMembers.Count > 0) {
                itemScrollListMembers[itemIdx].color = Color.white;
                scrollToPoint(instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetComponent<ScrollRect>(),
                    itemScrollListMembers[itemIdx].transform.position);
            }

            selectionMode = SelectionMode.ITEM_MENU_CONVOY;
        } else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemTypeIdx < CampaignData.getConvoyIds().Length - 1) {
                RectTransform rect = instantiatedItemsMenu.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>();
                rect.Translate(rect.rect.width / 1.25f, 0, 0);
                switchToItemType(itemTypeIdx + 1, true);
            }
        }
    }
    public override void ENTER()
    {
        /*
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            finish();
        }
        */
    }
    public override void ESCAPE()
    {
        //TODO maybe something
    }
    public override bool completed()
    {
        return done;
    }
    // Update is called once per frame
    void Update()
    {
        if (instantiatedSaveScreen != null && instantiatedSaveScreen.completed())
        {
            Destroy(instantiatedSaveScreen);
            instantiatedSaveScreen = null;
            toMainMenu();

            selectionMode = SelectionMode.MAIN_MENU;
        }
    }

    private void finish()
    {
        selectionMode = SelectionMode.STANDBY;
        instantiatedMapHUD.SetActive(false);
        Destroy(instantiatedMainMenu);
        Destroy(cursor);
        Destroy(instantiatedMapHUD);
        foreach (Tile t in playerTiles)
        {
            t.traverseHighlight.SetActive(false);
        }
        foreach (Tile t in gridArray)
        {
            Destroy(t);
        }

        done = true;
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
    private void addKeyAndValue(Dictionary<Tile, object> dict, Tile key, object val)
    {
        if (dict.ContainsKey(key))
        {
            dict.Remove(key);
        }
        dict.Add(key, val);
    }

    private void constructPickUnits()
    {
        foreach (GameObject go in pickerArray)
        {
            if (go != null) {
                go.SetActive(true);
            }
        }
        pickerCursor.SetActive(true);
        pickCursorX = 0;
        pickCursorY = 0;
        updatePickCursor();

        pickerBackground.SetActive(true);

        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        pickerBackground.SetActive(true);
        pickerBackground.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y,
            GridMap.BATTLE_BACKGROUND_LAYER);
    }
    private void deconstructPickUnits()
    {
        foreach (GameObject go in pickerArray)
        {
            if (go != null) {
                go.SetActive(false);
            }
        }
        pickerCursor.SetActive(false);
        pickerBackground.SetActive(false);
    }

    private void constructStatsPage()
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
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
            instantiatedStatsPageBackground.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (selectedUnit.team == Unit.UnitTeam.ALLY)
        {
            instantiatedStatsPageBackground.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if (selectedUnit.team == Unit.UnitTeam.OTHER)
        {
            instantiatedStatsPageBackground.GetComponent<SpriteRenderer>().color = Color.white;
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
        if (selectionMode == SelectionMode.STATS_PAGE) {
            Camera camCam = cam.GetComponent<Camera>();
            camCam.orthographicSize = cameraOrthographicSize;
            setCameraPosition(cursorX, cursorY);
            instantiatedMapHUD.SetActive(true);
        }

        Destroy(instantiatedStatsPageBackground);
    }

    private void constructItemMenu()
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        instantiatedItemsMenu = Instantiate(itemsMenu);
        Transform unitsContent = instantiatedItemsMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        unitScrollListMembers = new List<TextMeshProUGUI>();
        instantiatedItemsMenu.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "";
        inventoryIdx = 0;
        unitForConvoyIdx = 0;
        foreach (Unit u in CampaignData.members)
        {
            TextMeshProUGUI unitName = Instantiate(scrollListMember, unitsContent);
            unitName.text = u.unitName;
            unitName.color = u.isAlive() ? Color.black : Color.grey;
            unitScrollListMembers.Add(unitName);
        }
        unitsContent.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        switchUnitForConvoy(0);
        switchToItemType(0, false);
    }
    private void switchToItemType(int type, bool active)
    {
        Transform itemsContent = instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        itemsContent.DetachChildren();
        itemScrollListMembers = new List<TextMeshProUGUI>();
        currentItemIds = new List<int>();
        itemTypeIdx = type;
        itemIdx = 0;
        for (int q = 0; q < CampaignData.getConvoyIds()[type].Count; q++)
        {
            Item item = CampaignData.getItems()[CampaignData.getConvoyIds()[type][q]];
            TextMeshProUGUI itemName = Instantiate(scrollListMember, itemsContent);
            itemName.text = item.itemName + " (" + CampaignData.convoyDurabilities[type][q] + "/" + item.uses + ")";
            itemName.color = Color.black;
            itemScrollListMembers.Add(itemName);
            currentItemIds.Add(item.id);
        }
        if (itemScrollListMembers.Count > 0 && active)
        {
            itemScrollListMembers[0].color = Color.white;
            scrollToPoint(instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetComponent<ScrollRect>(),
                itemScrollListMembers[0].transform.position);
        }
    }
    private void switchUnitForConvoy(int unitIdx)
    {
        Unit u = CampaignData.members[unitIdx];
        unitForConvoyIdx = unitIdx;
        inventoryIdx = 0;
        instantiatedItemsMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
            = u.personalItem == null ? "_" : u.personalItem.itemName;
        instantiatedItemsMenu.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text
            = u.heldWeapon == null ? "_" : u.heldWeapon.itemName;
        instantiatedItemsMenu.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text
            = u.heldItem == null ? "_" : u.heldItem.itemName;
    }
    private void updateItemDescription(SelectionMode s)
    {
        TextMeshProUGUI desc = instantiatedItemsMenu.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>();
        if (s == SelectionMode.ITEM_MENU_INVENTORY)
        {
            if (inventoryIdx == 0)
            {
                desc.text = selectedUnit.personalItem == null ? "" : selectedUnit.personalItem.description();
            } else if (inventoryIdx == 1)
            {
                desc.text = selectedUnit.heldWeapon == null ? "" : selectedUnit.heldWeapon.description();
            }
            else if (inventoryIdx == 2)
            {
                desc.text = selectedUnit.heldItem == null ? "" : selectedUnit.heldItem.description();
            }
        } else if (s == SelectionMode.ITEM_MENU_CONVOY)
        {
            Item item = CampaignData.getItems()[currentItemIds[itemIdx]];
            desc.text = item.description();
        }
    }

    private void deconstructItemMenu()
    {
        Destroy(instantiatedItemsMenu);
    }

    /**
     * https://gist.github.com/yasirkula/75ca350fb83ddcc1558d33a8ecf1483f
     */
    private void scrollToPoint(ScrollRect scrollView, Vector2 focusPoint)
    {

        Vector2 contentSize = scrollView.content.rect.size;
        Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
        Vector2 contentScale = scrollView.content.localScale;

        contentSize.Scale(contentScale);
        focusPoint.Scale(contentScale);

        Vector2 scrollPosition = scrollView.normalizedPosition;
        if (scrollView.horizontal && contentSize.x > viewportSize.x)
            scrollPosition.x = Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
        if (scrollView.vertical && contentSize.y > viewportSize.y)
            scrollPosition.y = Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));

        scrollView.normalizedPosition = scrollPosition;
    }

    public enum SelectionMode
    {
        MAIN_MENU, ROAM, SWITCH, MOVE, MENU, SELECT_WEAPON,MAP_MENU, STATUS, STATS_PAGE,
        STATS_PAGE_PICK_UNITS, STATS_PAGE_ITEMS,
        ITEM_MENU_PICK_UNIT, ITEM_MENU_INVENTORY, ITEM_MENU_CONVOY, PICK_UNITS,
        STANDBY, ESCAPE_MENU, SAVE
    }

}
