using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Depreciated
/// </summary>
public class OldHexBoard
{
    OldHex center;

    //A method to create a OldHex board with a 1 to many relationship with other OldHex objects.
    //OldHex connections are formed with a left-right priority based on local orientation.
    //It then recursively calls forward down the OldHex tree filling it out as it goes along,
    //stoping once it goes the max distance or once it finds a completed OldHex 
    void CreateBoard(int size, int enterDir, OldHex currentOldHex)
    {
        if (size < 0)
            return;
        OldHex checkCell;
        switch(enterDir)
        {
            case -1://Starting point
                center = new OldHex();
                currentOldHex = center;
                center.North = new OldHex();
                center.NorthEast = new OldHex();
                center.SouthEast = new OldHex();
                center.South = new OldHex();
                center.SouthWest = new OldHex();
                center.NorthWest = new OldHex();
                currentOldHex.completed = true;
                //recursive calls
                CreateBoard(size - 1, 0, center);
                CreateBoard(size - 1, 1, center);
                CreateBoard(size - 1, 2, center);
                CreateBoard(size - 1, 3, center);
                CreateBoard(size - 1, 4, center);
                CreateBoard(size - 1, 5, center);
                break;
            case 0://Went North
                if (currentOldHex.North.completed)
                    return;
                currentOldHex.North.South = currentOldHex;
                currentOldHex = currentOldHex.North;
                if (currentOldHex.completed)
                    return;
                checkCell = currentOldHex.South.NorthWest;
                if (checkCell != null)
                    currentOldHex.SouthWest = checkCell;
                else
                    currentOldHex.SouthWest = new OldHex();
                checkCell = currentOldHex.South.NorthEast;
                if (checkCell != null)
                    currentOldHex.SouthEast = checkCell;
                else
                    currentOldHex.SouthEast = new OldHex();
                checkCell = currentOldHex.SouthEast.North;
                if (checkCell != null)
                    currentOldHex.NorthEast = checkCell;
                else
                    currentOldHex.NorthEast = new OldHex();
                checkCell = currentOldHex.SouthWest.North;
                if (checkCell != null)
                    currentOldHex.NorthWest = checkCell;
                else
                    currentOldHex.NorthWest = new OldHex();
                if (currentOldHex.NorthWest.NorthEast != null)
                    currentOldHex.North = currentOldHex.NorthWest.NorthEast;
                else if (currentOldHex.NorthEast.NorthWest != null)
                    currentOldHex.North = currentOldHex.NorthEast.NorthWest;
                else
                    currentOldHex.North = new OldHex();
                currentOldHex.completed = true;
                //Recursion calls
                CreateBoard(size - 1, 0, currentOldHex);
                CreateBoard(size - 1, 1, currentOldHex);
                CreateBoard(size - 1, 2, currentOldHex);
                CreateBoard(size - 1, 3, currentOldHex);
                CreateBoard(size - 1, 4, currentOldHex);
                CreateBoard(size - 1, 5, currentOldHex);
                break;
            case 1://Went NorthEast
                if (currentOldHex.NorthEast.completed)
                    return;
                currentOldHex.NorthEast.SouthWest = currentOldHex;
                currentOldHex = currentOldHex.NorthEast;
                if (currentOldHex.completed)
                    return;
                checkCell = currentOldHex.SouthWest.SouthEast;
                if (checkCell != null)
                    currentOldHex.South = checkCell;
                else
                    currentOldHex.South = new OldHex();
                checkCell = currentOldHex.SouthWest.North;
                if (checkCell != null)
                    currentOldHex.NorthWest = checkCell;
                else
                    currentOldHex.NorthWest = new OldHex();
                checkCell = currentOldHex.South.NorthEast;
                if (checkCell != null)
                    currentOldHex.SouthEast = checkCell;
                else
                    currentOldHex.SouthEast = new OldHex();
                checkCell = currentOldHex.NorthWest.NorthEast;
                if (checkCell != null)
                    currentOldHex.North = checkCell;
                else
                    currentOldHex.North = new OldHex();
                if (currentOldHex.North.SouthEast != null)
                    currentOldHex.NorthEast = currentOldHex.North.SouthEast;
                else if (currentOldHex.SouthEast.North != null)
                    currentOldHex.NorthEast = currentOldHex.SouthEast.North;
                else
                    currentOldHex.NorthEast = new OldHex();
                currentOldHex.completed = true;
                //Recursion calls
                CreateBoard(size - 1, 0, currentOldHex);
                CreateBoard(size - 1, 1, currentOldHex);
                CreateBoard(size - 1, 2, currentOldHex);
                CreateBoard(size - 1, 3, currentOldHex);
                CreateBoard(size - 1, 4, currentOldHex);
                CreateBoard(size - 1, 5, currentOldHex);
                break;
            case 2://Went SouthEast
                if (currentOldHex.SouthEast.completed)
                    return;
                currentOldHex.SouthEast.NorthWest = currentOldHex;
                currentOldHex = currentOldHex.SouthEast;
                if (currentOldHex.completed)
                    return;
                checkCell = currentOldHex.NorthWest.South;
                if (checkCell != null)
                    currentOldHex.SouthWest = checkCell;
                else
                    currentOldHex.SouthWest = new OldHex();
                checkCell = currentOldHex.NorthWest.NorthEast;
                if (checkCell != null)
                    currentOldHex.North = checkCell;
                else
                    currentOldHex.North = new OldHex();
                checkCell = currentOldHex.SouthWest.SouthEast;
                if (checkCell != null)
                    currentOldHex.South = checkCell;
                else
                    currentOldHex.South = new OldHex();
                checkCell = currentOldHex.North.SouthEast;
                if (checkCell != null)
                    currentOldHex.NorthEast = checkCell;
                else
                    currentOldHex.NorthEast = new OldHex();
                if (currentOldHex.South.NorthEast != null)
                    currentOldHex.SouthEast = currentOldHex.South.NorthEast;
                else if (currentOldHex.NorthEast.South != null)
                    currentOldHex.SouthEast = currentOldHex.NorthEast.South;
                else
                    currentOldHex.SouthEast = new OldHex();
                currentOldHex.completed = true;
                //Recursion calls
                CreateBoard(size - 1, 0, currentOldHex);
                CreateBoard(size - 1, 1, currentOldHex);
                CreateBoard(size - 1, 2, currentOldHex);
                CreateBoard(size - 1, 3, currentOldHex);
                CreateBoard(size - 1, 4, currentOldHex);
                CreateBoard(size - 1, 5, currentOldHex);
                break;
            case 3://Went South
                if (currentOldHex.South.completed)
                    return;
                currentOldHex.South.North = currentOldHex;
                currentOldHex = currentOldHex.South;
                if (currentOldHex.completed)
                    return;
                checkCell = currentOldHex.North.SouthEast;
                if (checkCell != null)
                    currentOldHex.NorthEast = checkCell;
                else
                    currentOldHex.NorthEast = new OldHex();
                checkCell = currentOldHex.North.SouthWest;
                if (checkCell != null)
                    currentOldHex.NorthWest = checkCell;
                else
                    currentOldHex.NorthWest = new OldHex();
                checkCell = currentOldHex.NorthEast.South;
                if (checkCell != null)
                    currentOldHex.SouthEast = checkCell;
                else
                    currentOldHex.SouthEast = new OldHex();
                checkCell = currentOldHex.NorthWest.South;
                if (checkCell != null)
                    currentOldHex.SouthWest = checkCell;
                else
                    currentOldHex.SouthWest = new OldHex();
                if (currentOldHex.SouthEast.SouthWest != null)
                    currentOldHex.South = currentOldHex.SouthEast.SouthWest;
                else if (currentOldHex.SouthWest.SouthEast != null)
                    currentOldHex.South = currentOldHex.SouthWest.SouthEast;
                else
                    currentOldHex.South = new OldHex();
                currentOldHex.completed = true;
                //Recursion calls
                CreateBoard(size - 1, 0, currentOldHex);
                CreateBoard(size - 1, 1, currentOldHex);
                CreateBoard(size - 1, 2, currentOldHex);
                CreateBoard(size - 1, 3, currentOldHex);
                CreateBoard(size - 1, 4, currentOldHex);
                CreateBoard(size - 1, 5, currentOldHex);
                break;
            case 4://Went SouthWest
                if (currentOldHex.SouthWest.completed)
                    return;
                currentOldHex.SouthWest.NorthEast = currentOldHex;
                currentOldHex = currentOldHex.SouthWest;
                if (currentOldHex.completed)
                    return;
                checkCell = currentOldHex.NorthEast.South;
                if (checkCell != null)
                    currentOldHex.SouthEast = checkCell;
                else
                    currentOldHex.SouthEast = new OldHex();
                checkCell = currentOldHex.NorthEast.NorthWest;
                if (checkCell != null)
                    currentOldHex.North = checkCell;
                else
                    currentOldHex.North = new OldHex();
                checkCell = currentOldHex.SouthEast.SouthWest;
                if (checkCell != null)
                    currentOldHex.South = checkCell;
                else
                    currentOldHex.South = new OldHex();
                checkCell = currentOldHex.North.SouthWest;
                if (checkCell != null)
                    currentOldHex.NorthWest = checkCell;
                else
                    currentOldHex.NorthWest = new OldHex();
                if (currentOldHex.South.NorthWest != null)
                    currentOldHex.SouthWest = currentOldHex.South.NorthWest;
                else if (currentOldHex.NorthWest.South != null)
                    currentOldHex.SouthWest = currentOldHex.NorthWest.South;
                else
                    currentOldHex.SouthWest = new OldHex();
                currentOldHex.completed = true;
                //Recursion calls
                CreateBoard(size - 1, 0, currentOldHex);
                CreateBoard(size - 1, 1, currentOldHex);
                CreateBoard(size - 1, 2, currentOldHex);
                CreateBoard(size - 1, 3, currentOldHex);
                CreateBoard(size - 1, 4, currentOldHex);
                CreateBoard(size - 1, 5, currentOldHex);
                break;
            case 5://Went NorthWest
                if (currentOldHex.NorthWest.completed)
                    return;
                currentOldHex.NorthWest.SouthEast = currentOldHex;
                currentOldHex = currentOldHex.NorthWest;
                if (currentOldHex.completed)
                    return;
                checkCell = currentOldHex.SouthEast.SouthWest;
                if (checkCell != null)
                    currentOldHex.South = checkCell;
                else
                    currentOldHex.South = new OldHex();
                checkCell = currentOldHex.SouthEast.North;
                if (checkCell != null)
                    currentOldHex.NorthEast = checkCell;
                else
                    currentOldHex.NorthEast = new OldHex();
                checkCell = currentOldHex.South.NorthWest;
                if (checkCell != null)
                    currentOldHex.SouthWest = checkCell;
                else
                    currentOldHex.SouthWest = new OldHex();
                checkCell = currentOldHex.NorthEast.NorthWest;
                if (checkCell != null)
                    currentOldHex.North = checkCell;
                else
                    currentOldHex.North = new OldHex();
                if (currentOldHex.SouthWest.North != null)
                    currentOldHex.NorthWest = currentOldHex.SouthWest.North;
                else if (currentOldHex.North.SouthWest != null)
                    currentOldHex.NorthWest = currentOldHex.North.SouthWest;
                else
                    currentOldHex.NorthWest = new OldHex();
                currentOldHex.completed = true;
                //Recursion calls
                CreateBoard(size - 1, 0, currentOldHex);
                CreateBoard(size - 1, 1, currentOldHex);
                CreateBoard(size - 1, 2, currentOldHex);
                CreateBoard(size - 1, 3, currentOldHex);
                CreateBoard(size - 1, 4, currentOldHex);
                CreateBoard(size - 1, 5, currentOldHex);
                break;
        }
    }

}

/// <summary>
/// OldHex data object used to create the grid.
/// </summary>
public class OldHex
{
    public OldHex North, South, NorthEast, SouthEast, NorthWest, SouthWest;

    public bool completed;

    public OldHex()
    {
        completed = false;
    }
}