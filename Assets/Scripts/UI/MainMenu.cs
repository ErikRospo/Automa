using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Canvases
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas hostCanvas;
    [SerializeField] private Canvas joinCanvas;

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
        // Method stub to start hosting a game with configured options
    }

    public void Join()
    {
        // Method stub to join a game via a steamid64
    }
}
