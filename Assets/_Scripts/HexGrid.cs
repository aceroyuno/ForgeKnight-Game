using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{

    public Hex[,] gridBoard;
    public int size;

    public HexGrid(int size, int shape)
    {
        this.size = size;
        switch (shape)
        {
            case 0:
                PopulateGrid_Hexagon(size);
                break;
            case 1:
                PopulateGrid_Rhombus(size);
                break;
            default:
                break;
        }
    }

    public Hex GetHex(Vector2Int v2)
    {
        return GetHex(v2.x, v2.y);
    }

    public Hex GetHex(int x, int y)
    {
        return gridBoard[x,y];
    }


    private void PopulateGrid_Hexagon(int size)
    {
        gridBoard = new Hex[size,size];
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                if (j + i < (int)(size * .5) || j + i >= (int)(size * 1.5))
                    continue;
                gridBoard[i, j] = new Hex(i, j);
            }
        }
    }
    private void PopulateGrid_Rhombus(int size)
    {
        gridBoard = new Hex[size, size];
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                gridBoard[i, j] = new Hex(i, j);
            }
        }
    }
}

/// <summary>
/// The Hex data type designed to store tile information for rendering and runnning the scene
/// 
/// </summary>
public class Hex
{
    bool obstacle;
    int elevation;//?
    int terrainID;
    public int x, y, z;
    public Pawn gamePiece;

    public Hex(int x, int y)
    {
        this.x = x;
        this.y = y;
        z = -x - y;
    }
}

/// <summary>
/// This is a static class of fundamental truths of the hex grid to be used for calculations
/// </summary>
public static class HexAxialTruths
{
    static Vector2Int[] AxialDirections =
    {
        new Vector2Int(0, 1),//North
        new Vector2Int(1, 0),//North-East
        new Vector2Int(1, -1),//South-East
        new Vector2Int(0, -1),//South
        new Vector2Int(-1, 0),//South-West
        new Vector2Int( -1, 1) //North-West
    };

    public static Vector2Int GetAxialDirection(int direction)
    {
        return AxialDirections[direction];
    }



}
