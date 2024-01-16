using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; } = false;
    public GameObject pauseMenu;

    void Start()
    {
   
        pauseMenu.SetActive(false);
    }

    void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f; 
            pauseMenu.SetActive(true); 
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false); 
        }
    }

    public void ResumeGame()
    {
        TogglePause(); 
    }

    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

 
        Time.timeScale = 1f;


        IsPaused = false;
        pauseMenu.SetActive(false);
    }

    public void MainMenu()
    {

        SceneManager.LoadScene(0);

        Time.timeScale = 1f;

        IsPaused = false;
        pauseMenu.SetActive(false);
    }
}
