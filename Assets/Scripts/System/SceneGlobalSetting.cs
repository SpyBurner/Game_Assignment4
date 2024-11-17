using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneGlobalSetting : PersistentPhotonSingleton<SceneGlobalSetting>
{
    public bool isSinglePlayer;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetSinglePlayer(bool isSinglePlayer)
    {
        this.isSinglePlayer = isSinglePlayer;
        PhotonNetwork.OfflineMode = isSinglePlayer;
    }

}
