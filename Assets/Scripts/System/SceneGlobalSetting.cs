using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneGlobalSetting : MonoBehaviour
{
    public bool isSinglePlayer;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.OfflineMode = isSinglePlayer;    
    }

}
