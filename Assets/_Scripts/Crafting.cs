using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that handles all crafting logic
/// </summary>
public static class Crafting
{





}


public class Recipe
{
    public string name;
    public int id;
    public int itemID;
    public int itemClass, itemType, craftingType;
    public List<RecipeRequirement> craftingRecipe;
}

public class ArmorRecipe : Recipe
{
    float weight;


}

public class Blueprint : Recipe
{
    int strReq, dexReq, affReq;
    float damageModifier;
    int[] avaiableActionIDs;
    ActionRequirement[] actionReqs;
    float weight;
    float balance;
    float earthValue, fireValue, windValue, waterValue;

    public Blueprint()
    {
        itemID = -1;
    }

}

public class RecipeRequirement
{
    int materialType;
    int amount;

    public RecipeRequirement(int mt, int amt)
    {
        materialType = mt;
        amount = amt;
    }
}

public class ActionRequirement
{
    float weightReq, balanceReq;
    float earthReq, fireReq, windReq, waterReq;
}

public class Material : Item
{
    int type; //
    int element; //0 = none, 1 = earth, 2 = fire, 3 = wind, 4 = water
    float elementVal;
    float weightMod;
    float balanceMod;


}

/// <summary>
/// Maybe use this to keep track of if an item can be used for different types of weapons
/// </summary>
public class Tag
{

}