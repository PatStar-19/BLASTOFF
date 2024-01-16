using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string name)
    {
        Debug.Log("New Level load: " + name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void QuitRequest()
    {
        Debug.Log("Quit requested");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
