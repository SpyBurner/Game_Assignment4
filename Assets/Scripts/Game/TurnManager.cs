using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(turnID);
            stream.SendNext(players.Count);
            for (int i = 0; i < players.Count; i++)
            {
                stream.SendNext(players[i].GetComponent<PhotonView>().ViewID);
            }
        }
        else
        {
            turnID = (int)stream.ReceiveNext();
            int count = (int)stream.ReceiveNext();
            players.Clear();
            for (int i = 0; i < count; i++)
            {
                int viewID = (int)stream.ReceiveNext();
                players.Add(PhotonView.Find(viewID).gameObject);
            }
        }
    }
}
