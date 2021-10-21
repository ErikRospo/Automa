using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using HeathenEngineering.SteamAPI;

public class MainMenu : MonoBehaviour
{
    // Screens
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject hostScreen;
    [SerializeField] private GameObject joinScreen;

    // Network Manager
    [SerializeField] private NetworkManager networkManager;

    // Host Options
    [SerializeField] private Slider maxPlayersSlider;

    // Join Options

    // Switch canvas to main screen
    public void GoToMainCanvas()
    {
        mainScreen.SetActive(true);
        hostScreen.SetActive(false);
        joinScreen.SetActive(false);
    }

    // Switch canvas to host screen
    public void GoToHostCanvas()
    {
        mainScreen.SetActive(false);
        hostScreen.SetActive(true);
        joinScreen.SetActive(false);
    }

    // Switch canvas to join screen
    public void GoToJoinCanvas()
    {
        mainScreen.SetActive(false);
        hostScreen.SetActive(false);
        joinScreen.SetActive(true);
        UpdateFriendsList();
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
