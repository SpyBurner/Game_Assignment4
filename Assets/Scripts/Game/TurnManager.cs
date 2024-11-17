using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public int turnID = 0;
    public List<GameObject> players;

    public UnityEvent OnAdvanceTurn;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(turnID);
        }
        else
        {
            turnID = (int)stream.ReceiveNext();
        }
    }
}
