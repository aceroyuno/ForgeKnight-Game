using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instnatiates the list of actions for the battlemanager to use
/// And any other game play asects that must be made upon game launch
/// </summary>
public class ActionSystem : MonoBehaviour
{

    public IEnumerator DoAction(BattleAction ba, Pawn actor, HexGrid stage)
    {
        Debug.Log("Action performing");
        yield return StartCoroutine(actor.personalActions[ba.aid].Perform(ba, actor, stage));      
    }
}


public class BattleAction
{
    /// <summary>
    /// (Turns Till Complete) A counter to keep track of how long the action takes to complete
    /// </summary>
    public int ttc;
    /// <summary>
    /// (Activation Turn) The turn when the battle action activates 
    /// </summary>
    public int at;
    /// <summary>
    /// (Action Identifier) gets the pawns personal action to call Perform
    /// </summary>
    public int aid;

    public Targeter target;

    public BattleAction()
    {
        ttc = -1;
        at = -1;
        aid = -1;
    }

    public BattleAction(int ttc, int at, int aid)
    {
        this.ttc = ttc;
        this.at = at;
        this.aid = aid;
    }
}

/// <summary>
/// The base class for attacks, movements, and other battle options
/// </summary>
public class Action
{
    public int attackType;
    public string name;
    public int ttc, at;
    /// <summary>
    /// (Follow Up Action) the action performed after this one
    /// </summary>
    public int fua;

    //Range away from the player the action can be used
    public int range = 2;

    //Info for the targeter actions that do not have a target 
    //have their info set to zero so that it can act as a confirmation

    //Target info
    public int shape;
    public int height;
    public int width;


    //animation reference goes here



    public Action()
    {
        attackType = 0;
        ttc = -1;
        at = -1;
    }

    public Action(int ttc, int at)
    {
        attackType = 0;
        this.ttc = ttc;
        this.at = at;
    }

    public virtual IEnumerator Perform(BattleAction ba, Pawn actor, HexGrid stage)
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Rotate the character's part");
        actor.orientation += 90f;
        actor.UpdateRotationTracker();
    }
}

public class Attack : Action
{
    float damageMultiplier;
    float staggerValue;

    public Attack(int ttc, int at, int range, int shape, int height, int width)
    {
        attackType = 1;
        this.ttc = ttc;
        this.at = at;
        this.shape = shape;
        this.range = range;
        this.height = height;
        this.width = width;
    }

    public override IEnumerator Perform(BattleAction ba, Pawn actor, HexGrid stage)
    {
        //Animate
        //get targeter info and do with that whatever

        //Do damage
        foreach(Hex h in ba.target.targetHexes)
        {
            if (h.gamePiece != null)
            {
                Debug.Log("Hit: " + h.gamePiece.name);
            }
            else Debug.Log("Miss");
            yield return new WaitForSeconds(3.0f);
        }
    }
}

public class Movement : Action
{

    public Movement(int ttc, int at, int range)
    {
        attackType = 2;
        this.ttc = ttc;
        this.at = at;
        this.shape = 0;
        this.range = range;
        this.height = 0;
        this.width = 0;
    }

    public override IEnumerator Perform(BattleAction ba, Pawn actor, HexGrid stage)
    {
        Debug.Log("Move " + actor.name);
        yield return new WaitForSeconds(3.0f);
        actor.currentHex.gamePiece = null;
        actor.currentHex = ba.target.targetHexes[0];
        actor.currentHex.gamePiece = actor;
        actor.UpdatePawnPosition();     
    }
}

//public class Dodge : Movement
//{
//    int iFrames;

//    public override void Perform(BattleAction ba, Pawn actor, HexGrid stage)
//    {
//        actor.dodgeValue = iFrames;
//    }
//}

//public class Rotate : Action
//{
//    public override void Perform(BattleAction ba, Pawn actor, HexGrid stage)
//    {
//        //prompt player with what angle they would like the pawn to be

//    }
//}

//public class Passive : Action
//{

//    int passiveToActivate;

//    public override void Perform(BattleAction ba, Pawn actor, HexGrid stage)
//    {
//        if (passiveToActivate == 1)//blocking
//        {
//            actor.isBlocking = !actor.isBlocking;
//        }
//    }


//}


//Code I need to hold on to as it gives me the attack angles
//angleFromPlayer = Vector2.SignedAngle(Vector2.right,
//            new Vector2(targetCenter.x* 0.75f, targetCenter.y + (targetCenter.x* 0.5f)) 
//- new Vector2(playerHex.x* 0.75f, playerHex.y + (playerHex.x* 0.5f)));
/// <summary>
/// This class will serve the purpose of aiming attacks at locations
/// </summary>
public class Targeter
{
    public Hex[] targetHexes;
    float angleFromPlayer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="direction"></param>
    /// <param name="hieght"> Also Considered to be the radius for circles</param>
    /// <param name="width"></param>
    /// <param name="targetCenter"></param>
    public Targeter(int shape, int hieght, int width, Hex playerHex, Hex targetCenter, HexGrid hexMap)
    {
        switch(shape){
            case 0:
                FillCircleAroundHex(hieght, targetCenter, hexMap);
                break;
        }
    }

    private void FillCircleAroundHex(int radius, Hex targetCenter, HexGrid hexMap)
    {
        int arraySize = 1;
        for (int i = 1; i <= radius; i++)
        {
            arraySize += i * 6;
        }
        targetHexes = new Hex[arraySize];
        targetHexes[0] = targetCenter;
        int arrayCounter = 1;
        int counter = 0;
        for (int i = 1; i <= radius; i++)
        {
            int temp = counter;
            for (int j = 1; j <= i * 6; j++)
            {
                Vector2Int toAddToArray = new Vector2Int();
                if (j == i * 6)
                {
                    toAddToArray = new Vector2Int(targetHexes[temp].x, targetHexes[temp].y);
                }
                else if ((j - 1) % i != 0)
                {
                    counter++;
                    toAddToArray = new Vector2Int(targetHexes[counter].x, targetHexes[counter].y);
                }
                else
                {
                    toAddToArray = new Vector2Int(targetHexes[counter].x, targetHexes[counter].y);
                }
                toAddToArray += HexAxialTruths.GetAxialDirection((j - 1) / i % 6);
                targetHexes[arrayCounter] = hexMap.GetHex(toAddToArray);
                arrayCounter++;
            }
            counter++;
        }

    }

    private void FillLineFromPlayer(int hieght, int width, Hex playerHex, Hex targetCenter)
    {


    }

    private void FillTriangleFromPlayer(int height, Hex targetCenter, Hex playerHex)
    {

    }

    private void FillArcAroundPlayer(int hieght, int width, Hex playerHex, Hex targetCenter)
    {

    }

}


/// <summary>
/// This is to handle effects that actions
/// </summary>
public class Effects
{

}
