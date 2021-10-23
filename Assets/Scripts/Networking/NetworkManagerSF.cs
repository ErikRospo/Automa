using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using HeathenEngineering.SteamAPI;

public class NetworkManagerSF : NetworkManager
{

    public static NetworkManagerSF active;

    private bool isServer = false;

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

    public override void OnStartServer()
    {
        print("Started server");
        isServer = true;
        SteamSettings.Client.SetRichPresence("clientOf", SteamSettings.Client.user.id.ToString());
    }

    public override void OnStopServer()
    {
        isServer = false;
        SteamSettings.Client.ClearRichPresence();

    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            print("Started client");
            SteamSettings.Client.SetRichPresence("clientOf", networkAddress);
        }
    }

    public override void OnStopClient()
    {
        SteamSettings.Client.ClearRichPresence();
    }
}
