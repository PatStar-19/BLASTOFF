using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadySetGoBossManager : MonoBehaviour
{
    public Text transitionText;
    public BossSpawn enemySpawnScript;

    private void Start()
    {
        StartCoroutine(TransitionSequence());
    }

    IEnumerator TransitionSequence()
    {
        // Find the Canvas with the "UITag" and disable it during the countdown
        GameObject canvasObject = GameObject.FindGameObjectWithTag("UITag");
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.enabled = false;

        // Disable the EnemySpawn script during the countdown
        enemySpawnScript.enabled = false;

        yield return new WaitForSeconds(1f); // Wait for 1 second before starting the countdown

        for (int i = 3; i > 0; i--)
        {
            transitionText.text = i.ToString();
            yield return new WaitForSeconds(1f); // Wait for 1 second between countdown numbers
        }

        // Customize the "Blast Off!" text properties
        transitionText.rectTransform.localScale = new Vector3(2f, 2f, 1f); // Adjust the scale as needed

        // Set the position slightly lower
        transitionText.rectTransform.anchoredPosition += new Vector2(0f, -100f); // Adjust the Y value as needed

        transitionText.text = "Blast Off!";

        yield return new WaitForSeconds(1f); // Wait for 1 second after the countdown

        // Hide the "Blast Off!" text after the sequence
        transitionText.gameObject.SetActive(false);

        // Enable the EnemySpawn script after the countdown
        enemySpawnScript.enabled = true;

        // Show the Canvas again after the sequence
        canvas.enabled = true;
    }
}
