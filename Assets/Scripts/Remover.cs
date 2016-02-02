using UnityEngine;
using System.Collections;

public class Remover : MonoBehaviour
{
	public GameObject splash;


	void OnTriggerEnter2D(Collider2D col)
	{
        if (col.CompareTag("Enemy"))
            Destroy(col.gameObject);
		// If the player hits the trigger...
		if(col.CompareTag("Player"))
		{
			// .. stop the camera tracking the player
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;

			// .. stop the Health Bar following the player
			//if(GameObject.FindGameObjectWithTag("HealthBar").activeSelf)
			//{
			//	GameObject.FindGameObjectWithTag("HealthBar").SetActive(false);
			//}

			// ... instantiate the splash where the player falls in.
			//Instantiate(splash, col.transform.position, transform.rotation);
			// ... destroy the player.
			Destroy (col.gameObject);
			// ... reload the level.
			StartCoroutine("ReloadGame");
		}
		else
		{
			// ... instantiate the splash where the enemy falls in.
			//Instantiate(splash, col.transform.position, transform.rotation);

			// Destroy the enemy.
			//Destroy (col.gameObject);	
		}
	}

	IEnumerator ReloadGame()
	{			
		// ... pause briefly
		yield return new WaitForSeconds(2);
		// ... and then reload the level.
		Application.LoadLevel(Application.loadedLevel);
	}
}
