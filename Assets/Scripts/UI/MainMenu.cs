using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using HeathenEngineering.SteamAPI;
using HeathenEngineering.Tools;
using System.Collections.Generic;
using HeathenEngineering.Events;

public class MainMenu : MonoBehaviour
{
    // Screens
    private CanvasGroup[] canvasGroups;

    // Network Manager
    [SerializeField] private NetworkManager networkManager;

    // Host Options
    [SerializeField] private Slider maxPlayersSlider;

    // Join Options
    [SerializeField] private JoinButton joinButton;
    [SerializeField] private Transform joinList;

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

    public void StartNewGame(bool multiplayer)
    {
        if (multiplayer)
        {
            // Multiplayer settings
            networkManager.maxConnections = (int) maxPlayersSlider.value;
        } 
        else
        {
            // Singleplayer settings
            networkManager.maxConnections = 0;
        }

        // Start the game
        networkManager.StartHost();
    }

    public void Join()
    {
        // Method stub to join a game via a steamid64
    }

    public void UpdateMaxPlayers()
    {
        maxPlayersSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Max Friends ({(int) maxPlayersSlider.value})";
    }

    public void UpdateFriendsList()
    {
        List<UserData> friends = SortFriendsList(SteamSettings.Client.ListFriends());

        foreach (UserData friend in friends)
        {
            JoinButton button = Instantiate(joinButton.gameObject, Vector3.zero, Quaternion.identity).GetComponent<JoinButton>();
            button.SetUserData(friend);
            button.UpdateUserData();
            button.GetComponent<RectTransform>().localScale = new Vector3(.8f, .8f, .8f);
            button.transform.SetParent(joinList);
        }
    }

    private List<UserData> SortFriendsList(List<UserData> friends)
    {
        List<UserData> unsorted = friends;
        List<UserData> sorted = new List<UserData>();

        // Get friends currently in-game
        foreach (UserData friend in unsorted)
        {
            if (friend.GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
            {
                sorted.Add(friend);
                unsorted.Remove(friend);
            }
        }

        return sorted;
    }
}
