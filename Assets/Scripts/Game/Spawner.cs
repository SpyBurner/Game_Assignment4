using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject TurnManagerPrefab;
    public GameObject BoardPrefab;

    private bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnObjects", .3f);
    }

    void SpawnObjects()
    {
        if (spawned) return;
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            Invoke("SpawnObjects", .3f);
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(TurnManagerPrefab.name, Vector3.zero, Quaternion.identity);
            PhotonNetwork.Instantiate(BoardPrefab.name, BoardPrefab.transform.position, Quaternion.identity);
        }
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        spawned = true;
    }

}
