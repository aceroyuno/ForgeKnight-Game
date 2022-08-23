using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item
{
    public string name;
    public int id;
    public int amount;
    public int maxStack;
    public bool Stackable
    {
        get { if (maxStack > 1) return true; else return false; }
    }
}

public class Consumable : Item
{
    int consumableType;


}

