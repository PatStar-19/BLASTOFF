using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelManager : MonoBehaviour
{
    public Toggle musicToggle;

    void Start()
    {
        // Add listener to the toggle to handle music on/off
        musicToggle.onValueChanged.AddListener(ToggleMusic);
    }

    void ToggleMusic(bool isOn)
    {
        // Implement music on/off logic here
        if (isOn)
        {
            // Turn on music
            Debug.Log("Music turned on");
        }
        else
        {
            // Turn off music
            Debug.Log("Music turned off");
        }
    }

    public void BackToMainMenu()
    {
        // Implement back to main menu logic here
        Debug.Log("Back to Main Menu");
    }
}
