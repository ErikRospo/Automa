using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Canvases
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas hostCanvas;
    [SerializeField] private Canvas joinCanvas;

    // Network Manager
    [SerializeField] private NetworkManager networkManager;

    // Host Options
    [SerializeField] private Slider maxPlayersSlider;

    // Join Options

    // Switch canvas to main screen
    public void GoToMainCanvas()
    {
        mainCanvas.gameObject.SetActive(true);
        hostCanvas.gameObject.SetActive(false);
        joinCanvas.gameObject.SetActive(false);
    }

    // Switch canvas to host screen
    public void GoToHostCanvas()
    {
        mainCanvas.gameObject.SetActive(false);
        hostCanvas.gameObject.SetActive(true);
        joinCanvas.gameObject.SetActive(false);
    }

    // Switch canvas to join screen
    public void GoToJoinCanvas()
    {
        mainCanvas.gameObject.SetActive(false);
        hostCanvas.gameObject.SetActive(false);
        joinCanvas.gameObject.SetActive(true);
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
}
