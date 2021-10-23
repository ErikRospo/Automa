using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerSF : NetworkManager
{

    public static NetworkManagerSF active;

    public void Start()
    {
        base.Start();
        active = this;
    }

    public void Join(string id)
    {
        // Set address to steamid64
        networkAddress = id;

        // Switch scenes
        SceneManager.LoadSceneAsync(2);

        // Connect
        StartClient();
    }
}
