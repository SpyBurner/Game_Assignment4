using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneGlobalSetting : PersistentPhotonSingleton<SceneGlobalSetting>
{
    public bool isSinglePlayer;

    private void Start()
    {
    }

    public void SetSinglePlayer(bool isSinglePlayer)
    {
        this.isSinglePlayer = isSinglePlayer;
        PhotonNetwork.AutomaticallySyncScene = !isSinglePlayer;
        PhotonNetwork.OfflineMode = isSinglePlayer;
    }
}
