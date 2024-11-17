using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerCore : MonoBehaviour
{
    public GameObject UIPrefab = null;

    private HexBoardGenerator board = null;
    private HexagonTile currentTile = null;

    private int turnID = -1;
    private TurnManager turnManager = null;

    public UnityEvent OnTurnStart;
    public UnityEvent OnTurnEnd;
    // Start is called before the first frame update
    void Start()
    {
        board = HexBoardGenerator.Instance;
        turnManager = TurnManager.Instance;
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
                Debug.Log("Current tile is null");
            if (turnID == -1)
                Debug.Log("Turn ID is -1");
            InitPlayer();
        }

        transform.position = currentTile.transform.position;
    }

    public void StartTurn()
    {
        OnTurnStart.Invoke();
    }

    public void EndTurn()
    {
        OnTurnEnd.Invoke();
    }
}
