using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrystalBase : SequenceMember
{

    private Camera cam;
    private Unit instantiatableUnit;

    public SelectionMode selectionMode = SelectionMode.MAIN_MENU;

    [SerializeField] private GameObject mainMenu;
    private GameObject instantiatedMainMenu;
    private int mainMenuIdx;

    [SerializeField] private SaveScreen saveScreen;
    private SaveScreen instantiatedSaveScreen;

    private Unit selectedUnit;

    public GameObject itemsMenu;
    private GameObject instantiatedItemsMenu;
    public TextMeshProUGUI itemsScrollListMember;
    private List<TextMeshProUGUI> itemUnitScrollListMembers;
    private List<TextMeshProUGUI> itemScrollListMembers;
    private List<int> currentItemIds;
    private int itemTypeIdx;
    private int itemIdx;
    private int inventoryIdx;
    private int unitForConvoyIdx;

    [SerializeField] private GameObject statsPageBackground;
    private GameObject instantiatedStatsPageBackground;

    [SerializeField] private GameObject supportsMenu;
    private GameObject instantiatedSupportsMenu;
    [SerializeField] private TextMeshProUGUI supportsScrollListMember;
    private List<TextMeshProUGUI> supportUnitScrollListMembers;
    private int unitForSupportIdx;
    private int supportIdx;
    private List<int> membersListIndexes;
    [SerializeField] private Cutscene supportConversation;
    private Cutscene instantiatedSupportConversation;
    [SerializeField] private GameObject bonusEXPMenu;
    private GameObject instantiatedBonusEXPMenu;
    private List<TextMeshProUGUI> bonusEXPScrollListMembers;
    private int unitForBonusEXPIdx;
    private int bonusEXPAmount;
    [SerializeField] private GameObject levelUpPanel;
    private GameObject instantiatedLevelUpPanel;

    private bool done;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void constructor(Camera cam, Unit instantUnit)
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        instantiatableUnit = instantUnit;

        toMainMenu();
    }

    private void toMainMenu()
    {
        instantiatedMainMenu = Instantiate(mainMenu);
        mainMenuIdx = 0;
        instantiatedMainMenu.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        setMainMenuInfo();

        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        selectionMode = SelectionMode.MAIN_MENU;
    }
    private void setMainMenuInfo()
    {
        if (mainMenuIdx == 0)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Give units weapons and items";
        }
        else if (mainMenuIdx == 1)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Increase the bond between two units who have spent enough turns on the battlefield together";
        }
        else if (mainMenuIdx == 2)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Award extra experience points that you've earned from completing chapters under the par turn count";
        }
        else if (mainMenuIdx == 3)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Allow units to practice fighting with each other for EXP and weapon proficiency";
        }
        else if (mainMenuIdx == 4)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Use the iron, steel, and silver you've collected to make weapons";
        }
        else if (mainMenuIdx == 5)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Get information about the coming chapter";
        }
        else if (mainMenuIdx == 6)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Visit the enemies whose Gems you've collected";
        }
        else if (mainMenuIdx == 7)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Save your progress along with your units' inventories and battle positions";
        }
        else if (mainMenuIdx == 8)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Start the chapter! REMEMBER TO SAVE FIRST!";
        }
        else if (mainMenuIdx == 9)
        {
            instantiatedMainMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text
                = "Back to the main title. REMEMBER TO SAVE FIRST!";
        }
    }



    public override bool completed()
    {
        return done;
    }
    public override void LEFT_MOUSE(float mouseX, float mouseY)
    {
        //TODO
        if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.LEFT_MOUSE(mouseX, mouseY);
        }
        if (selectionMode == SelectionMode.SUPPORT_CONVO)
        {
            instantiatedSupportConversation.LEFT_MOUSE(mouseX, mouseY);
        }
    }
    public override void RIGHT_MOUSE(float mouseX, float mouseY)
    {
        //TODO
        if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.RIGHT_MOUSE(mouseX, mouseY);
        }
        if (selectionMode == SelectionMode.SUPPORT_CONVO)
        {
            instantiatedSupportConversation.RIGHT_MOUSE(mouseX, mouseY);
        }
    }
    public override void Z()
    {
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            if (mainMenuIdx == 0)
            {
                //TODO
                Destroy(instantiatedMainMenu);
                constructItemMenu();

                selectionMode = SelectionMode.ITEM_MENU_PICK_UNIT;
            }
            else if (mainMenuIdx == 1)
            {
                Destroy(instantiatedMainMenu);
                constructSupportsMenu();

                selectionMode = SelectionMode.SUPPORTS_PICK_UNIT;
            }
            else if (mainMenuIdx == 2)
            {
                Destroy(instantiatedMainMenu);
                constructBonusEXPMenu();

                selectionMode = SelectionMode.BONUS_EXP_PICK_UNIT;
            }
            else if (mainMenuIdx == 3)
            {

            }
            else if (mainMenuIdx == 4)
            {

            }
            else if (mainMenuIdx == 5)
            {

            }
            else if (mainMenuIdx == 6)
            {

            }
            else if (mainMenuIdx == 7)
            {
                Destroy(instantiatedMainMenu);
                instantiatedSaveScreen = Instantiate(saveScreen);
                instantiatedSaveScreen.constructor(cam.GetComponent<Camera>());
                selectionMode = SelectionMode.SAVE;
            }
            else if (mainMenuIdx == 8)
            {
                //TODO anything else
                finish();
            }
            else if (mainMenuIdx == 9)
            {
                SpecialMenuLogic.mainMenu(instantiatableUnit);
            }
        }
        else if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.Z();
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            if (CampaignData.members[unitForConvoyIdx].isAlive())
            {
                inventoryIdx = 0;
                selectedUnit = CampaignData.members[unitForConvoyIdx];
                instantiatedItemsMenu.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.white;
                updateItemDescription(SelectionMode.ITEM_MENU_INVENTORY);

                selectionMode = SelectionMode.ITEM_MENU_INVENTORY;
            }
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            if (inventoryIdx == 1 && CampaignData.members[unitForConvoyIdx].heldWeapon != null)
            {
                Weapon w = CampaignData.members[unitForConvoyIdx].heldWeapon;
                CampaignData.addToConvoy(w);
                CampaignData.members[unitForConvoyIdx].heldWeapon = null;
                switchToItemType(itemTypeIdx, false);
                instantiatedItemsMenu.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text
                    = "_";
            }
            else if (inventoryIdx == 2 && CampaignData.members[unitForConvoyIdx].heldItem != null)
            {
                Item w = CampaignData.members[unitForConvoyIdx].heldItem;
                CampaignData.addToConvoy(w);
                CampaignData.members[unitForConvoyIdx].heldItem = null;
                switchToItemType(itemTypeIdx, false);
                instantiatedItemsMenu.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text
                    = "_";
            }
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
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
                }
                else if (itemTypeIdx < CampaignData.getConvoyIds().Length - 1 && selectedUnit.heldWeapon == null)
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
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_UNIT)
        {
            if (CampaignData.members[unitForConvoyIdx].isAlive())
            {
                supportIdx = 0;
                selectedUnit = CampaignData.members[membersListIndexes[unitForSupportIdx]];
                instantiatedSupportsMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

                selectionMode = SelectionMode.SUPPORTS_PICK_SUPPORT;
            }
        }
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_SUPPORT)
        {
            int supportId = supportIdx == 0 ? selectedUnit.supportId1 : selectedUnit.supportId2;
            int convo = 0;
            while (convo < 4 && CampaignData.getSupportRequirements()[supportId][convo] == -1)
            {
                convo++;
            }
            if (convo == 4
                || CampaignData.getSupportRequirements()[supportId][convo] > CampaignData.getSupportLevels()[supportId])
            {
                return;
            }
            if (convo == 3)
            {
                //FUSE
                Destroy(instantiatedSupportsMenu);
                CampaignData.getSupportLevels()[supportId] = 0;
                CampaignData.getSupportRequirements()[supportId][convo] = -1;
                //TODO instantiate fusion animation
                //TODO switch to SelectionMode.FUSION
            }
            else {
                Destroy(instantiatedSupportsMenu);
                CampaignData.getSupportLevels()[supportId] = 0;
                CampaignData.getSupportRequirements()[supportId][convo] = -1;
                if (CampaignData.getSupportLog()[supportId][convo] == "")
                {
                    constructSupportsMenu();
                    selectionMode = SelectionMode.SUPPORTS_PICK_UNIT;
                } else
                {
                    instantiatedSupportConversation = Instantiate(supportConversation);
                    instantiatedSupportConversation.constructor(new DialogueEvent(0,
                        "Assets/Dialogue/Supports/" + CampaignData.getSupportLog()[supportId][convo]), cam);
                    selectionMode = SelectionMode.SUPPORT_CONVO;
                }
            }
        }
        else if (selectionMode == SelectionMode.SUPPORT_CONVO)
        {
            instantiatedSupportConversation.Z();
        } else if (selectionMode == SelectionMode.BONUS_EXP_PICK_UNIT)
        {
            if (CampaignData.members[unitForBonusEXPIdx].isAlive())
            {
                selectedUnit = CampaignData.members[membersListIndexes[unitForBonusEXPIdx]];
                CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Units").gameObject.SetActive(false);
                CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "CurrentEXPLabel").gameObject.SetActive(false);
                CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "GiveEXP").gameObject.SetActive(true);
                setupGiveEXP();

                selectionMode = SelectionMode.BONUS_EXP_GIVE;
            }

        }
        else if (selectionMode == SelectionMode.BONUS_EXP_GIVE)
        {
            bool[] gainedStats = selectedUnit.addExperience(bonusEXPAmount);
            setupGiveEXP();
            if (gainedStats != null)
            {
                instantiatedLevelUpPanel = Instantiate(levelUpPanel);
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "LevelUpName")
                    .GetComponent<TextMeshProUGUI>().text = selectedUnit.unitName;
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "Portrait")
                    .GetComponent<SpriteRenderer>().sprite
                    = ImageDictionary.getImage(selectedUnit.spriteName);

                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "HPLevel")
                    .GetComponent<TextMeshProUGUI>().text = "HP       " + selectedUnit.maxHP;
                if (gainedStats[0])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "HPLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "STRLevel")
                    .GetComponent<TextMeshProUGUI>().text = "STR       " + selectedUnit.strength;
                if (gainedStats[1])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "STRLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "MAGLevel")
                    .GetComponent<TextMeshProUGUI>().text = "MAG       " + selectedUnit.magic;
                if (gainedStats[2])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "MAGLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "SKLLevel")
                    .GetComponent<TextMeshProUGUI>().text = "SKL       " + selectedUnit.skill;
                if (gainedStats[3])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "SKLLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "SPDLevel")
                    .GetComponent<TextMeshProUGUI>().text = "SPD       " + selectedUnit.speed;
                if (gainedStats[4])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "SPDLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "LCKLevel")
                    .GetComponent<TextMeshProUGUI>().text = "LCK       " + selectedUnit.luck;
                if (gainedStats[5])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "LCKLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "DEFLevel")
                    .GetComponent<TextMeshProUGUI>().text = "DEF       " + selectedUnit.defense;
                if (gainedStats[6])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "DEFLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "RESLevel")
                    .GetComponent<TextMeshProUGUI>().text = "RES       " + selectedUnit.resistance;
                if (gainedStats[7])
                {
                    CampaignData.findDeepChild(instantiatedLevelUpPanel.transform, "RESLevel")
                        .GetComponent<TextMeshProUGUI>().color = Color.cyan;
                }
                selectionMode = SelectionMode.LEVEL_UP;
            }
        }
        else if (selectionMode == SelectionMode.LEVEL_UP)
        {
            Destroy(instantiatedLevelUpPanel);

            selectionMode = SelectionMode.BONUS_EXP_GIVE;
        }
    }
    public override void X()
    {
        if (selectionMode == SelectionMode.STATS_PAGE_ITEMS)
        {
            deconstructStatsPage();

            selectionMode = SelectionMode.ITEM_MENU_PICK_UNIT;
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            deconstructItemMenu();
            toMainMenu();

            selectionMode = SelectionMode.MAIN_MENU;
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;

            selectionMode = SelectionMode.ITEM_MENU_PICK_UNIT;
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemScrollListMembers.Count > 0)
            {
                itemScrollListMembers[itemIdx].color = Color.black;
            }
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;

            selectionMode = SelectionMode.ITEM_MENU_INVENTORY;
        }
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_UNIT)
        {
            deconstructSupportsMenu();
            toMainMenu();

            selectionMode = SelectionMode.MAIN_MENU;
        }
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_SUPPORT)
        {
            instantiatedItemsMenu.transform.GetChild(1).GetChild(5 * supportIdx).GetComponent<TextMeshProUGUI>().color = Color.black;

            selectionMode = SelectionMode.SUPPORTS_PICK_UNIT;
        }
        else if (selectionMode == SelectionMode.STATS_PAGE_SUPPORTS)
        {
            deconstructStatsPage();

            selectionMode = SelectionMode.SUPPORTS_PICK_UNIT;
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_PICK_UNIT)
        {
            deconstructBonusEXPPage();

            selectionMode = SelectionMode.MAIN_MENU;
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_GIVE)
        {
            CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Units").gameObject.SetActive(true);
            CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "CurrentEXPLabel").gameObject.SetActive(true);
            CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "GiveEXP").gameObject.SetActive(false);
        }
        else if (selectionMode == SelectionMode.LEVEL_UP)
        {
            Destroy(instantiatedLevelUpPanel);

            selectionMode = SelectionMode.BONUS_EXP_GIVE;
        }
    }
    public override void C()
    {
        if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            selectedUnit = CampaignData.members[unitForConvoyIdx];
            constructStatsPage();
            selectionMode = SelectionMode.STATS_PAGE_ITEMS;
        }
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_UNIT)
        {
            selectedUnit = CampaignData.members[membersListIndexes[unitForSupportIdx]];
            constructStatsPage();
            selectionMode = SelectionMode.STATS_PAGE_SUPPORTS;
        }
    }
    public override void UP()
    {
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            mainMenuIdx--;
            if (mainMenuIdx < 0)
            {
                mainMenuIdx = instantiatedMainMenu.transform.GetChild(1).childCount - 1;
            }
            instantiatedMainMenu.transform.GetChild(1).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            setMainMenuInfo();
        }
        else if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.UP();
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            itemUnitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.black : Color.grey;
            switchUnitForConvoy(unitForConvoyIdx - 1 > -1 ? unitForConvoyIdx - 1 : itemUnitScrollListMembers.Count - 1);
            itemUnitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.white : Color.red;
            scrollToPoint(
            instantiatedItemsMenu.transform.GetChild(0).GetChild(0).GetComponent<ScrollRect>(),
                itemUnitScrollListMembers[unitForConvoyIdx].transform.position);
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            inventoryIdx = inventoryIdx == 0 ? 2 : inventoryIdx - 1;
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            updateItemDescription(SelectionMode.ITEM_MENU_INVENTORY);
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
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
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_UNIT)
        {
            supportUnitScrollListMembers[unitForSupportIdx].color = Color.black;
            unitForSupportIdx = unitForSupportIdx - 1 > -1 ? unitForSupportIdx - 1 : supportUnitScrollListMembers.Count - 1;
            supportUnitScrollListMembers[unitForSupportIdx].color = Color.white;
            scrollToPoint(
            instantiatedSupportsMenu.transform.GetChild(0).GetChild(0).GetComponent<ScrollRect>(),
                supportUnitScrollListMembers[unitForSupportIdx].transform.position);
        }
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_SUPPORT)
        {
            instantiatedSupportsMenu.transform.GetChild(1).GetChild(5 * supportIdx).GetComponent<TextMeshProUGUI>().color
                = Color.black;
            supportIdx = 1 - supportIdx;
            instantiatedSupportsMenu.transform.GetChild(1).GetChild(5 * supportIdx).GetComponent<TextMeshProUGUI>().color
                = Color.white;
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_PICK_UNIT)
        {
            bonusEXPScrollListMembers[unitForBonusEXPIdx].color = Color.black;
            unitForBonusEXPIdx = unitForBonusEXPIdx - 1 > -1 ? unitForBonusEXPIdx - 1 : bonusEXPScrollListMembers.Count - 1;
            bonusEXPScrollListMembers[unitForBonusEXPIdx].color = Color.white;
            scrollToPoint(
                CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Units")
                .GetComponent<ScrollRect>(),
                bonusEXPScrollListMembers[unitForBonusEXPIdx].transform.position);
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_GIVE)
        {
            changeGivenBonusEXP(1);
        }

    }
    public override void LEFT()
    {
        if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemTypeIdx == 0)
            {
                if (itemScrollListMembers.Count > 0)
                {
                    itemScrollListMembers[itemIdx].color = Color.black;
                }
                instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;

                selectionMode = SelectionMode.ITEM_MENU_INVENTORY;
            }
            else
            {
                RectTransform rect = instantiatedItemsMenu.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>();
                rect.Translate(-rect.rect.width / 1.25f, 0, 0);
                switchToItemType(itemTypeIdx - 1, true);
            }
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_GIVE)
        {
            changeGivenBonusEXP(-10);
        }
    }
    public override void DOWN()
    {
        if (selectionMode == SelectionMode.MAIN_MENU)
        {
            instantiatedMainMenu.transform.GetChild(1).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            mainMenuIdx = (mainMenuIdx + 1) % instantiatedMainMenu.transform.GetChild(1).childCount;
            instantiatedMainMenu.transform.GetChild(1).GetChild(mainMenuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            setMainMenuInfo();
        }
        else if (selectionMode == SelectionMode.SAVE)
        {
            instantiatedSaveScreen.DOWN();
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_PICK_UNIT)
        {
            itemUnitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.black : Color.grey;
            switchUnitForConvoy((unitForConvoyIdx + 1) % itemUnitScrollListMembers.Count);
            itemUnitScrollListMembers[unitForConvoyIdx].color = CampaignData.members[unitForConvoyIdx].isAlive()
                ? Color.white : Color.red;
            scrollToPoint(
            instantiatedItemsMenu.transform.GetChild(0).GetChild(0).GetComponent<ScrollRect>(),
                itemUnitScrollListMembers[unitForConvoyIdx].transform.position);
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            inventoryIdx = (inventoryIdx + 1) % 3;
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
            updateItemDescription(SelectionMode.ITEM_MENU_INVENTORY);
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
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
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_UNIT)
        {
            supportUnitScrollListMembers[unitForSupportIdx].color = Color.black;
            unitForSupportIdx = (unitForSupportIdx + 1) % supportUnitScrollListMembers.Count;
            supportUnitScrollListMembers[unitForSupportIdx].color = Color.white;
            scrollToPoint(
            instantiatedSupportsMenu.transform.GetChild(0).GetChild(0).GetComponent<ScrollRect>(),
                supportUnitScrollListMembers[unitForSupportIdx].transform.position);
        }
        else if (selectionMode == SelectionMode.SUPPORTS_PICK_SUPPORT)
        {
            instantiatedSupportsMenu.transform.GetChild(1).GetChild(5 * supportIdx).GetComponent<TextMeshProUGUI>().color
                = Color.black;
            supportIdx = 1 - supportIdx;
            instantiatedSupportsMenu.transform.GetChild(1).GetChild(5 * supportIdx).GetComponent<TextMeshProUGUI>().color
                = Color.white;
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_PICK_UNIT)
        {
            bonusEXPScrollListMembers[unitForBonusEXPIdx].color = Color.black;
            unitForBonusEXPIdx = (unitForBonusEXPIdx + 1) % bonusEXPScrollListMembers.Count;
            bonusEXPScrollListMembers[unitForBonusEXPIdx].color = Color.white;
            scrollToPoint(
                CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Units")
                .GetComponent<ScrollRect>(),
                bonusEXPScrollListMembers[unitForBonusEXPIdx].transform.position);
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_GIVE)
        {
            changeGivenBonusEXP(-1);
        }

    }
    public override void RIGHT()
    {
        if (selectionMode == SelectionMode.ITEM_MENU_INVENTORY)
        {
            instantiatedItemsMenu.transform.GetChild(0).GetChild(2 + inventoryIdx).GetComponent<TextMeshProUGUI>().color = Color.black;
            switchToItemType(itemTypeIdx, true);
            if (itemScrollListMembers.Count > 0)
            {
                itemScrollListMembers[itemIdx].color = Color.white;
                scrollToPoint(instantiatedItemsMenu.transform.GetChild(0).GetChild(1).GetComponent<ScrollRect>(),
                    itemScrollListMembers[itemIdx].transform.position);
            }

            selectionMode = SelectionMode.ITEM_MENU_CONVOY;
        }
        else if (selectionMode == SelectionMode.ITEM_MENU_CONVOY)
        {
            if (itemTypeIdx < CampaignData.getConvoyIds().Length - 1)
            {
                RectTransform rect = instantiatedItemsMenu.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>();
                rect.Translate(rect.rect.width / 1.25f, 0, 0);
                switchToItemType(itemTypeIdx + 1, true);
            }
        }
        else if (selectionMode == SelectionMode.BONUS_EXP_GIVE)
        {
            changeGivenBonusEXP(10);
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
        if (selectionMode == SelectionMode.SUPPORT_CONVO)
        {
            instantiatedSupportConversation.ENTER();
        }
    }
    public override void ESCAPE()
    {
        //TODO maybe something
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
        else if (instantiatedSupportConversation != null && instantiatedSupportConversation.completed())
        {
            Destroy(instantiatedSupportConversation);
            instantiatedSupportConversation = null;
            constructSupportsMenu();

            selectionMode = SelectionMode.SUPPORTS_PICK_UNIT;
        }
    }

    private void finish()
    {
        selectionMode = SelectionMode.STANDBY;
        Destroy(instantiatedMainMenu);

        done = true;
    }

    private void constructItemMenu()
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        instantiatedItemsMenu = Instantiate(itemsMenu);
        Transform unitsContent = instantiatedItemsMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        itemUnitScrollListMembers = new List<TextMeshProUGUI>();
        instantiatedItemsMenu.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "";
        inventoryIdx = 0;
        unitForConvoyIdx = 0;
        foreach (Unit u in CampaignData.members)
        {
            TextMeshProUGUI unitName = Instantiate(itemsScrollListMember, unitsContent);
            unitName.text = u.unitName;
            unitName.color = u.isAlive() ? Color.black : Color.grey;
            itemUnitScrollListMembers.Add(unitName);
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
            TextMeshProUGUI itemName = Instantiate(itemsScrollListMember, itemsContent);
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
            }
            else if (inventoryIdx == 1)
            {
                desc.text = selectedUnit.heldWeapon == null ? "" : selectedUnit.heldWeapon.description();
            }
            else if (inventoryIdx == 2)
            {
                desc.text = selectedUnit.heldItem == null ? "" : selectedUnit.heldItem.description();
            }
        }
        else if (s == SelectionMode.ITEM_MENU_CONVOY)
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

    private void constructStatsPage()
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;
        instantiatedStatsPageBackground = Instantiate(statsPageBackground);
        instantiatedStatsPageBackground.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite
            = selectedUnit.GetComponent<SpriteRenderer>().sprite;
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
        /*
        if (selectionMode == SelectionMode.STATS_PAGE)
        {
            Camera camCam = cam.GetComponent<Camera>();
            camCam.orthographicSize = cameraOrthographicSize;
            setCameraPosition(cursorX, cursorY);
            instantiatedMapHUD.SetActive(true);
        }
        */

        Destroy(instantiatedStatsPageBackground);
    }

    private void constructSupportsMenu()
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        instantiatedSupportsMenu = Instantiate(supportsMenu);
        Transform unitsContent = instantiatedSupportsMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        supportUnitScrollListMembers = new List<TextMeshProUGUI>();
        supportIdx = 0;
        unitForSupportIdx = 0;
        membersListIndexes = new List<int>();
        for (int q = 0; q < CampaignData.members.Count; q++)
        {
            Unit u = CampaignData.members[q];
            if (u.isAlive() && (u.supportId1 >= 0 || u.supportId2 >= 0))
            {
                TextMeshProUGUI unitName = Instantiate(supportsScrollListMember, unitsContent);
                unitName.text = u.unitName;
                unitName.color = Color.black;
                itemUnitScrollListMembers.Add(unitName);
            }
        }
        unitsContent.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        switchUnitForSupports(0);
    }
    private void switchUnitForSupports(int listIdx)
    {
        int membersIdx = membersListIndexes[listIdx];
        Unit u = CampaignData.members[membersIdx];

        TextMeshProUGUI talker1 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI c1 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI b1 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI a1 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fuse1 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI talker2 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI c2 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI b2 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI a2 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(8).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fuse2 = instantiatedSupportsMenu.transform.GetChild(1).GetChild(9).GetComponent<TextMeshProUGUI>();

        Unit[] livingPartners = CampaignData.findLivingSupportPartners(u);
        if (livingPartners[0] == null)
        {
            talker1.text = "---";
            c1.text = "-"; c1.color = Color.black;
            b1.text = "-"; b1.color = Color.black;
            a1.text = "-"; a1.color = Color.black;
            fuse1.text = "-"; fuse1.color = Color.black;
        } else
        {
            c1.text = "C";
            b1.text = "B";
            a1.text = "A";
            fuse1.text = "FUSE";
            talker1.text = livingPartners[0].unitName;
            if (CampaignData.getSupportRequirements()[u.supportId1][0] == -1)
            {
                //We've already completed the support
                c1.color = Color.white;
            } else if (CampaignData.getSupportRequirements()[u.supportId1][0] <= CampaignData.getSupportLevels()[u.supportId1])
            {
                //The support is available
                c1.color = new Color(1, 180 / 255f, 0);
            } else
            {
                //The support is not unlocked yet
                c1.color = Color.black;
            }
            if (CampaignData.getSupportRequirements()[u.supportId1][1] == -1)
            {
                //We've already completed the support
                b1.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId1][1] <= CampaignData.getSupportLevels()[u.supportId1])
            {
                //The support is available
                b1.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //The support is not unlocked yet
                b1.color = Color.black;
            }
            if (CampaignData.getSupportRequirements()[u.supportId1][2] == -1)
            {
                //We've already completed the support
                a1.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId1][2] <= CampaignData.getSupportLevels()[u.supportId1])
            {
                //The support is available
                a1.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //The support is not unlocked yet
                a1.color = Color.black;
            }
            if (CampaignData.getSupportRequirements()[u.supportId1][3] == -1)
            {
                //We've already completed FUSION
                fuse1.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId1][2] == -1)
            {
                //We've completed the A support and FUSE is available
                fuse1.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //FUSE is not unlocked yet
                fuse1.color = Color.black;
            }
        }
        if (livingPartners[1] == null)
        {
            talker2.text = "---";
            c2.text = "-"; c2.color = Color.black;
            b2.text = "-"; b2.color = Color.black;
            a2.text = "-"; a2.color = Color.black;
            fuse2.text = "-"; fuse2.color = Color.black;
        }
        else
        {
            c2.text = "C";
            b2.text = "B";
            a2.text = "A";
            fuse2.text = "FUSE";
            talker2.text = livingPartners[1].unitName;
            if (CampaignData.getSupportRequirements()[u.supportId2][0] == -1)
            {
                //We've already completed the support
                c2.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId2][0] <= CampaignData.getSupportLevels()[u.supportId1])
            {
                //The support is available
                c2.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //The support is not unlocked yet
                c2.color = Color.black;
            }
            if (CampaignData.getSupportRequirements()[u.supportId2][1] == -1)
            {
                //We've already completed the support
                b2.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId2][1] <= CampaignData.getSupportLevels()[u.supportId1])
            {
                //The support is available
                b2.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //The support is not unlocked yet
                b2.color = Color.black;
            }
            if (CampaignData.getSupportRequirements()[u.supportId2][2] == -1)
            {
                //We've already completed the support
                a2.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId2][2] <= CampaignData.getSupportLevels()[u.supportId1])
            {
                //The support is available
                a2.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //The support is not unlocked yet
                a2.color = Color.black;
            }
            if (CampaignData.getSupportRequirements()[u.supportId2][3] == -1)
            {
                //We've already completed FUSION
                fuse2.color = Color.white;
            }
            else if (CampaignData.getSupportRequirements()[u.supportId2][2] == -1)
            {
                //We've completed the A support and FUSE is available
                fuse2.color = new Color(1, 180 / 255f, 0);
            }
            else
            {
                //FUSE is not unlocked yet
                fuse2.color = Color.black;
            }
        }
    }

    private void deconstructSupportsMenu()
    {
        Destroy(instantiatedSupportsMenu);
    }

    private void constructBonusEXPMenu()
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        instantiatedBonusEXPMenu = Instantiate(bonusEXPMenu);
        Transform unitsContent = instantiatedBonusEXPMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        bonusEXPScrollListMembers = new List<TextMeshProUGUI>();
        unitForBonusEXPIdx = 0;
        for (int q = 0; q < CampaignData.members.Count; q++)
        {
            Unit u = CampaignData.members[q];
            if (u.isAlive())
            {
                TextMeshProUGUI unitName = Instantiate(supportsScrollListMember, unitsContent);
                unitName.text = u.unitName;
                unitName.color = Color.black;
                itemUnitScrollListMembers.Add(unitName);
            }
        }
        unitsContent.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
//        switchUnitForSupports(0);
    }
    private void setupGiveEXP()
    {
        CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Stats")
            .GetComponent<TextMeshProUGUI>().text = $"HP: {selectedUnit.currentHP}/{selectedUnit.maxHP}\n" +
            $"Strength: {selectedUnit.strength}\nMagic: {selectedUnit.magic}\n" +
            $"Skill: {selectedUnit.skill}\nSpeed: {selectedUnit.speed}\nLuck: {selectedUnit.luck}\n" +
            $"Defense: {selectedUnit.defense}\nResistance: {selectedUnit.resistance}";
        CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Name")
            .GetComponent<TextMeshProUGUI>().text = selectedUnit.unitName;
        CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Portrait")
            .GetComponent<Image>().sprite = ImageDictionary.getImage(selectedUnit.spriteName);
        bonusEXPAmount = 0;
        changeGivenBonusEXP(0);
    }
    private void changeGivenBonusEXP(int change)
    {
        bonusEXPAmount = Mathf.Max(0, Mathf.Min(100, Mathf.Min(CampaignData.bonusEXP, bonusEXPAmount + change)));
        CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "GiveAmount")
            .GetComponent<TextMeshProUGUI>().text = $"Give: {bonusEXPAmount}/{CampaignData.bonusEXP}";
        int expSum = selectedUnit.experience + bonusEXPAmount;
        int resultLvl = selectedUnit.level + (expSum / 100);
        int resultExp = selectedUnit.experience + (expSum % 100);
        CampaignData.findDeepChild(instantiatedBonusEXPMenu.transform, "Level")
            .GetComponent<TextMeshProUGUI>().text = $"LVL {selectedUnit.level} : EXP {selectedUnit.experience} " +
            $"=> LVL {resultLvl} : EXP {resultExp}";
    }

    private void deconstructBonusEXPPage()
    {
        Destroy(instantiatedBonusEXPMenu);
    }

    public enum SelectionMode
    {
        MAIN_MENU,
        SUPPORTS_PICK_UNIT, SUPPORTS_PICK_SUPPORT, SUPPORT_CONVO, FUSION,
        BONUS_EXP_PICK_UNIT, BONUS_EXP_GIVE, LEVEL_UP,
        STATS_PAGE_ITEMS, STATS_PAGE_SPARRING, STATS_PAGE_SUPPORTS,
        ITEM_MENU_PICK_UNIT, ITEM_MENU_INVENTORY, ITEM_MENU_CONVOY,
        STANDBY, ESCAPE_MENU, SAVE
    }

}
