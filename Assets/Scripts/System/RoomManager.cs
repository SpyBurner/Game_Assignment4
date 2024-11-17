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
    }

    // Update is called once per frame
    void Update()
    {
        if (roomNameText != null && PhotonNetwork.InRoom)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        }
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.OfflineMode)
        {
            Debug.Log("Creating offline room");
            PhotonNetwork.CreateRoom(null);
            return;
        }

        if (roomNameText == null)
        {
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
        if (roomNameText == null)
        {
            Debug.Log("Room name text is null");
            return;
        }
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
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.Log("Not in room");
        }
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
        PhotonNetwork.LoadLevel("Selection");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join room failed: " + message);
    }
}
