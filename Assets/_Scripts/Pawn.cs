using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn
{
    public GameObject gamePiece;
    private GameObject rotationTracker;
    public string name;

    public Hex currentHex;
    int x, y; //coordinate of hex in the array (is redundent as it can be gotten from currentHex)

    public float orientation; //0-360 degrees of rotation to determine where attacks wil land
    public static Sprite rotationSprite;

    //Battle Stats
    int health;
    int stamina;
    DefenseStatMatrix pawnsDefenses;

    public bool needsToAct;

    //pawn stats

    //fix this
    public bool isBlocking;

    //A list to contain the int id of other passive effects currently active like weapon buffs
    public List<Action> personalActions;


    public Pawn(Sprite image, Hex space, string name)
    {
        this.name = name;
        currentHex = space;
        x = currentHex.x;
        y = currentHex.y;
        gamePiece = new GameObject(image.name);
        gamePiece.AddComponent<SpriteRenderer>();
        gamePiece.GetComponent<SpriteRenderer>().sprite = image;
        gamePiece.transform.position = new Vector3(x * 0.75f, y + (0.5f * x), 0);
        rotationTracker = new GameObject("RotationTracker");
        rotationTracker.AddComponent<SpriteRenderer>();
        rotationTracker.GetComponent<SpriteRenderer>().sprite = rotationSprite;
        rotationTracker.transform.SetParent(gamePiece.transform);
        rotationTracker.transform.localPosition = Vector3.zero;
        personalActions = new List<Action>();
        needsToAct = true;
    }

    public void UpdateRotationTracker()
    {
        Vector3 rot = new Vector3(0, 0, orientation);
        rotationTracker.transform.localEulerAngles = rot;

    }

    //return type of BattleAction
    public BattleAction SelectNewBattleAction(int actionNum)
    {
        return new BattleAction(personalActions[actionNum].ttc, personalActions[actionNum].at, actionNum);
    }

    public void UpdatePawnPosition()
    {
        gamePiece.transform.position = new Vector3(currentHex.x * 0.75f, currentHex.y + (0.5f * currentHex.x), 0);
    }

}

public static class EnemyAI
{

}
