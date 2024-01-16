using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject EnemyLaser;
    public GameObject explosionPrefab;
    public float health = 150f;
    public float enemyLaserSpeed = 10f;
    public float shotsPerSeconds = 0.5f;
    public int scoreValue = 150;

    // Use an array to store multiple score keepers instead of individual variables
    private ScoreKepper[] scoreKeepers;

    public AudioClip fireSound;
    public AudioClip deathSound;

    public float blinkDuration = 0.2f;
    public Color blinkColor = Color.red;

    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.2f;

    private bool isBlinking = false;
    private Color originalColor;

    private bool shouldRespawn = true; // Flag to determine if the enemy should respawn

    void Start()
    {
        // Find GameObjects with the "Score" tags and get their components
        GameObject[] scoreGameObjects = GameObject.FindGameObjectsWithTag("Score");

        // Initialize the scoreKeepers array with the length of the scoreGameObjects array
        scoreKeepers = new ScoreKepper[scoreGameObjects.Length];

        for (int i = 0; i < scoreGameObjects.Length; i++)
        {
            if (scoreGameObjects[i] != null)
            {
                scoreKeepers[i] = scoreGameObjects[i].GetComponent<ScoreKepper>();
            }
        }

        // Get the enemy's SpriteRenderer component
        SpriteRenderer enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;

        // Check if the target score has been reached
        foreach (ScoreKepper scoreKeeper in scoreKeepers)
        {
            if (scoreKeeper != null && scoreKeeper.HasReachedTargetScore())
            {
                shouldRespawn = false;
                break; // No need to check further if one of the score keepers has reached the target
            }
        }

        // Disable the GameObject if it should not respawn
        if (!shouldRespawn)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        float probability = Time.deltaTime * shotsPerSeconds;

        if (Random.value < probability)
        {
            Fire();
        }
        if (isBlinking)
        {
            StartCoroutine(BlinkEffect());
        }
    }

    void Fire()
    {
        GameObject enemyMissile = Instantiate(EnemyLaser, transform.position, Quaternion.identity) as GameObject;

        enemyMissile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);

        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Laser missile = collider.gameObject.GetComponent<Laser>();

        if (missile)
        {
            health -= missile.getDamage();
            missile.Hit();

            if (health <= 0)
            {
                EnemyDeath();
            }
            else
            {
                StartCoroutine(StartBlink());
                StartCoroutine(StartShake());
            }
        }
    }

    void EnemyDeath()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Iterate through the score keepers and award the score to each valid one
        foreach (ScoreKepper scoreKeeper in scoreKeepers)
        {
            if (scoreKeeper != null)
            {
                scoreKeeper.Score(scoreValue);
            }
        }

        if (shouldRespawn)
        {
            // Only destroy the enemy if it should respawn
            Destroy(gameObject);
        }
        else
        {
            // If shouldRespawn is false, deactivate the GameObject instead of destroying
            gameObject.SetActive(false);
        }
    }

    // Function to set the respawn flag
    public void SetRespawn(bool respawn)
    {
        shouldRespawn = respawn;
    }

    IEnumerator StartBlink()
    {
        isBlinking = true;
        yield return new WaitForSeconds(blinkDuration);
        isBlinking = false;
        GetComponent<SpriteRenderer>().color = originalColor; // Reset the color after blinking
    }

    IEnumerator BlinkEffect()
    {
        GetComponent<SpriteRenderer>().color = blinkColor; // Change the color during blinking
        yield return null;
    }

    IEnumerator StartShake()
    {
        float originalX = transform.position.x;
        float originalY = transform.position.y;

        float shakeTimer = 0f;

        while (shakeTimer < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            transform.position = new Vector3(originalX + offsetX, originalY + offsetY, transform.position.z);

            shakeTimer += Time.deltaTime;
            yield return null;
        }

        // Reset position after shaking
        transform.position = new Vector3(originalX, originalY, transform.position.z);
    }
}
