using System.Collections;
using UnityEngine;

public class Capsule : MonoBehaviour
{

    public float speed = 7f;
    public Transform currentBall;
    public GameObject prefabBall;
    public int type = 0;
    private bool isCollected = false;
    [Header("Sprites")]
    public Sprite[] sprites;

    private void Awake()
    {
        int roll = Random.Range(0,12);
        if (roll < 2) 
        {
            type = 0;
        }
        else if (roll < 5) 
        {
            type = 1;
        }
        else if (roll < 6) 
        {
            type = 2;
        }
        else if (roll < 9) 
        {
            type = 3;
        }
        else 
        {
            type = 4;
        }
        GetComponent<SpriteRenderer>().sprite = sprites[type];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBall = GameObject.FindGameObjectWithTag("Ball").transform;
    }

    public void setType(int newType)
    {
        type = newType;
        GetComponent<SpriteRenderer>().sprite = sprites[type];
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollected) return;
        this.transform.Translate(Vector3.right*-1*speed*Time.deltaTime);
        if(this.gameObject.transform.position.y <= -3)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;

            switch (type)
            {
                case 0:
                    Debug.Log("PowerUp 1: Multi-Ball");
                    StartCoroutine(MultiBall());
                    break;
                case 1:
                    Debug.Log("PowerUp 2: Ball Extra-Speed");
                    StartCoroutine(ExtraSpeed());
                    break;
                case 2:
                    Debug.Log("PowerUp 3: Extra Life");
                    GameManager.instance.GainLife();
                    Destroy(this.gameObject);
                    break;
                case 3:
                    Debug.Log("PowerUp 4: Bigger Pad");
                    StartCoroutine(ChangePlayerScale(6f,-4.4f,4.4f));
                    break;
                case 4:
                    Debug.Log("PowerUp 5: Slower Pad");
                    StartCoroutine(PlayerSlowSpeed());
                    break;
            }
        }
    }

    IEnumerator MultiBall()
    {
        var newBall1 = Instantiate(prefabBall,currentBall.position,Quaternion.identity);
        newBall1.GetComponent<Ball>().Launch();
        var newBall2 = Instantiate(prefabBall,currentBall.position,Quaternion.identity);
        newBall2.GetComponent<Ball>().Launch();
        yield return new WaitForSeconds(7f);
        Destroy(newBall1.gameObject);
        Destroy(newBall2.gameObject);
        Destroy(this.gameObject);

    }

    IEnumerator ExtraSpeed()
    {
        currentBall.gameObject.GetComponent<Ball>().MultiplySpeed(2f);
        yield return new WaitForSeconds(3f);
        currentBall.gameObject.GetComponent<Ball>().RevertSpeed();
        Destroy(this.gameObject);
    }

    IEnumerator ChangePlayerScale(float newScale, float newMin, float newMax)
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.Scale(newScale,newMin,newMax);
        yield return new WaitForSeconds(7f);
        if(player != null)
            player.ResetScale();
        Destroy(this.gameObject);
    }

    IEnumerator PlayerSlowSpeed()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.MultiplySpeed(0.5f);
        yield return new WaitForSeconds(3f);
        if(player != null)
            player.MultiplySpeed(2f);
        Destroy(this.gameObject);
    }
}
