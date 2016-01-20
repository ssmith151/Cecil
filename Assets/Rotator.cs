using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float rotation;
	public float waittime;
	// Use this for initialization
	void Start () {
		StartCoroutine (rotate());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator rotate() {
		while (true) {
			transform.localEulerAngles += new Vector3(0.0f, 0.0f, transform.rotation.z + rotation);

			//gameObject.transform.rotation = Quaternion.Euler (gameObject.transform.rotation.x, 
			//	gameObject.transform.rotation.y, gameObject.transform.rotation.z + rotation );
			yield return new WaitForSeconds (waittime); 
	
		}
	}
}
