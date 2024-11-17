using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SceneLoadOnClick : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PhotonNetwork.LoadLevel(sceneName);
        });
    }
}
