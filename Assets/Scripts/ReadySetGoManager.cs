using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadySetGoManager : MonoBehaviour
{
    public Text transitionText;
    public GameObject panel;
    public EnemeySpawn enemySpawnScript;

    private void Start()
    {
        StartCoroutine(TransitionSequence());
    }

    IEnumerator TransitionSequence()
    {
        // Disable the EnemySpawn script during the countdown
        enemySpawnScript.enabled = false;

        // Find the Canvas with the "UITag" and disable it during the countdown
        GameObject canvasObject = GameObject.FindGameObjectWithTag("UITag");
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.enabled = false;
        
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

        // Enable the EnemySpawn script after the countdown
        enemySpawnScript.enabled = true;

        // Hide the panel after the sequence
        panel.SetActive(false);

        // Show the Canvas again after the sequence
        canvas.enabled = true;
    }
}
