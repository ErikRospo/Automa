using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerSF : NetworkManager
{

    public static NetworkManagerSF active;

    public override void Start()
    {
        active = this;
        base.Start();
    }

    public void Join(string id)
    {
        // Set address to steamid64
        networkAddress = id;

        // Switch scenes
        SceneManager.LoadScene(2);

        // Connect
        StartClient();
    }
}
