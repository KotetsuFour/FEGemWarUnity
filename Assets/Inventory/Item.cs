using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string itemName;
    public int uses;
    public int usesLeft;
    public int id;

    public Item(string itemName, int uses, int id)
    {
        this.itemName = itemName;
        this.uses = uses;
        this.usesLeft = uses;
        this.id = id;
    }
    public abstract Item clone();
    public abstract string description();
}
