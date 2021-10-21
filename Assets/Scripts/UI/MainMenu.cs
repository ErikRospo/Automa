using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using HeathenEngineering.SteamAPI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    // Screens
    private CanvasGroup[] canvasGroups;

    // Network Manager
    [SerializeField] private NetworkManager networkManager;

    // Host Options
    [SerializeField] private Slider maxPlayersSlider;
    private bool multiplayer = false;

    // Join Options

    private void Start()
    {
        canvasGroups = this.gameObject.GetComponentsInChildren<CanvasGroup>();
    }

    // Switch shown canvas group
    public void SwitchCanvasGroup(CanvasGroup canvasGroup)
    {
        foreach(CanvasGroup group in canvasGroups) 
        {
            group.alpha = 0;
            group.blocksRaycasts = false;
            group.interactable = false;
        }

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Host()
    {
        // Set max players setting
        networkManager.maxConnections = (int) maxPlayersSlider.value;

        // Start host
        networkManager.StartHost();
    }

    public void Join()
    {
        // Method stub to join a game via a steamid64
    }

    public void UpdateMaxPlayers()
    {
        maxPlayersSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Max Players ({(int) maxPlayersSlider.value})";
    }

    public void UpdateFriendsList()
    {
        foreach (UserData friend in SteamSettings.Client.ListFriends())
        {
            Debug.Log(friend.DisplayName);
        }
    }
}
