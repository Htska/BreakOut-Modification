using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int lifes = 3;
    public int points = 0;
    public Block[] blocks;
    public int blockCount = 0;
    [Header("UI")]
    public TMP_Text pointsText;
    public TMP_Text lifesText;
    public GameObject ogBall;
    public GameObject finalBoss;
    public GameObject endGameCanva;
    public GameObject winGameCanva;


    private void Awake()
    {
        Application.targetFrameRate = 120;
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blocks = FindObjectsByType<Block>();
        blockCount = blocks.Length;
        ogBall = FindAnyObjectByType<Ball>().gameObject;
        UpdateUI();
    }

    public void BlockDestroy()
    {
        blockCount--;
        points += 100;
        UpdateUI();
        if (blockCount <= 0)
        {
            finalBoss.SetActive(true);
            ogBall.GetComponent<Ball>().ResetBall();
            AudioManager.instance.MusicLowPitch(0.5f);
        }
    }

    public void LoseLife()
    {
        AudioManager.instance.PlayDamageSFX();
        lifes--;
        UpdateUI();
        if(lifes <= 0)
            EndGame();
    }

    public void GainLife()
    {
        lifes++;
        UpdateUI();
    }

    public void EndGame()
    {
        GameObject.Find("Player").SetActive(false);
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
        finalBoss.SetActive(false);
        AudioManager.instance.EndSong();
        endGameCanva.SetActive(true);
    }

    public void WinGame()
    {
        GameObject.Find("Player").SetActive(false);
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
        finalBoss.SetActive(false);
        AudioManager.instance.EndSong();
        winGameCanva.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GainPoints()
    {
        points+=100;
        UpdateUI();
    }
    void UpdateUI()
    {
        pointsText.text = $"Puntos: {points}";
        lifesText.text = $"Vidas: {lifes}";
    }
}
