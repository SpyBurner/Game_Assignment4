using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

//Static instance base class
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
};

public abstract class StaticPhotonInstance<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;
    protected virtual void OnApplicationQuit()
    {
        Instance = null;

        if (GetComponent<PhotonView>())
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);

    }
};

//Singleton, destroy on scene load
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        //Destroy self if this is a duplicated instance
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

public abstract class PhotonSingleton<T> : StaticPhotonInstance<T> where T : MonoBehaviourPunCallbacks
{
    protected override void Awake()
    {
        //Destroy self if this is a duplicated instance
        if (Instance != null)
        {
            if (GetComponent<PhotonView>())
                PhotonNetwork.Destroy(gameObject);
            else
                Destroy(gameObject);
        }
        base.Awake();
    }
}

//Singleton, stay between scene loads
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        if (transform.parent == null) DontDestroyOnLoad(gameObject);
    }
}

public abstract class PersistentPhotonSingleton<T> : PhotonSingleton<T> where T : MonoBehaviourPunCallbacks
{
    protected override void Awake()
    {
        base.Awake();
        if (transform.parent == null) DontDestroyOnLoad(gameObject);
    }
}