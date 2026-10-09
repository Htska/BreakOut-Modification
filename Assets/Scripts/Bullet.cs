using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20f;
    private Vector3 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player == null)
        {
            Destroy(this.gameObject);
            return;
        }
        Vector3 playerPosition = player.transform.position;
        direction = (playerPosition - transform.position).normalized;
        transform.up = -direction;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime,Space.World);

        if (transform.position.y <= -3f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.LoseLife();
            Destroy(this.gameObject);
        } 
        else if (other.CompareTag("DeadZone"))
        {
            Destroy(this.gameObject);
        }
    }
}
