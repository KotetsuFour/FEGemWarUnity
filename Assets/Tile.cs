using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public string tileName;
    public int onFootCost;
    public int inAirCost;
    public int avoidBonus;

    private Unit occupant;

    public GameObject traverseHighlight;
    public GameObject attackHighlight;
    public GameObject interactHighlight;
    public GameObject moveSpriteOutline;
    public GameObject moveSprite;

    public Unit unit;
    public List<Gemstone> gemstones;
    public int ironLoot, steelLoot, silverLoot;
    public Item itemLoot;


    void Start()
    {
        traverseHighlight = Instantiate(traverseHighlight);
        traverseHighlight.transform.position = new Vector3(transform.position.x, transform.position.y, GridMap.TILE_HIGHLIGHT_LAYER);
        attackHighlight = Instantiate(attackHighlight);
        attackHighlight.transform.position = new Vector3(transform.position.x, transform.position.y, (float)GridMap.TILE_HIGHLIGHT_LAYER);
        interactHighlight = Instantiate(interactHighlight);
        interactHighlight.transform.position = new Vector3(transform.position.x, transform.position.y, (float)GridMap.TILE_HIGHLIGHT_LAYER);
        moveSpriteOutline = Instantiate(moveSpriteOutline);
        moveSpriteOutline.transform.position = new Vector3(transform.position.x, transform.position.y, (float)GridMap.MOVE_UNIT_OUTLINE_LAYER);
        moveSprite = Instantiate(moveSprite);
        moveSprite.transform.position = new Vector3(transform.position.x, transform.position.y, (float)GridMap.MOVE_UNIT_SPRITE_LAYER);
        gemstones = new List<Gemstone>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOccupant(Unit u)
    {
        this.occupant = u;
        if (occupant != null)
        {
            occupant.transform.position = new Vector3(transform.position.x, transform.position.y, GridMap.UNIT_SPRITE_LAYER);
        }
    }
    public Unit getOccupant()
    {
        return occupant;
    }
    public bool isVacant()
    {
        return occupant == null;
    }
    public int getMoveCost(Unit u)
    {
        if (u.isFlying())
        {
            return inAirCost;
        }
        return onFootCost;
    }


    public void setValues(string tileName, int onFootCost, int inAirCost, int avoidBonus)
    {
        this.tileName = tileName;
        this.onFootCost = onFootCost;
        this.inAirCost = inAirCost;
        this.avoidBonus = avoidBonus;
    }

    public bool hasLoot()
    {
        return ironLoot > 0 || steelLoot > 0 || silverLoot > 0 || itemLoot != null;
    }

    public string takeLoot(Unit retriever)
    {
        //You can't have multiple types of loot
        if (ironLoot > 0)
        {
            CampaignData.iron += ironLoot;
            string ret = "You got " + ironLoot + " Iron!";
            ironLoot = 0;
            return ret;
        }
        if (steelLoot > 0)
        {
            CampaignData.steel += steelLoot;
            string ret = "You got " + steelLoot + " Steel!";
            steelLoot = 0;
            return ret;
        }
        if (silverLoot > 0)
        {
            CampaignData.silver += silverLoot;
            string ret = "You got " + silverLoot + " Silver!";
            silverLoot = 0;
            return ret;
        }
        if (itemLoot is Weapon && retriever.heldWeapon == null)
        {
            retriever.heldWeapon = (Weapon)itemLoot.clone();
            string ret = "You got a(n) " + itemLoot.itemName + "!";
            itemLoot = null;
            return ret;
        } else if (!(itemLoot is Weapon) && retriever.heldItem == null)
        {
            retriever.heldItem = itemLoot.clone();
            string ret = "You got a(n) " + itemLoot.itemName + "!";
            itemLoot = null;
            return ret;
        } else
        {
            CampaignData.addToConvoy(itemLoot.clone());
            string ret = "A(n) " + itemLoot.itemName + " was sent to the convoy!";
            itemLoot = null;
            return ret;
        }
    }
}
