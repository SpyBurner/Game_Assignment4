using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : PhotonSingleton<TurnManager>
{
    public int turnID = 0;
    public List<GameObject> players;

    public UnityEvent OnAdvanceTurn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int AddPlayer(GameObject player)
    {
        players.Add(player);
        return players.Count - 1;
    }

    public GameObject GetCurrentPlayer()
    {
        return players[turnID];
    }

    public void AdvanceTurn()
    {
        GetCurrentPlayer().GetComponent<PlayerCore>().EndTurn();

        turnID++;
        if (turnID >= players.Count)
        {
            turnID = 0;
        }

        GetCurrentPlayer().GetComponent<PlayerCore>().StartTurn();

        OnAdvanceTurn.Invoke();
    }
}
