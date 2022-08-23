using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(ActionSystem))]
public class BattleManager : MonoBehaviour
{
    ActionSystem actionHandler;
    public HexGrid battleMap;
    public int mapSize;
    GameObject[,] graphicMap;
    public GameObject tilePrefab, backgroundPrefab;
    public GameObject camera;

    public GameObject cursor;//Gonna move via arrow keys for now
    Hex cursorHex;

    void MoveCursor(int dir)
    {
        Vector2Int change = new Vector2Int();
        switch(dir)
        {
            case 0://up
                change = HexAxialTruths.GetAxialDirection(0);//north
                break;
            case 1://right
                change = HexAxialTruths.GetAxialDirection(1);//north-east
                break;
            case 2://down
                change = HexAxialTruths.GetAxialDirection(3);//south
                break;
            case 3://left
                change = HexAxialTruths.GetAxialDirection(4);//south-west
                break;
            default://used for intialization
                change = Vector2Int.zero;
                break;
        }
        if (battleMap.GetHex(cursorHex.x + change.x, cursorHex.y + change.y) == null)
            return;
        cursorHex = battleMap.GetHex(cursorHex.x + change.x, cursorHex.y + change.y);
        cursor.transform.position = new Vector3(cursorHex.x * 0.75f, cursorHex.y + (cursorHex.x * 0.5f), 0);
        camera.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, camera.transform.position.z);
        Debug.Log(cursorHex.x + " " + cursorHex.y + " " + cursorHex.z);
    }
    


    List<Pawn> gamePieces = new List<Pawn>();
    bool pawnsToDecide;

    List<BattleAction> attackqueue = new List<BattleAction>();

    private void Start()
    {
        actionHandler = gameObject.GetComponent<ActionSystem>();
        Pawn.rotationSprite = Resources.Load<Sprite>("pointer");
        battleMap = new HexGrid(mapSize, 0);
        DrawMap();
        gamePieces.Add(new Pawn(playerTemp, battleMap.GetHex(mapSize / 2, mapSize / 2), "Player"));
        gamePieces[0].personalActions.Add(new Attack(5, 3, 1, 0, 0, 0));
        gamePieces[0].personalActions.Add(new Movement(3, 2, 1));
        gamePieces[0].needsToAct = true;
        gamePieces[0].currentHex.gamePiece = gamePieces[0];
        attackqueue.Add(new BattleAction());
        gamePieces.Add(new Pawn(playerTemp, battleMap.GetHex((mapSize / 2) + 1, mapSize / 2), "Goblin"));
        gamePieces[1].personalActions.Add(new Action(7,4));
        gamePieces[1].needsToAct = true;
        gamePieces[1].currentHex.gamePiece = gamePieces[1];
        attackqueue.Add(new BattleAction());
        cursorHex = battleMap.GetHex(mapSize / 2, mapSize / 2);
        MoveCursor(-1);
        StartBattle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveCursor(0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveCursor(1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveCursor(2);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveCursor(3);
        if(!actionInputed)
        {
            PlayerActionSelect();
        }
        else if(!targetSet)
        {
            ActionTargetSelect();
        }
        if (Input.GetKeyDown(KeyCode.Z))
            battleOngoing = false;
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    float angleFromPlayer = Vector2.SignedAngle(Vector2.up,
        //    new Vector2(cursorHex.x * 0.75f, cursorHex.y + (cursorHex.x * 0.5f)) - 
        //    new Vector2(Player.currentHex.x * 0.75f, Player.currentHex.y + (Player.currentHex.x * 0.5f)));
        //    Debug.Log(angleFromPlayer);
        //    Player.orientation = angleFromPlayer;
        //    Player.UpdateRotationTracker();
        //}
    }

    private void PlayerActionSelect()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            actionNum--;
            if (actionNum < 0)
                actionNum = 0;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            actionNum++;
            if (actionNum > Player.personalActions.Count -1)
                actionNum = Player.personalActions.Count - 1;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //attackqueue[playerPiece] = Player.SelectNewBattleAction(actionNum);
            actionInputed = true;
        }
    }

    private void ActionTargetSelect()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            for(int i = 0; i < potentialTargetHexes.Length; i++)
            {
                if(cursorHex == potentialTargetHexes[i])
                {
                    Action grabAction = Player.personalActions[actionNum];
                    attackTarget = new Targeter(grabAction.shape, grabAction.height, grabAction.width,
                        Player.currentHex, potentialTargetHexes[i], battleMap);
                    targetSet = true;
                    if (Player.personalActions[actionNum].attackType == 1)
                    {
                        float angleFromPlayer = Vector2.SignedAngle(Vector2.up,
                          new Vector2(attackTarget.targetHexes[0].x * 0.75f, attackTarget.targetHexes[0].y + (attackTarget.targetHexes[0].x * 0.5f)) - 
                          new Vector2(Player.currentHex.x * 0.75f, Player.currentHex.y + (Player.currentHex.x * 0.5f)));
                        Player.orientation = angleFromPlayer;
                        Player.UpdateRotationTracker();
                    }
                    actionPicked = true;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            targetSet = true;
        }
    }

    public Sprite playerTemp;
    Pawn Player
    {
        get { return gamePieces[0]; }
    }

    public void DrawMap()
    {
        int size = battleMap.size;
        graphicMap = new GameObject[size, size];
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if (battleMap.gridBoard[x,y] == null)
                {
                    graphicMap[x, y] = Instantiate(backgroundPrefab, new Vector3(x * 0.75f, y + (x * 0.5f),0), 
                        gameObject.transform.rotation, gameObject.transform);
                }
                else
                {
                    graphicMap[x, y] = Instantiate(tilePrefab, new Vector3(x * 0.75f, y + (x * 0.5f), 0), 
                        gameObject.transform.rotation, gameObject.transform);
                }
            }
        }
    }


    public void StartBattle()
    {
        battleOngoing = true;
        pawnsToDecide = true;
        StartCoroutine(Battle());
    }

    public bool battleOngoing;
    private IEnumerator Battle()
    {
        while (battleOngoing)
        {
            yield return StartCoroutine(GetPawnActions());
            Debug.Log("pawns decided");
            //SimulateTurn
            yield return StartCoroutine(SimulateTurn()); 
            Debug.Log("Pretend turn was simulated");

        }
        Debug.Log("BattleEnded");
    }

    private IEnumerable EndBattle()
    {
        return null;
    }

    private IEnumerator SimulateTurn()
    {
        for (int i = 0; i < attackqueue.Count; i++)
        {
            Debug.Log(gamePieces[i].name + " is taking thier turn");
            attackqueue[i].ttc--;
            if (attackqueue[i].ttc == attackqueue[i].at)
            {
                Debug.Log(gamePieces[i].name + " is performing an action");
                //Make this an enumerable for any animations that have to play
                yield return StartCoroutine(actionHandler.DoAction(attackqueue[i], gamePieces[i], battleMap));
            }
            if (attackqueue[i].ttc == 0)
            {
                gamePieces[i].needsToAct = true;
                attackqueue[i] = null;
            }
        }
        yield return null;
    }

    public int actionNum;
    public Targeter attackTarget;

    private IEnumerator GetPawnActions()
    {
        if (!pawnsToDecide)
            yield break;
        for (int i = 0; i < gamePieces.Count; i++)
        {

            if(gamePieces[i].needsToAct)
            {
                if (gamePieces[i] == Player)// == Player will eventually become a method to check if it is a players piece
                {
                    yield return StartCoroutine(SetPlayerAction(i));
                }
                else
                {
                    //Handle Enemy AI
                    actionNum = 0;
                }
                attackqueue[i] = gamePieces[i].SelectNewBattleAction(actionNum);
                attackqueue[i].target = attackTarget;
                gamePieces[i].needsToAct = false;
            }
        }
    }

    public bool actionInputed;
    public bool ationParamSet;
    private bool actionPicked;
    private bool targetSet;
    private IEnumerator SetPlayerAction(int pawn)
    {
        actionPicked = false;
        while (!actionPicked)
        {
            Debug.Log("Player Action Required");
            actionInputed = false;
            actionNum = 0;
            yield return new WaitUntil(() => actionInputed);
            Debug.Log(actionNum);
            targetSet = false;
            //Create the potential targets for the attack
            PlacePotentialAttackTargets();
            yield return new WaitUntil(() => targetSet);
            foreach (GameObject g in potentialTargetOverlay)
            {
                Destroy(g);
            }
        }
    }

    GameObject[] potentialTargetOverlay;
    Hex[] potentialTargetHexes;
    private void PlacePotentialAttackTargets()
    {
        int range = Player.personalActions[actionNum].range;
        int arraySize = 1;
        for(int i = 1; i <= range; i++)
        {
            arraySize += i * 6;
        }
        potentialTargetHexes = new Hex[arraySize];
        potentialTargetOverlay = new GameObject[arraySize];
        potentialTargetHexes[0] = Player.currentHex;
        //Instantiate gameobject on top of
        int arrayCounter = 1;
        int counter = 0;
        for (int i = 1; i <= range; i++)
        {
            int temp = counter;
            for(int j = 1; j <= i*6; j++)
            {
                Vector2Int toAddToArray = new Vector2Int();
                if (j == i * 6)
                {
                    toAddToArray = new Vector2Int(potentialTargetHexes[temp].x, potentialTargetHexes[temp].y);
                }
                else if ((j-1)%i != 0)
                {
                    counter++;
                    toAddToArray = new Vector2Int(potentialTargetHexes[counter].x, potentialTargetHexes[counter].y);
                }
                else
                {
                    toAddToArray = new Vector2Int(potentialTargetHexes[counter].x, potentialTargetHexes[counter].y);
                }
                toAddToArray += HexAxialTruths.GetAxialDirection((j-1)/i%6);
                potentialTargetHexes[arrayCounter] = battleMap.GetHex(toAddToArray);
                arrayCounter++;
            }
            counter++;
        }

        for(int i = 0; i < potentialTargetHexes.Length; i++)
        {
            potentialTargetOverlay[i] = Instantiate(cursor, new Vector3(potentialTargetHexes[i].x * 0.75f, 
                potentialTargetHexes[i].y + (potentialTargetHexes[i].x * 0.5f), 0),
                        gameObject.transform.rotation, gameObject.transform);
        }
    }

    private bool HasParams(int pawn, int actionNum)
    {

        return true;
    }

    public void UpdateGraphics()
    {

    }
}


