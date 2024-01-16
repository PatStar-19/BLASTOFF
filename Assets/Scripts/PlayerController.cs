using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 15.0f;
    public float padding = 1f;
    public GameObject Laser;
    public float laserSpeed = 5f;
    public float firingRate = 0.2f;
    public float health = 250f;
    public Slider healthSlider;
    public float blinkDuration = 0.2f; // Duration of the blink in seconds
    public Color blinkColor = Color.red; // Color to blink to
    public float shakeDuration = 0.3f; // Duration of the shake in seconds
    public float shakeIntensity = 0.1f; // Intensity of the shake

    private bool isBlinking = false;
    private bool isShaking = false;
    private Color originalColor;
    private SpriteRenderer playerRenderer;

    float xMin;
    float xMax;

    public AudioClip fireSound;

    void Start()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;

        // Initialize health bar
        healthSlider.maxValue = health;
        healthSlider.value = health;

        // Get the player's SpriteRenderer component
        playerRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerRenderer.color;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        // Move left and right
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        // Move up and down
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }

        // Clamp the player's position within the boundaries
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        float newY = Mathf.Clamp(transform.position.y, float.MinValue, float.MaxValue);
        transform.position = new Vector3(newX, newY, transform.position.z);

        // Check if the player is blinking and update the blink effect
        if (isBlinking)
        {
            StartCoroutine(BlinkEffect());
        }

        // Check if the player is shaking and update the shake effect
        if (isShaking)
        {
            StartCoroutine(ShakeEffect());
        }
    }

    void Fire()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject laserAway = Instantiate(Laser, transform.position + offset, Quaternion.identity) as GameObject;
        laserAway.GetComponent<Rigidbody2D>().velocity = new Vector3(0, laserSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Laser missile = collider.gameObject.GetComponent<Laser>();

        if (missile)
        {
            health -= missile.getDamage();
            healthSlider.value = health; // Update the health bar

            missile.Hit();

            if (health <= 0)
            {
                PlayerDeath();
            }
            else
            {
                // Trigger the blink and shake effects
                StartCoroutine(StartBlink());
                StartCoroutine(StartShake());
            }
        }
    }

    void PlayerDeath()
    {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);
    }

    IEnumerator StartBlink()
    {
        isBlinking = true;
        yield return new WaitForSeconds(blinkDuration);
        isBlinking = false;
        playerRenderer.color = originalColor; // Reset the color after blinking
    }

    IEnumerator BlinkEffect()
    {
        playerRenderer.color = blinkColor; // Change the color during blinking
        yield return null;
    }

    IEnumerator StartShake()
    {
        isShaking = true;
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
    }

    IEnumerator ShakeEffect()
    {
        Vector3 originalPosition = transform.position;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x + Random.Range(-shakeIntensity, shakeIntensity);
            float y = originalPosition.y + Random.Range(-shakeIntensity, shakeIntensity);
            transform.position = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset position after shaking
    }
}