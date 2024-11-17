using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitRoom : MonoBehaviourPunCallbacks
{
    public Text playerCountText;

    public int minPlayers = 2;
    public int maxPlayers = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCountText != null)
        {
            playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + maxPlayers;
        }
    }

    public void StartGame()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= minPlayers)
                {
                    PhotonNetwork.LoadLevel("Game");
                }
            }
        }
        else
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
