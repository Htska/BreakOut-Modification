using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 12f;
    public float minX = -6f;
    public float maxX = 6f;
    private float xTransform = 2.5f;
    private float limits = 6f;
    public Rigidbody rb;
    public float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x,minX,maxX);
        rb.MovePosition(newPosition);
    }

    public void Scale(float scaleX, float minX, float maxX)
    {
        transform.localScale = new Vector3(scaleX,transform.localScale.y, transform.localScale.z);
        this.minX = minX;
        this.maxX = maxX;
    }

    public void ResetScale()
    {
        transform.localScale = new Vector3(xTransform,transform.localScale.y, transform.localScale.z);
        this.minX = -limits;
        this.maxX = limits;
    }

    public void MultiplySpeed(float multiply)
    {
        speed *= multiply;
    }
}
