using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using HeathenEngineering.SteamAPI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Active instance
    public static MainMenu active;

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
        active = this;
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
            networkManager.maxConnections = (int) maxPlayersSlider.value + 1;
        } 
        else
        {
            // Singleplayer settings
            networkManager.maxConnections = 0;
        }

        // Start the game
        networkManager.StartHost();
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

        // Put list in alphabetical order
        unsorted = unsorted.OrderBy(friend => friend.DisplayName).ToList();

        // Get friends currently in-game
        for (int i = 0; i < unsorted.Count; i++)
        {
            if (unsorted[i].GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
            {
                sorted.Add(unsorted[i]);
                unsorted.Remove(unsorted[i]);
                i--;
            }
        }

        // Get online friends
        for (int i = 0; i < unsorted.Count; i++)
        {
            if (unsorted[i].State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateOnline))
            {
                sorted.Add(unsorted[i]);
                unsorted.Remove(unsorted[i]);
                i--;
            }
        }

        // Get away friends
        for (int i = 0; i < unsorted.Count; i++)
        {
            if (unsorted[i].State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateAway))
            {
                sorted.Add(unsorted[i]);
                unsorted.Remove(unsorted[i]);
                i--;
            }
        }

        // Add remaining friends
        sorted.AddRange(unsorted);

        return sorted;
    }
}
