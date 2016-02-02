using UnityEngine;
using System.Collections;

public class PickupCollect : MonoBehaviour {

    public int pointAmount;
    public int rotateAngle;
    public float rotateSpeed;
    public float bobHeight;
    public float bobSpeed;
    public float timer;
    public int itemIndex;

    void Start()
    {
        StartCoroutine(wiggleAnimation());
    }
    IEnumerator wiggleAnimation()
    {
        float currentAngle = 0;
        float currentHeight = 0;
        float startHeight = transform.position.y;
        //bobHeight += currentHeight;
        while (true)
        {
            currentAngle = Mathf.PingPong(Time.time * rotateSpeed, rotateAngle * 2);
            currentHeight = Mathf.PingPong(Time.time * bobSpeed, bobHeight);
            gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, currentAngle - rotateAngle);
            gameObject.transform.position = new Vector3(transform.position.x, currentHeight + startHeight, transform.position.z);
            yield return new WaitForSeconds(timer);
        }
        //yield return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControl>().SendMessage("AddPoints", pointAmount);
            Destroy(gameObject);
        }
    }
}
