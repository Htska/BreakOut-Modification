using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 16;
    private int currentHealth;
    private bool isPhaseTwo = false;
    public Transform ballTransform;
    public float dodgeSpeed = 10f;
    public float invincibilityDuration = 0.5f;
    private float lastHitTime = -10f;
    private SpriteRenderer bossRenderer;
    private Color originalColor;
    public float actionPoint = 5f;
    public Sprite spriteCenter;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    [Header("SecondPhase")]
    public Sprite low2;
    public Sprite low2Right;
    public Sprite low2Left;
    public Sprite right2;
    public Sprite left2;
    public Sprite top2;
    public Sprite top2Right;
    public Sprite top2Left;
    [Header("Throwable")]
    public GameObject bulletPrefab;
    public GameObject capsulePrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        ballTransform = GameManager.instance.ogBall.transform;

        bossRenderer = GetComponent<SpriteRenderer>();
        originalColor = bossRenderer.material.color;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Time.time >= lastHitTime + invincibilityDuration)
            {
                AudioManager.instance.PlayBlockSFX();
                lastHitTime = Time.time;
                TakeDamage();
                collision.gameObject.GetComponent<Ball>().SumSpeed(5f);

                bossRenderer.material.color = Color.red;
            }
        }
    }

    void TakeDamage()
    {
        GameManager.instance.GainPoints();
        currentHealth--;
        if (currentHealth <= maxHealth/2 && !isPhaseTwo)
        {
            StartPhaseTwo();
            currentHealth = 6;
        }
        else if (currentHealth <= 0)
        {
            Die();
        }
    }

    void StartPhaseTwo()
    {
        isPhaseTwo = true;
        transform.localScale = new Vector3(4f,transform.localScale.y,transform.localScale.z);
        bossRenderer.sprite = low2;
        var extraLife = Instantiate(capsulePrefab, transform.position + new Vector3(0f, -0.5f, 0f), capsulePrefab.transform.rotation);
        extraLife.GetComponent<Capsule>().setType(2);
        StartCoroutine(ThrowsObject());
        AudioManager.instance.MusicLowPitch(0.7f);

    }

    void Die()
    {
        Destroy(gameObject);
        GameManager.instance.WinGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastHitTime + invincibilityDuration)
        {
            bossRenderer.material.color = originalColor;
        }

        if(isPhaseTwo && ballTransform != null)
        {
            UpdateSpriteEyesSecondPhase();
            if (ballTransform.position.y < actionPoint)
            {
                float step = dodgeSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, transform.position.y, transform.position.z), step);
            }
            else
            {

                float direction = 0f;

                if (ballTransform.position.x < transform.position.x -0.5f)
                {
                    direction = 1f;
                }
                else if (ballTransform.position.x > transform.position.x + 0.5f)
                {
                    direction = -1f;
                }

                transform.Translate(Vector3.right * direction * dodgeSpeed * Time.deltaTime);

                float minX = -5f;
                float maxX = 5f;
                float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            UpdateSpriteEyes();
        }
    }

    private void UpdateSpriteEyes()
    {
        if (ballTransform == null)
        {
            bossRenderer.sprite = spriteCenter;
            return;
        }

        float xDifference = ballTransform.position.x - transform.position.x;

        float deadzone = 1.5f;

        if (xDifference > deadzone) 
        {
            bossRenderer.sprite = spriteRight;
        }
        else if (xDifference < -deadzone) 
        {
            bossRenderer.sprite = spriteLeft;
        }
        else 
        {
            bossRenderer.sprite = spriteCenter;
        }
    }

    private void UpdateSpriteEyesSecondPhase()
    {
        if (ballTransform == null)
        {
            bossRenderer.sprite = low2;
            return;
        }

        float xDifference = ballTransform.position.x - transform.position.x;
        float yDifference = ballTransform.position.y - transform.position.y;
        float Xdeadzone = 1.5f;
        float Ydeadzone = 1f;

        if (xDifference > Xdeadzone)
        {
            if (yDifference > Ydeadzone)
            {
                bossRenderer.sprite = top2Right;
            }
            else if (yDifference < -Ydeadzone)
            {
                bossRenderer.sprite = low2Right;
            }
            else
            {
                bossRenderer.sprite = right2;
            }
        }
        else if (xDifference < -Xdeadzone)
        {
            if (yDifference > Ydeadzone)
            {
                bossRenderer.sprite = top2Left;
            }
            else if (yDifference < -Ydeadzone)
            {
                bossRenderer.sprite = low2Left;
            }
            else
            {
                bossRenderer.sprite = left2;
            }
        }
        else
        {
             if (yDifference > Ydeadzone)
            {
                bossRenderer.sprite = top2;
            }
            else
            {
                bossRenderer.sprite = low2;
            }
        }


    }

    IEnumerator ThrowsObject()
    {
        while (isPhaseTwo)
        {

            float waitTime = Random.Range(2f, 4.5f);
            yield return new WaitForSeconds(waitTime);

            int roll = Random.Range(1, 5);

            Vector3 spawnPosition = transform.position + new Vector3(0f, -0.5f, 0f);

            if (roll <= 3) 
            {
                Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            }
            else 
            {
                Instantiate(capsulePrefab, spawnPosition, capsulePrefab.transform.rotation);
            }
        }
    }
}
