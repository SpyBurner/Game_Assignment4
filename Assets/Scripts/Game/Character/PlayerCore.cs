using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEditor.Timeline.Actions;

public class PlayerCore : MonoBehaviour
{
    public GameObject UIPrefab = null;

    private HexBoardGenerator board = null;
    public HexagonTile currentTile { get; private set; } = null;

    public int turnID { get; private set; } = -1;
    private TurnManager turnManager = null;

    public UnityEvent OnTurnStart;
    public UnityEvent OnTurnEnd;

    private PlayerStat playerStat = null;
    // Start is called before the first frame update
    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        board = HexBoardGenerator.Instance;
        turnManager = TurnManager.Instance;
        Invoke("InitPlayer", 0.5f);
    }

    void InitPlayer()
    {
        GameObject newUI = Instantiate(UIPrefab);
        newUI.GetComponent<UIUpdater>().player = gameObject;

        currentTile = board.GetSpawnPoint();

        turnID = turnManager.AddPlayer(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Set first tile
        if (currentTile == null || turnID == -1)
        {
            if (currentTile == null)
                Debug.Log("[" + gameObject.name + "] Current tile is null");
            if (turnID == -1)
                Debug.Log("[" + gameObject.name + "] Turn ID is -1");
            return;
        }

        transform.position = currentTile.transform.position;
    }

    public void StartTurn()
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        playerStat.ResetMana();
        OnTurnStart.Invoke();
    }

    public void EndTurn()
    {
        OnTurnEnd.Invoke();
    }

    public void Interact(HexagonTile otherTile)
    {
        if (otherTile == null)
            return;
        //Move
        if (otherTile.onThisTile == null)
        {
            if (currentTile.IsNeighbour(otherTile))
            {
                if (playerStat.UseMana(1))
                    currentTile = otherTile;
            }
        }
        else
        {
            //Attack
            PlayerStat otherPlayerStat = otherTile.onThisTile.GetComponent<PlayerStat>();

            if (otherPlayerStat != null)
            {
                if (currentTile.IsNeighbour(otherTile))
                {
                    if (playerStat.UseMana(0))
                        otherPlayerStat.TakeDamage(1);
                }
            }
        }
    }
}
