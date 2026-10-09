using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Referencia al Pad")]
    public Transform padTransform;
    public Vector3 offsetFromPad = new Vector3(0f,0.65f,0f); 
    public float launchSpeed = 15f;
    private float baseLaunchSpeed;
    public bool launched = false;
    private List<float> revertSpeed;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        padTransform = GameObject.Find("Player").transform;
        baseLaunchSpeed = launchSpeed;
        revertSpeed = new List<float>();
    }
    
    void FollowPad()
    {
        transform.position = padTransform.position + offsetFromPad;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (launched)
        {
            Vector3 currentVelocity = rb.linearVelocity;

            if (Mathf.Abs(currentVelocity.x) < 1.0f) 
            {
                currentVelocity.x = currentVelocity.x >= 0f ? 1.0f : -1.0f;
            }
            if (Mathf.Abs(currentVelocity.y) < 1.0f)
            {
                currentVelocity.y = currentVelocity.y >= 0f ? 1.0f : -1.0f;
            }
            rb.linearVelocity = currentVelocity.normalized * launchSpeed;
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            if(GameManager.instance.ogBall == this.gameObject)
            {
                GameManager.instance.LoseLife();
                ResetBall();
            } else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Launch()
    {
        //En diagonal
        //rb.linearVelocity = new Vector3(0.7f,0.7f,0f).normalized * launchSpeed;
        //rb.linearVelocity = Vector3.up * launchSpeed;
        float angle = Random.Range(10f,170f);
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians),0f);
        rb.linearVelocity = direction.normalized * launchSpeed;
        launched = !launched;
    }

    private void Update()
    {
        if (!launched)
        {
            FollowPad();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        launched = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        launchSpeed = baseLaunchSpeed;
        revertSpeed.Clear();
        FollowPad();
    }

    public void MultiplySpeed(float multiply)
    {
        launchSpeed *= multiply;
        revertSpeed.Add(1/multiply);
    }

    public void SumSpeed(float sum)
    {
        if(launchSpeed + sum <= 70f)
            launchSpeed += sum;
        
    }

    public void RevertSpeed()
    {
        if (revertSpeed.Count == 0)
            return;
        launchSpeed *= revertSpeed[0];
        revertSpeed.RemoveAt(0);
    }

}
