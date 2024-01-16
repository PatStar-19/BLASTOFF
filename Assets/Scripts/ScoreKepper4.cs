using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreKepper4 : MonoBehaviour
{
    public static int score = 0;
    private Text myText;
    public GameObject player; // Reference to the player object
    public Vector3 playerAwayPosition; // Position to move the player away
    public float flyAwayDuration = 2f; // Duration of the fly-away animation
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public Text completionText; // Reference to the completion text element

    private bool hasReachedTargetScore = false;

    void Start()
    {
        myText = GetComponent<Text>();
        Reset();
    }

    public void Score(int points)
    {
        score += points;
        myText.text = score.ToString();

        if (score >= 30000 && !hasReachedTargetScore)
        {
            hasReachedTargetScore = true;
            StartCoroutine(ExplodeEnemiesAndFlyPlayer());
        }
    }

    public bool HasReachedTargetScore()
    {
        return hasReachedTargetScore;
    }

    private IEnumerator ExplodeEnemiesAndFlyPlayer()
    {
        // Find and explode all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Instantiate explosion animation
            Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);

            // Disable the enemy for the duration of the explosion
            enemy.SetActive(false);
        }

        // Display completion text for 3 seconds
        if (completionText != null)
        {
            completionText.text = "Stage 4 Complete";
            yield return new WaitForSeconds(3f);
            completionText.text = "";
        }

        // Check if the player object is assigned
        if (player != null)
        {
            Vector3 initialPosition = player.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < flyAwayDuration)
            {
                player.transform.position = Vector3.Lerp(initialPosition, playerAwayPosition, elapsedTime / flyAwayDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the player reaches the final position
            player.transform.position = playerAwayPosition;

            // Wait for a brief moment to allow explosion animations
            yield return new WaitForSeconds(1f);

            // Load the next scene after the fly-away animation and explosions
            LoadNextScene();
        }
        else
        {
            Debug.LogError("Player object is not assigned in the ScoreKepper3 script.");
        }
    }

    private void LoadNextScene()
    {
        // Add your logic to load the next scene here
        // For example, load the scene with build index 5
        SceneManager.LoadScene(5);
    }

    public static void Reset()
    {
        score = 0;
    }
}
