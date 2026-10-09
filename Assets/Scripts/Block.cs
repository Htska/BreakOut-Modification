using UnityEngine;

public class Block : MonoBehaviour
{

    public GameObject capsule;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            int prob = Random.Range(0,10);
            if(prob <= 1)
            {
                Instantiate(capsule,this.transform.position,capsule.transform.rotation);
            }
            GameManager.instance.BlockDestroy();
            AudioManager.instance.PlayBlockSFX();
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //Color randomColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
        //GetComponent<Renderer>().material.color = randomColor;
    }
}
