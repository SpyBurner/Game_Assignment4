using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RoomManager : PhotonSingleton<RoomManager>
{
    public Text roomNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (roomNameText == null)
        {
            if (!PhotonNetwork.OfflineMode)
                throw new System.Exception("Room name text is not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.OfflineMode)
        {
            Debug.Log("Creating offline room");
            PhotonNetwork.CreateRoom(null);
            return;
        }
        string roomName = roomNameText.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is empty");
            return;
        }

        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom()
    {
        string roomName = roomNameText.text;


        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is empty");
            return;
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join room failed: " + message);
    }
}
