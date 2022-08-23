using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This primarily exists for logic purposes
public class Equipment : Item
{
    public float weight;


}

public class Armor: Equipment
{
    int armorClass; // 1 = clothing || 2 = light || 3 = heavy
    int armorType; // 1 = greave || 2 = torso || 3 = arms || 4 = helm
    DefenseStatMatrix defenses;


}

public class Weapon: Equipment
{
    int weaponType;


}

public class DefenseStatMatrix
{
    //physical defense
    float slashRes, strikeRes, pierceRes;
    //elemental defense
    float fireRes, iceRes, lightningRes;

}

