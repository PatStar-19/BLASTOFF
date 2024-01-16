using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;

    public AudioClip startClip;
    public AudioClip gameClip;
    public AudioClip endClip;

    private AudioSource music;

    public Toggle audioToggle; // Reference to the UI toggle

    private const string MuteKey = "IsMuted"; // Key to store mute state in PlayerPrefs

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate music player self-destructing!");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);

            // Ensure there's an AudioSource component
            music = GetComponent<AudioSource>();
            if (music == null)
            {
                music = gameObject.AddComponent<AudioSource>();
            }

            // Set up the UI toggle listener
            if (audioToggle != null)
            {
                audioToggle.onValueChanged.AddListener(OnAudioToggleChanged);
            }

            // Retrieve the mute state from PlayerPrefs
            bool isMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
            audioToggle.isOn = !isMuted;

            // Set the initial state of the audio source based on the toggle's state
            music.mute = isMuted;

            // Set the default clip and loop it
            music.clip = startClip;
            music.loop = true;
            music.Play();

            // Subscribe to scene changes to update the audio toggle state
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // Add debug logs in the MusicPlayer script
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the audio toggle state based on the music mute state
        if (audioToggle != null)
        {
            audioToggle.isOn = !music.mute;
        }

        // Play the appropriate music for the loaded scene
        PlayMusicForScene(scene.name);
    }


    void OnAudioToggleChanged(bool isOn)
    {
        // Toggle audio based on the UI toggle state
        music.mute = !isOn;

        // Save the mute state to PlayerPrefs
        PlayerPrefs.SetInt(MuteKey, isOn ? 0 : 1);
        PlayerPrefs.Save();

        // If toggled on, play the music
        if (isOn)
        {
            music.Play();
        }
        else
        {
            music.Stop();
        }
    }

    // Expose a method to toggle the audio state
    public void ToggleAudio()
    {
        // Toggle the audio based on the current state
        music.mute = !music.mute;

        // Save the mute state to PlayerPrefs
        PlayerPrefs.SetInt(MuteKey, music.mute ? 1 : 0);
        PlayerPrefs.Save();

        // If toggled on, play the music
        if (!music.mute)
        {
            music.Play();
        }
    }

    void PlayMusicForScene(string sceneName)
    {
        // Determine which music clip to play based on the scene
        AudioClip clipToPlay = null;
        float volume = 0.04f; // Default volume

        switch (sceneName)
        {
            case "Start Menu":
                clipToPlay = startClip;
                volume = 0.3f;
                break;
            case "Game":
            case "Game 2":
            case "Game 3":
            case "Game 4":
                clipToPlay = gameClip;
                volume = 0.04f;
                break;
            case "Win Screen":
                clipToPlay = endClip;
                volume = 0.04f;
                break;
            // Add more cases for other scenes if needed

            // Default case (e.g., for scenes without specific music)
            default:
                clipToPlay = startClip;
                break;
        }
        music.volume = volume;
        // Play the selected music clip and loop it
        music.clip = clipToPlay;
        music.loop = true;
        music.Play();
    }
}
