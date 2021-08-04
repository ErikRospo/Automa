using UnityEngine;
using System.IO;

public static class SaveData
{
    // File locations
    const string PATH_KEYBINDS = "/keybinds.json";
    const string PATH_SETTINGS = "/settings.json";

    public static void SaveKeybinds()
    {
        // Get file path
        string filePath = Application.persistentDataPath + PATH_KEYBINDS;
        
        // Check if the file exists
        if (File.Exists(filePath))
        {
            FileStream a = new FileStream(filePath, FileMode.Open);
            
            // do stuff

            a.Close();
            return;
        }
        else Debug.Log("Keybinds file does not exist. Generating one.");

        
    }
}
