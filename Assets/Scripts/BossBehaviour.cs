using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Add this line to include the UI namespace

public class BossBehaviour : MonoBehaviour
{
    public GameObject EnemyLaser;
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public float health = 0f; // Set initial health to 20000
    public float enemyLaserSpeed = 10f;
    public float shotsPerSeconds = 0.5f;
    public int scoreValue = 150;

    private ScoreKepper scoreKeeper;
    private ScoreKepper2 scoreKeeper2;
    private ScoreKepper3 scoreKeeper3;
    private ScoreKepper4 scoreKeeper4;

    // Enemy sound
    public AudioClip fireSound;
    public AudioClip deathSound;

    public Slider healthSlider; // Reference to the UI Slider

    // Blink effect parameters
    public float blinkDuration = 0.2f; // Duration of the blink in seconds
    public Color blinkColor = Color.red; // Color to blink to

    // Shake effect parameters
    public float shakeDuration = 0.5f; // Increase duration to half a second
    public float shakeIntensity = 0.2f; // Increase intensity for a more noticeable shake

    private bool isBlinking = false;
    private Color originalColor;

    private bool shouldRespawn = true; // Flag to determine if the enemy should respawn

    void Start()
    {
        // Find GameObjects with the "Score" and "Score2" tags and get their components
        GameObject scoreGameObject = GameObject.FindWithTag("Score");
        GameObject score2GameObject = GameObject.FindWithTag("Score2");
        GameObject score3GameObject = GameObject.FindWithTag("Score3");
        GameObject score4GameObject = GameObject.FindWithTag("Score4");

        if (scoreGameObject != null)
        {
            scoreKeeper = scoreGameObject.GetComponent<ScoreKepper>();
        }
        else
        {
            Debug.LogError("No GameObject with the 'Score' tag found.");
        }
        if (score2GameObject != null)
        {
            scoreKeeper2 = score2GameObject.GetComponent<ScoreKepper2>();
        }
        if (score3GameObject != null)
        {
            scoreKeeper3 = score3GameObject.GetComponent<ScoreKepper3>();
        }
        if (score4GameObject != null)
        {
            scoreKeeper4 = score4GameObject.GetComponent<ScoreKepper4>();
        }
        else
        {
            Debug.LogError("No GameObject with the 'Score2' tag found.");
        }

        // Get the enemy's SpriteRenderer component
        SpriteRenderer enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;

        // Find the UI Slider in the scene
        healthSlider = GameObject.FindObjectOfType<Slider>();

        if (healthSlider == null)
        {
            Debug.LogError("Health Slider not found in the scene!");
        }
        else
        {
            // Set the initial and maximum values of the healthSlider
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }

        // Check if the target score has been reached
        if ((scoreKeeper != null && scoreKeeper.HasReachedTargetScore()) || (scoreKeeper2 != null && scoreKeeper2.HasReachedTargetScore()) || (scoreKeeper3 != null && scoreKeeper3.HasReachedTargetScore()) || (scoreKeeper4 != null && scoreKeeper4.HasReachedTargetScore()))
        {
            shouldRespawn = false;
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

        // Check if the enemy is blinking and update the blink effect
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
            healthSlider.value = health; // Update the UI Slider
            missile.Hit();

            if (health <= 0)
            {
                EnemyDeath();
            }
            else
            {
                // Trigger the blink and shake effect when hit
                StartCoroutine(StartBlink());
                StartCoroutine(StartShake());
            }
        }
    }

    void EnemyDeath()
    {
        // Play explosion sound
        AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // Instantiate explosion animation
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Use the correct references to ScoreKepper and ScoreKepper2 and call the Score methods
        if (scoreKeeper != null)
        {
            scoreKeeper.Score(scoreValue);
        }
        else
        {
            Debug.LogError("ScoreKepper reference is null in EnemyBehaviour.");
        }

        if (scoreKeeper2 != null)
        {
            scoreKeeper2.Score(scoreValue);
        }

        if (scoreKeeper3 != null)
        {
            scoreKeeper3.Score(scoreValue);
        }

        if (scoreKeeper4 != null)
        {
            scoreKeeper4.Score(scoreValue);
        }
        else
        {
            Debug.LogError("ScoreKepper2 reference is null in EnemyBehaviour.");
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
        float shakeDuration = 0.3f;
        float shakeIntensity = 0.1f;

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
