using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeY;

    private Vector3 startPosition;

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeY);
        Vector3 newPosVect = new Vector3(transform.position.x, newPosition, transform.position.z);
        transform.position = startPosition + newPosVect;
	}
}
