using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject steamworksBehavior;

    // Start is called before the first frame update
    private void Start()
    {
        Object.DontDestroyOnLoad(steamworksBehavior);
        SceneManager.LoadSceneAsync(1); // Load the main menu scene
    }

    // Loading animation
    private void Update()
    {
        
    }
}
